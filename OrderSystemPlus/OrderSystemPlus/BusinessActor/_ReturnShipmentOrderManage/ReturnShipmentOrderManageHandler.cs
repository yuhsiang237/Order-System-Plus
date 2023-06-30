using System.Transactions;

using AutoMapper;

using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Models.BusinessActor;
using OrderSystemPlus.Models.DataAccessor;
using OrderSystemPlus.Utils.OrderNumberTool;

namespace OrderSystemPlus.BusinessActor
{
    public class ReturnShipmentOrderManageHandler : IReturnShipmentOrderManageHandler
    {
        private readonly IReturnShipmentOrderRepository _ReturnShipmentOrderRepository;
        private readonly IProductInventoryManageHandler _productInventoryManageHandler;
        private readonly IProductRepository _productRepository;
        private static SemaphoreSlim _createOrderSemaphoreSlim;

        public ReturnShipmentOrderManageHandler(
            IReturnShipmentOrderRepository ReturnShipmentOrderRepository,
            IProductRepository productRepository,
            IProductInventoryManageHandler productInventoryManageHandler)
        {
            _createOrderSemaphoreSlim = new SemaphoreSlim(1, 1);
            _ReturnShipmentOrderRepository = ReturnShipmentOrderRepository;
            _productRepository = productRepository;
            _productInventoryManageHandler = productInventoryManageHandler;
        }
        public async Task<List<RspGetReturnShipmentOrderList>> GetReturnShipmentOrderListAsync(ReqGetReturnShipmentOrderList req)
        {
            var data = await _ReturnShipmentOrderRepository.FindByOptionsAsync();
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
            var data = (await _ReturnShipmentOrderRepository.FindByOptionsAsync(returnShipmentOrderNumber: req.ReturnShipmentOrderNumber)).FirstOrDefault();
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
            await _createOrderSemaphoreSlim.WaitAsync();
            try
            {
                var returnShipmentOrderNumber = "";
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                scope.Complete();
                return returnShipmentOrderNumber;
            }
            finally
            {
                _createOrderSemaphoreSlim.Release();
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
            await _ReturnShipmentOrderRepository.DeleteAsync(dtoList);
        }
    }
}
