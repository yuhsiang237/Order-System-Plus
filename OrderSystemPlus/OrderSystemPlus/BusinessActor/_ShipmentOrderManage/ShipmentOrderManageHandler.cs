using AutoMapper;

using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Models.BusinessActor;
using OrderSystemPlus.Models.DataAccessor;
using OrderSystemPlus.Utils.OrderNumberTool;
using System.Transactions;

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

        public async Task<RspGetShipmentOrderInfo> GetShipmentOrderInfoAsync(ReqGetShipmentOrderInfo req)
        {
            var data = (await _ShipmentOrderRepository.FindByOptionsAsync(orderNumber: req.OrderNumber)).FirstOrDefault();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ShipmentOrderDto, RspGetShipmentOrderInfo>();
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
                var orderNumber = OrderNumberTool.GenerateNumber(OrderNumberTool.Type.Shipment);
                var now = DateTime.Now;
                // Deduct product inventory
                var updateProductInventory = req?.Details
                                                ?.Select(x =>
                                                    new ReqUpdateProductInventory
                                                    {
                                                        Type = Enums.AdjustProductInventoryType.IncreaseDecrease,
                                                        ProductId = x.ProductId,
                                                        AdjustQuantity = x.ProductQuantity,
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
                    var product = (await _productRepository.FindByOptionsAsync()).FirstOrDefault();
                    if (product != null)
                    {
                        totalAmount += product.Price * item.ProductQuantity.Value;
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
                           Status = 1, // TODO
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
