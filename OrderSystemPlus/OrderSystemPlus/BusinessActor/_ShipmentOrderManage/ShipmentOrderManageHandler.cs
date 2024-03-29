﻿using System.Transactions;

using AutoMapper;

using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Models.BusinessActor;
using OrderSystemPlus.Models.DataAccessor;
using OrderSystemPlus.Utils.OrderNumberTool;

namespace OrderSystemPlus.BusinessActor
{
    public class ShipmentOrderManageHandler : IShipmentOrderManageHandler
    {
        private readonly IShipmentOrderRepository _ShipmentOrderRepository;
        private readonly IProductInventoryManageHandler _productInventoryManageHandler;
        private readonly IProductRepository _productRepository;
        private static SemaphoreSlim _createOrderSemaphoreSlim;

        public ShipmentOrderManageHandler(
            IShipmentOrderRepository ShipmentOrderRepository,
            IProductRepository productRepository,
            IProductInventoryManageHandler productInventoryManageHandler)
        {
            _createOrderSemaphoreSlim = new SemaphoreSlim(1, 1);
            _ShipmentOrderRepository = ShipmentOrderRepository;
            _productRepository = productRepository;
            _productInventoryManageHandler = productInventoryManageHandler;
        }
        public async Task<RspGetShipmentOrderList> GetShipmentOrderListAsync(ReqGetShipmentOrderList req)
        {
            var data = await _ShipmentOrderRepository.FindByOptionsAsync(
                orderNumber:req.OrderNumber,
                pageIndex: req.PageIndex,
                pageSize: req.PageSize,
                sortField: req.SortField,
                sortType: req.SortType);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ShipmentOrderDto, RspGetShipmentOrderListItem>();
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            var rsp = mapper.Map<List<ShipmentOrderDto>, List<RspGetShipmentOrderListItem>>(data.Data);
            
            return new RspGetShipmentOrderList
            {
                TotalCount = data.TotalCount,
                Data = rsp,
            };
        }

        public async Task<RspGetShipmentOrderInfo> GetShipmentOrderInfoAsync(ReqGetShipmentOrderInfo req)
        {
            var data = (await _ShipmentOrderRepository.FindByOptionsAsync(
                orderNumber: req.OrderNumber
                )).Data.FirstOrDefault();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ShipmentOrderDto, RspGetShipmentOrderInfo>();
                cfg.CreateMap<ShipmentOrderDetailDto, RspGetShipmentOrderInfo.ShipmentOrderDetail>();
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            var rsp = mapper.Map<ShipmentOrderDto, RspGetShipmentOrderInfo>(data);
            return rsp;
        }

        public async Task<string> HandleAsync(ReqCreateShipmentOrder req)
        {
            await _createOrderSemaphoreSlim.WaitAsync();
            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var orderNumber = string.Empty;
                while (string.IsNullOrEmpty(orderNumber))
                {
                    var newOrderNumber = OrderNumberTool.GenerateNumber(OrderNumberTool.Type.Shipment);
                    var isExistOrderNumber = (await _ShipmentOrderRepository.FindByOptionsAsync(newOrderNumber)).Data.Any();
                    if (isExistOrderNumber == false)
                        orderNumber = newOrderNumber;
                }

                var now = DateTime.Now;
                // Deduct product inventory
                var updateProductInventory = req?.Details
                                                ?.Select(x =>
                                                    new ReqUpdateProductInventory
                                                    {
                                                        Type = Enums.AdjustProductInventoryType.IncreaseDecrease,
                                                        ProductId = x.ProductId,
                                                        AdjustQuantity = -1 * Math.Abs(x.ProductQuantity),
                                                        Description = $"ShipmentOrder : {orderNumber}。",
                                                    })
                                                .ToList();
                var rsp = await _productInventoryManageHandler.HandleAsync(updateProductInventory);
                if (rsp == false)
                    throw new Exception("productInventory error");

                // Insert order
                var details = new List<ShipmentOrderDetailDto>();
                var totalAmount = 0M;
                foreach (var item in req?.Details)
                {
                    var product = (await _productRepository
                        .FindByOptionsAsync(id: item.ProductId))
                        .Data.FirstOrDefault();

                    if (product != null)
                    {
                        totalAmount += product.Price * item.ProductQuantity;
                        details.Add(new ShipmentOrderDetailDto
                        {
                            OrderNumber = orderNumber,
                            ProductId = product.Id,
                            ProductNumber = product.Number,
                            ProductName = product.Name,
                            ProductPrice = product.Price,
                            ProductQuantity = item.ProductQuantity,
                            Remarks = item.Remarks,
                            UpdatedOn = now,
                            CreatedOn = now,
                            IsValid = true,
                        });
                    }
                    else
                    {
                        throw new Exception($"NOT FOUND PRODUCT ID : {item.ProductId}");
                    }
                }

                await _ShipmentOrderRepository.InsertAsync(new List<ShipmentOrderDto>
                    {
                       new ShipmentOrderDto
                       {
                           OrderNumber = orderNumber,
                           TotalAmount = totalAmount,
                           OperatorUserId = 123, // TODO
                           RecipientName = req.RecipientName,
                           Status = req.FinishDate == null ? Enums.ShipmentOrderStatus.Processing : Enums.ShipmentOrderStatus.Finish,
                           FinishDate = req.FinishDate,
                           DeliveryDate = req.DeliveryDate,
                           Address = req.Address,
                           Remark = req.Remark,
                           Details = details,
                           CreatedOn = now,
                           UpdatedOn = now,
                           IsValid = true,
                       }
                    });

                scope.Complete();
                return orderNumber;
            }
            finally
            {
                _createOrderSemaphoreSlim.Release();
            }
        }

        public async Task HandleAsync(ReqUpdateShipmentOrder req)
        {
            var now = DateTime.Now;
            var orderDto = (await _ShipmentOrderRepository.FindByOptionsAsync(req.OrderNumber))
                        .Data.FirstOrDefault();
            orderDto.OrderNumber = req.OrderNumber;
            orderDto.RecipientName = req.RecipientName;
            orderDto.OperatorUserId = 123; // TODO
            orderDto.Status = req.FinishDate == null ? Enums.ShipmentOrderStatus.Processing : Enums.ShipmentOrderStatus.Finish;
            orderDto.FinishDate = req.FinishDate;
            orderDto.DeliveryDate = req.DeliveryDate;
            orderDto.Address = req.Address;
            orderDto.Remark = req.Remark;
            orderDto.UpdatedOn = now;
            orderDto.Details = req?.Details?.Select(s => new ShipmentOrderDetailDto
            {
                Id = s.Id,
                Remarks = s.Remarks,
                UpdatedOn = now,
            }).ToList();

            await _ShipmentOrderRepository.UpdateAsync(new List<ShipmentOrderDto>
            {
                orderDto,
            });
        }

        public async Task HandleAsync(ReqDeleteShipmentOrder req)
        {
            var now = DateTime.Now;
            var dtoList = req?
                .OrderNumber?
                .Select(orderNumber => new ShipmentOrderDto
                {
                    OrderNumber = orderNumber,
                    UpdatedOn = now,
                }).ToList();
            await _ShipmentOrderRepository.DeleteAsync(dtoList);
        }
    }
}
