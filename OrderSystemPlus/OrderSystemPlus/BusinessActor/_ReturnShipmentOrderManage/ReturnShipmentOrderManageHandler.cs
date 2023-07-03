using System.Transactions;
using AutoMapper;

using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Models.BusinessActor;
using OrderSystemPlus.Models.DataAccessor;
using OrderSystemPlus.Utils.OrderNumberTool;
using OrderSystemPlus.Enums;

namespace OrderSystemPlus.BusinessActor
{
    public class ReturnShipmentOrderManageHandler : IReturnShipmentOrderManageHandler
    {
        private readonly IReturnShipmentOrderRepository _returnShipmentOrderRepository;
        private readonly IShipmentOrderRepository _shipmentOrderRepository;
        private readonly IProductInventoryManageHandler _productInventoryManageHandler;
        private readonly IProductRepository _productRepository;
        private static SemaphoreSlim _createReturnOrderSemaphoreSlim;

        public ReturnShipmentOrderManageHandler(
            IReturnShipmentOrderRepository returnShipmentOrderRepository,
            IShipmentOrderRepository shipmentOrderRepository,
            IProductRepository productRepository,
            IProductInventoryManageHandler productInventoryManageHandler)
        {
            _createReturnOrderSemaphoreSlim = new SemaphoreSlim(1, 1);
            _returnShipmentOrderRepository = returnShipmentOrderRepository;
            _shipmentOrderRepository = shipmentOrderRepository;
            _productRepository = productRepository;
            _productInventoryManageHandler = productInventoryManageHandler;
        }
        public async Task<List<RspGetReturnShipmentOrderList>> GetReturnShipmentOrderListAsync(ReqGetReturnShipmentOrderList req)
        {
            var data = await _returnShipmentOrderRepository.FindByOptionsAsync();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReturnShipmentOrderDto, RspGetReturnShipmentOrderList>();
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            var rsp = mapper.Map<List<ReturnShipmentOrderDto>, List<RspGetReturnShipmentOrderList>>(data);
            return rsp;
        }

        public async Task<RspGetReturnShipmentOrderInfo> GetReturnShipmentOrderInfoAsync(ReqGetReturnShipmentOrderInfo req)
        {
            var data = (await _returnShipmentOrderRepository.FindByOptionsAsync(returnShipmentOrderNumber: req.ReturnShipmentOrderNumber)).FirstOrDefault();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReturnShipmentOrderDto, RspGetReturnShipmentOrderInfo>();
                cfg.CreateMap<ReturnShipmentOrderDetailDto, RspGetReturnShipmentOrderInfo.ReturnShipmentOrderDetail>();
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            var rsp = mapper.Map<ReturnShipmentOrderDto, RspGetReturnShipmentOrderInfo>(data);
            return rsp;
        }

        public async Task<string> HandleAsync(ReqCreateReturnShipmentOrder req)
        {
            await _createReturnOrderSemaphoreSlim.WaitAsync();
            try
            {
                var now = DateTime.Now;
                // 1. Find target shipmentOrder & check unique
                var shipmentOrder = (await _shipmentOrderRepository.FindByOptionsAsync(req.ShipmentOrderNumber)).FirstOrDefault();
                if (shipmentOrder == null)
                    throw new Exception("shipmentOrder not found");
                var isExistReturnShipmentOrder = (await _returnShipmentOrderRepository.FindByOptionsAsync(shipmentOrderNumber: shipmentOrder.OrderNumber)).Any();
                if (isExistReturnShipmentOrder)
                    throw new Exception("returnShipmentOrder has been created");

                // 2. Create return order
                var returnShipmentOrderNumber = string.Empty;
                while (string.IsNullOrEmpty(returnShipmentOrderNumber))
                {
                    var newOrderNumber = OrderNumberTool.GenerateNumber(OrderNumberTool.Type.ReturnShipment);
                    var isExistOrderNumber = (await _returnShipmentOrderRepository.FindByOptionsAsync(newOrderNumber)).Any();
                    if (isExistOrderNumber == false)
                        returnShipmentOrderNumber = newOrderNumber;
                }
                var orderDto = new ReturnShipmentOrderDto
                {
                    ReturnShipmentOrderNumber = returnShipmentOrderNumber,
                    ShipmentOrderNumber = req.ShipmentOrderNumber,
                    ReturnDate = req.ReturnDate,
                    Remark = req.Remark,
                    OperatorUserId = 9999,// TODO
                    CreatedOn = now,
                    UpdatedOn = now,
                    IsValid = true,
                };

                // 3. Create details
                var detailDto = new List<ReturnShipmentOrderDetailDto>();
                var totalReturnAmount = default(decimal);
                foreach (var o in shipmentOrder?.Details)
                {
                    var reqDetail = req?.Details?.Find(f => f.ShipmentOrderDetailId == o.Id);
                    totalReturnAmount += o.ProductPrice.Value * reqDetail.ReturnProductQuantity.Value;
                    detailDto.Add(new ReturnShipmentOrderDetailDto
                    {
                        ReturnShipmentOrderNumber = returnShipmentOrderNumber,
                        ShipmentOrderDetailId = o.Id,
                        ReturnProductQuantity = reqDetail.ReturnProductQuantity,
                        Remarks = reqDetail.Remarks,
                        CreatedOn = now,
                        UpdatedOn = now,
                        IsValid = true,
                    });
                }

                orderDto.TotalReturnAmount = totalReturnAmount;
                orderDto.Details = detailDto;

                // 4.  Adjust inventory
                var reqUpdateProductInventoryList = new List<ReqUpdateProductInventory>();
                foreach (var o in shipmentOrder?.Details)
                {
                    var reqDetail = req?.Details?.Find(f => f.ShipmentOrderDetailId == o.Id);
                    if (o.ProductQuantity < reqDetail.ReturnProductQuantity)
                        throw new Exception("ReturnProductQuantity Error");
                    if (reqDetail.ReturnProductQuantity < 0)
                        throw new Exception("ReturnProductQuantity less than 0 Error");

                    reqUpdateProductInventoryList
                        .Add(new ReqUpdateProductInventory
                        {
                            Type = AdjustProductInventoryType.IncreaseDecrease,
                            ProductId = o.ProductId,
                            AdjustQuantity = reqDetail.ReturnProductQuantity,
                            Description = $"ReturnShipmentOrder : {returnShipmentOrderNumber}。",
                        });
                }

                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var rsp = await _productInventoryManageHandler.HandleAsync(reqUpdateProductInventoryList);
                if (rsp == false)
                    throw new Exception("productInventory error");
                await _returnShipmentOrderRepository.InsertAsync(
                              new List<ReturnShipmentOrderDto> { orderDto });
                scope.Complete();

                return returnShipmentOrderNumber;
            }
            finally
            {
                _createReturnOrderSemaphoreSlim.Release();
            }
        }

        public async Task HandleAsync(ReqUpdateReturnShipmentOrder req)
        {
            var now = DateTime.Now;
            //TODO
        }

        public async Task HandleAsync(ReqDeleteReturnShipmentOrder req)
        {
            var now = DateTime.Now;
            var dtoList = req?
                .ReturnShipmentOrderNumber?
                .Select(orderNumber => new ReturnShipmentOrderDto
                {
                    ReturnShipmentOrderNumber = orderNumber,
                    UpdatedOn = now,
                }).ToList();
            await _returnShipmentOrderRepository.DeleteAsync(dtoList);
        }
    }
}
