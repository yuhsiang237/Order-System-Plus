using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Xunit;
using FluentAssertions;

using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Models.DataAccessor;
using OrderSystemPlus.Utils.OrderNumberTool;

namespace OrderSystemPlusTest.DataAccessor
{
    public class ShipmentOrderDataAccessorTest
    {
        private IShipmentOrderRepository _repository;
        private DateTime _now;
        private readonly string _orderNumber;
        public ShipmentOrderDataAccessorTest()
        {
            _now = DateTime.Now;
            _orderNumber = OrderNumberTool.GenerateNumber(OrderNumberTool.Type.Shipment);
            _repository = new ShipmentOrderRepository();
        }

        [Fact]
        public async Task RunAsync()
        {
            await _repository.InsertAsync(new List<ShipmentOrderDto> { GetInsertModel() });
            var insertResult = await _repository.FindByOptionsAsync(GetInsertModel().OrderNumber);
            insertResult.Count.Should().Be(1);
            insertResult.First().OrderNumber.Should().Be(GetInsertModel().OrderNumber);
            insertResult.First().OrderType.Should().Be(GetInsertModel().OrderType);
            insertResult.First().TotalAmount.Should().Be(GetInsertModel().TotalAmount);
            insertResult.First().RecipientName.Should().Be(GetInsertModel().RecipientName);
            insertResult.First().OperatorUserId.Should().Be(GetInsertModel().OperatorUserId);
            insertResult.First().Status.Should().Be(GetInsertModel().Status);
            insertResult.First().FinishDate.Should().Be(GetInsertModel().FinishDate);
            insertResult.First().DeliveryDate.Should().Be(GetInsertModel().DeliveryDate);
            insertResult.First().Address.Should().Be(GetInsertModel().Address);
            insertResult.First().Remark.Should().Be(GetInsertModel().Remark);

            var insertDetailResult = insertResult.First().Details.First();
            insertDetailResult.OrderNumber.Should().Be(GetInsertModel().Details.First().OrderNumber);
            insertDetailResult.ProductId.Should().Be(GetInsertModel().Details.First().ProductId);
            insertDetailResult.ProductNumber.Should().Be(GetInsertModel().Details.First().ProductNumber);
            insertDetailResult.ProductName.Should().Be(GetInsertModel().Details.First().ProductName);
            insertDetailResult.ProductPrice.Should().Be(GetInsertModel().Details.First().ProductPrice);
            insertDetailResult.ProductQuantity.Should().Be(GetInsertModel().Details.First().ProductQuantity);
            insertDetailResult.Remarks.Should().Be(GetInsertModel().Details.First().Remarks);

            await _repository.UpdateAsync(new List<ShipmentOrderDto> { GetUpdateModel() });
            var updateResult = await _repository.FindByOptionsAsync(GetUpdateModel().OrderNumber);
            updateResult.First().OrderNumber.Should().Be(GetUpdateModel().OrderNumber);
            updateResult.First().OrderType.Should().Be(GetUpdateModel().OrderType);
            updateResult.First().TotalAmount.Should().Be(GetUpdateModel().TotalAmount);
            updateResult.First().RecipientName.Should().Be(GetUpdateModel().RecipientName);
            updateResult.First().OperatorUserId.Should().Be(GetUpdateModel().OperatorUserId);
            updateResult.First().Status.Should().Be(GetUpdateModel().Status);
            updateResult.First().FinishDate.Should().Be(GetUpdateModel().FinishDate);
            updateResult.First().DeliveryDate.Should().Be(GetUpdateModel().DeliveryDate);
            updateResult.First().Address.Should().Be(GetUpdateModel().Address);
            updateResult.First().Remark.Should().Be(GetUpdateModel().Remark);

            var updateDetailResult = updateResult.First().Details.First();
            updateDetailResult.OrderNumber.Should().Be(GetUpdateModel().Details.First().OrderNumber);
            updateDetailResult.ProductId.Should().Be(GetUpdateModel().Details.First().ProductId);
            updateDetailResult.ProductNumber.Should().Be(GetUpdateModel().Details.First().ProductNumber);
            updateDetailResult.ProductName.Should().Be(GetUpdateModel().Details.First().ProductName);
            updateDetailResult.ProductPrice.Should().Be(GetUpdateModel().Details.First().ProductPrice);
            updateDetailResult.ProductQuantity.Should().Be(GetUpdateModel().Details.First().ProductQuantity);
            updateDetailResult.Remarks.Should().Be(GetUpdateModel().Details.First().Remarks);

            await _repository.DeleteAsync(new List<ShipmentOrderDto> { GetDeleteModel() });
            var deleteResult = await _repository.FindByOptionsAsync(_orderNumber);
            deleteResult.Count.Should().Be(0);
        }

        public ShipmentOrderDto GetInsertModel() =>
                new ShipmentOrderDto
                {
                    OrderNumber = _orderNumber,
                    OrderType = 1,
                    TotalAmount = 100000,
                    RecipientName = "接收人",
                    OperatorUserId = 5,
                    Status = 1,
                    FinishDate = new DateTime(2023, 06, 16),
                    DeliveryDate = new DateTime(2023, 07, 16),
                    Address = "OO市OO區OO路",
                    Remark = "備註",
                    Details = new List<ShipmentOrderDetailDto>
                    {
                        new ShipmentOrderDetailDto
                        {
                            OrderNumber = _orderNumber,
                            ProductId = 3,
                            ProductNumber = "TEST",
                            ProductName = "TESTNAME",
                            ProductPrice= 300,
                            ProductQuantity = 53,
                            Remarks = "備註",
                            CreatedOn = new DateTime(2023, 06, 16),
                            UpdatedOn = new DateTime(2023, 06, 16),
                            IsValid = true,
                        }
                    },
                    IsValid = true,
                    CreatedOn = _now,
                    UpdatedOn = _now,
                };

        public ShipmentOrderDto GetUpdateModel() =>
                new ShipmentOrderDto
                {
                    OrderNumber = _orderNumber,
                    OrderType = 1,
                    TotalAmount = 100000,
                    RecipientName = "接收人",
                    OperatorUserId = 5,
                    Status = 1,
                    FinishDate = new DateTime(2023, 06, 16),
                    DeliveryDate = new DateTime(2023, 07, 16),
                    Address = "OO市OO區OO路",
                    Remark = "備註",
                    Details = new List<ShipmentOrderDetailDto>
                    {
                        new ShipmentOrderDetailDto
                        {
                            OrderNumber = _orderNumber,
                            ProductId = 65,
                            ProductNumber = "TEST",
                            ProductName = "TESTNAME",
                            ProductPrice= 30,
                            ProductQuantity = 5,
                            Remarks = "備註",
                            CreatedOn = new DateTime(2023, 06, 16),
                            UpdatedOn = new DateTime(2023, 06, 16),
                            IsValid = true,
                        }
                    },
                    IsValid = true,
                    UpdatedOn = _now,
                };

        public ShipmentOrderDto GetDeleteModel() =>
                 new ShipmentOrderDto
                 {
                     OrderNumber = _orderNumber,
                     UpdatedOn = _now,
                 };
    }
}
