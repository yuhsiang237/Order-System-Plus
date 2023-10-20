using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;
using Moq;

using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Models.DataAccessor;
using OrderSystemPlus.BusinessActor;
using OrderSystemPlus.Models.BusinessActor;
using System;
using OrderSystemPlus.Enums;

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

        [Theory]
        [InlineData("R202306277890DB", "S202306277890DB")]
        public async Task ReturnShipmentOrderCreate(string returnShipmentOrderNumber, string shipmentOrderNumber)
        {
            _shipmentOrderRepository
                .Setup(x => x.FindByOptionsAsync(It.IsAny<string?>(), It.IsAny<int?>(),
               It.IsAny<int?>(),
               It.IsAny<string?>(),
               It.IsAny<SortType?>()))
                .ReturnsAsync((1,new List<ShipmentOrderDto> {
                    new ShipmentOrderDto
                    {
                        OrderNumber = shipmentOrderNumber,
                        TotalAmount = 100,
                        RecipientName = "user",
                        OperatorUserId = 123,
                        Status = OrderSystemPlus.Enums.ShipmentOrderStatus.Finish,
                        FinishDate = DateTime.Now,
                        DeliveryDate = DateTime.Now,
                        Address = "test",
                        Details = new List<ShipmentOrderDetailDto>
                        {
                            new ShipmentOrderDetailDto
                            {
                                Id = 10,
                                OrderNumber = shipmentOrderNumber,
                                ProductId = 79,
                                ProductNumber = "T1N",
                                ProductPrice = 100,
                                ProductQuantity = 1,
                            }
                        }
                    }
                }));

            _returnShipmentOrderRepository
             .Setup(x => x.FindByOptionsAsync(It.IsAny<string?>(), It.IsAny<string?>()))
             .ReturnsAsync(new List<ReturnShipmentOrderDto> { });

            _productInventoryHandler.Setup(x => x.HandleAsync(It.IsAny<List<ReqUpdateProductInventory>>())).ReturnsAsync(true);
            _returnShipmentOrderRepository.Setup(x => x.InsertAsync(It.IsAny<IEnumerable<ReturnShipmentOrderDto>>())).ReturnsAsync(new List<string>
            {
                returnShipmentOrderNumber
            });

            await _handler.HandleAsync(new ReqCreateReturnShipmentOrder
            {
                ShipmentOrderNumber = shipmentOrderNumber,
                ReturnDate = DateTime.Now,
                Remark = "test",
                Details = new List<ReqCreateReturnShipmentOrder.ReturnShipmentOrderDetail2>
                {
                    new ReqCreateReturnShipmentOrder.ReturnShipmentOrderDetail2
                    {
                        ShipmentOrderDetailId = 10,
                        ReturnProductQuantity = 1,
                        Remarks = "Test",
                    }
                }
            });

            _returnShipmentOrderRepository.Verify(x => x.InsertAsync(It.IsAny<IEnumerable<ReturnShipmentOrderDto>>()), Times.Once());
            _productInventoryHandler.Verify(x => x.HandleAsync(It.IsAny<List<ReqUpdateProductInventory>>()), Times.Once());
            _shipmentOrderRepository.Verify(x => x.FindByOptionsAsync(It.IsAny<string?>(), It.IsAny<int?>(),
               It.IsAny<int?>(),
               It.IsAny<string?>(),
               It.IsAny<SortType?>()), Times.Once());
            _returnShipmentOrderRepository
              .Verify(x => x.FindByOptionsAsync(null, shipmentOrderNumber), Times.Once());
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

        [Theory]
        [InlineData("R202306277890DB", "S202306277890DB")]
        public async Task ReturnShipmentOrderUpdate(string returnShipmentOrderNumber, string shipmentOrderNumber)
        {
            _returnShipmentOrderRepository
             .Setup(x => x.FindByOptionsAsync(It.IsAny<string?>(), It.IsAny<string?>()))
             .ReturnsAsync(new List<ReturnShipmentOrderDto> {
                 new ReturnShipmentOrderDto{
                     ReturnShipmentOrderNumber = returnShipmentOrderNumber,
                     ShipmentOrderNumber = shipmentOrderNumber,
                     TotalReturnAmount = 0,
                     ReturnDate = DateTime.Now,
                     Remark = "Test",
                     OperatorUserId = 999,
                     Details = new List<ReturnShipmentOrderDetailDto>
                     {
                         new ReturnShipmentOrderDetailDto
                         {
                             Id = 10,
                             ReturnShipmentOrderNumber = returnShipmentOrderNumber,
                             ShipmentOrderDetailId  = 10,
                             ReturnProductQuantity = 0,
                             Remarks = "Test",
                         }
                     }
                 }
             });

            _shipmentOrderRepository
                .Setup(x => x.FindByOptionsAsync(It.IsAny<string?>(), It.IsAny<int?>(),
               It.IsAny<int?>(),
               It.IsAny<string?>(),
               It.IsAny<SortType?>()))
                .ReturnsAsync((1,new List<ShipmentOrderDto> {
                    new ShipmentOrderDto
                    {
                        OrderNumber = shipmentOrderNumber,
                        TotalAmount = 100,
                        RecipientName = "user",
                        OperatorUserId = 123,
                        Status = OrderSystemPlus.Enums.ShipmentOrderStatus.Finish,
                        FinishDate = DateTime.Now,
                        DeliveryDate = DateTime.Now,
                        Address = "test",
                        Details = new List<ShipmentOrderDetailDto>
                        {
                            new ShipmentOrderDetailDto
                            {
                                Id = 10,
                                OrderNumber = shipmentOrderNumber,
                                ProductId = 79,
                                ProductNumber = "T1N",
                                ProductPrice = 100,
                                ProductQuantity = 1,
                            }
                        }
                    }
                }));

            _productInventoryHandler.Setup(x => x.HandleAsync(It.IsAny<List<ReqUpdateProductInventory>>())).ReturnsAsync(true);

            _returnShipmentOrderRepository.Setup(x => x.UpdateAsync(It.IsAny<IEnumerable<ReturnShipmentOrderDto>>()));

            await _handler.HandleAsync(new ReqUpdateReturnShipmentOrder
            {
                ReturnShipmentOrderNumber = returnShipmentOrderNumber,
                ReturnDate = DateTime.Now,
                Remark = "Test",
                Details = new List<ReqUpdateReturnShipmentOrder.ReturnShipmentOrderDetail3>
                {
                    new ReqUpdateReturnShipmentOrder.ReturnShipmentOrderDetail3
                    {
                        Id = 10,
                        ReturnProductQuantity = 1,
                        Remarks = "test"
                    }
                }
            });

            _productInventoryHandler.Verify(x => x.HandleAsync(It.IsAny<List<ReqUpdateProductInventory>>()), Times.Once());
            _returnShipmentOrderRepository.Verify(x => x.FindByOptionsAsync(It.IsAny<string?>(), It.IsAny<string?>()), Times.Once());
            _shipmentOrderRepository.Verify(x => x.FindByOptionsAsync(It.IsAny<string?>(), It.IsAny<int?>(),
               It.IsAny<int?>(),
               It.IsAny<string?>(),
               It.IsAny<SortType?>()), Times.Once());
            _returnShipmentOrderRepository.Verify(x => x.UpdateAsync(It.IsAny<IEnumerable<ReturnShipmentOrderDto>>()), Times.Once());
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
