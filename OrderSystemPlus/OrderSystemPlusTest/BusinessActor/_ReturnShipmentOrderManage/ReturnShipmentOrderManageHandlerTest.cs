using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;
using Moq;

using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Models.DataAccessor;
using OrderSystemPlus.BusinessActor;
using OrderSystemPlus.Models.BusinessActor;

namespace OrderSystemPlusTest.BusinessActor
{
    public class ReturnShipmentOrderManageHandlerTest
    {
        private IReturnShipmentOrderManageHandler _handler;
        private readonly Mock<IReturnShipmentOrderRepository> _returnShipmentOrderRepository;
        private readonly Mock<IProductInventoryManageHandler> _productInventoryHandler;
        private readonly Mock<IShipmentOrderRepository> _shipmentOrderRepository;
        public ReturnShipmentOrderManageHandlerTest()
        {
            _returnShipmentOrderRepository = new Mock<IReturnShipmentOrderRepository>();
            _productInventoryHandler = new Mock<IProductInventoryManageHandler>();
            _shipmentOrderRepository = new Mock<IShipmentOrderRepository>();
            _handler = new ReturnShipmentOrderManageHandler(
              _returnShipmentOrderRepository.Object,
              _shipmentOrderRepository.Object,
              _productInventoryHandler.Object);
        }

        [Fact]
        public async Task ReturnShipmentOrderCreate()
        {
            // TODO
        }

        [Fact]
        public async Task GetReturnShipmentOrderListAsync()
        {
            _returnShipmentOrderRepository
                  .Setup(x => x.FindByOptionsAsync(It.IsAny<string?>(), It.IsAny<string?>()))
                  .ReturnsAsync(new List<ReturnShipmentOrderDto> { });
            await _handler.GetReturnShipmentOrderListAsync(new ReqGetReturnShipmentOrderList
            { });
            _returnShipmentOrderRepository.Verify(x => x.FindByOptionsAsync(It.IsAny<string?>(), It.IsAny<string?>()), Times.Once());
        }

        [Fact]
        public async Task GetReturnShipmentOrderInfoAsync()
        {
            _returnShipmentOrderRepository
              .Setup(x => x.FindByOptionsAsync(It.IsAny<string?>(), It.IsAny<string?>()))
              .ReturnsAsync(new List<ReturnShipmentOrderDto> { });
            await _handler.GetReturnShipmentOrderInfoAsync(new ReqGetReturnShipmentOrderInfo
            { });
            _returnShipmentOrderRepository.Verify(x => x.FindByOptionsAsync(It.IsAny<string?>(), It.IsAny<string?>()), Times.Once());
        }

        [Fact]
        public async Task ReturnShipmentOrderUpdate()
        {
            // TODO
        }

        [Fact]
        public async Task ReturnShipmentOrderDelete()
        {
            _returnShipmentOrderRepository.Setup(x => x.DeleteAsync(It.IsAny<IEnumerable<ReturnShipmentOrderDto>>()));
            await _handler.HandleAsync(
                new ReqDeleteReturnShipmentOrder
                {
                    ReturnShipmentOrderNumber = new List<string> { "R20230621D1BCF3" },
                }
            );
            _returnShipmentOrderRepository.Verify(x => x.DeleteAsync(It.IsAny<IEnumerable<ReturnShipmentOrderDto>>()), Times.Once());
        }
    }
}
