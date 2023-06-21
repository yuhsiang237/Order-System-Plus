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
    public class ShipmentOrderManageHandlerTest
    {
        private IShipmentOrderManageHandler _handler;
        private readonly Mock<IShipmentOrderRepository> _shipmentOrderRepository;
        private readonly Mock<IProductInventoryManageHandler> _productInventoryHandler;
        private readonly Mock<IProductRepository> _productRepository;
        public ShipmentOrderManageHandlerTest()
        {
            _shipmentOrderRepository = new Mock<IShipmentOrderRepository>();
            _productInventoryHandler = new Mock<IProductInventoryManageHandler>();
            _productRepository = new Mock<IProductRepository>();
            _handler = new ShipmentOrderManageHandler(
              _shipmentOrderRepository.Object,
              _productRepository.Object,
              _productInventoryHandler.Object);
        }

        [Fact]
        public async Task ShipmentOrderCreate()
        {
            _shipmentOrderRepository
                .Setup(x => x.FindByOptionsAsync(It.IsAny<string?>()))
                .ReturnsAsync(new List<ShipmentOrderDto> { });
            _productInventoryHandler.Setup(x => x.HandleAsync(It.IsAny<List<ReqUpdateProductInventory>>())).ReturnsAsync(true);
            _productRepository
               .Setup(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()))
               .ReturnsAsync(new List<ProductDto> {
                    new ProductDto{
                        Id = 1,
                        Name = "Test",
                        Description = "Test",
                        Number = "TEST",
                        Price = 500,
                    }});
            _shipmentOrderRepository.Setup(x => x.InsertAsync(It.IsAny<IEnumerable<ShipmentOrderDto>>()))
                .ReturnsAsync(new List<string> { "S20230621D1BCF3" });
            await _handler.HandleAsync(new ReqCreateShipmentOrder
            {
                RecipientName = "string",
                FinishDate = null,
                DeliveryDate = null,
                Address = "string",
                Remark = "string",
                Details = new List<ReqCreateShipmentOrder.ShipmentOrderDetailModel>
                              {
                                 new ReqCreateShipmentOrder.ShipmentOrderDetailModel
                                 {
                                     ProductId = 1,
                                     ProductQuantity = 1,
                                     Remarks = "remarks",
                                 }
                              }
            });
            _shipmentOrderRepository.Verify(x => x.FindByOptionsAsync(It.IsAny<string?>()), Times.Once());
            _productRepository.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()), Times.Once());
            _productInventoryHandler.Verify(x => x.HandleAsync(It.IsAny<List<ReqUpdateProductInventory>>()), Times.Once());
            _shipmentOrderRepository.Verify(x => x.InsertAsync(It.IsAny<IEnumerable<ShipmentOrderDto>>()), Times.Once());
        }

        [Fact]
        public async Task GetShipmentOrderListAsync()
        {
            _shipmentOrderRepository
                  .Setup(x => x.FindByOptionsAsync(It.IsAny<string?>()))
                  .ReturnsAsync(new List<ShipmentOrderDto> { });
            await _handler.GetShipmentOrderListAsync(new ReqGetShipmentOrderList
            { });
            _shipmentOrderRepository.Verify(x => x.FindByOptionsAsync(It.IsAny<string?>()), Times.Once());
        }

        public async Task GetShipmentOrderInfoAsync()
        {
            _shipmentOrderRepository
              .Setup(x => x.FindByOptionsAsync(It.IsAny<string?>()))
              .ReturnsAsync(new List<ShipmentOrderDto> { });
            await _handler.GetShipmentOrderInfoAsync(new ReqGetShipmentOrderInfo
            { });
            _shipmentOrderRepository.Verify(x => x.FindByOptionsAsync(It.IsAny<string?>()), Times.Once());
        }

        [Fact]
        public async Task ShipmentOrderUpdate()
        {
            _shipmentOrderRepository
               .Setup(x => x.FindByOptionsAsync(It.IsAny<string?>()))
               .ReturnsAsync(new List<ShipmentOrderDto> {
                   new ShipmentOrderDto {
                    OrderNumber = "S20230621D1BCF3",
                    RecipientName = "string",
                    FinishDate = null,
                    DeliveryDate = null,
                    Address = "string",
                    Remark = "string",
                 } 
               });
            _shipmentOrderRepository.Setup(x => x.UpdateAsync(It.IsAny<IEnumerable<ShipmentOrderDto>>()));

            await _handler.HandleAsync(new ReqUpdateShipmentOrder
            {
                OrderNumber = "S20230621D1BCF3",
                RecipientName = "string",
                FinishDate = null,
                DeliveryDate = null,
                Address = "string",
                Remark = "string",
            });
            _shipmentOrderRepository.Verify(x => x.UpdateAsync(It.IsAny<IEnumerable<ShipmentOrderDto>>()), Times.Once());
            _shipmentOrderRepository.Verify(x => x.FindByOptionsAsync(It.IsAny<string?>()), Times.Once());

        }

        [Fact]
        public async Task ShipmentOrderDelete()
        {
            _shipmentOrderRepository.Setup(x => x.DeleteAsync(It.IsAny<IEnumerable<ShipmentOrderDto>>()));
            await _handler.HandleAsync(
                new ReqDeleteShipmentOrder
                {
                    OrderNumber = new List<string> { "S20230621D1BCF3" },
                }
            );
            _shipmentOrderRepository.Verify(x => x.DeleteAsync(It.IsAny<IEnumerable<ShipmentOrderDto>>()), Times.Once());
        }
    }
}
