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
        private int _updateShipmentOrderDetailId;
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
            insertResult.Data.Count.Should().Be(1);
            insertResult.Data.First().OrderNumber.Should().Be(GetInsertModel().OrderNumber);
            insertResult.Data.First().TotalAmount.Should().Be(GetInsertModel().TotalAmount);
            insertResult.Data.First().RecipientName.Should().Be(GetInsertModel().RecipientName);
            insertResult.Data.First().OperatorUserId.Should().Be(GetInsertModel().OperatorUserId);
            insertResult.Data.First().Status.Should().Be(GetInsertModel().Status);
            insertResult.Data.First().FinishDate.Should().Be(GetInsertModel().FinishDate);
            insertResult.Data.First().DeliveryDate.Should().Be(GetInsertModel().DeliveryDate);
            insertResult.Data.First().Address.Should().Be(GetInsertModel().Address);
            insertResult.Data.First().Remark.Should().Be(GetInsertModel().Remark);

            var insertDetailResult = insertResult.Data.First().Details.First();
            insertDetailResult.OrderNumber.Should().Be(GetInsertModel().Details.First().OrderNumber);
            insertDetailResult.ProductId.Should().Be(GetInsertModel().Details.First().ProductId);
            insertDetailResult.ProductNumber.Should().Be(GetInsertModel().Details.First().ProductNumber);
            insertDetailResult.ProductName.Should().Be(GetInsertModel().Details.First().ProductName);
            insertDetailResult.ProductPrice.Should().Be(GetInsertModel().Details.First().ProductPrice);
            insertDetailResult.ProductQuantity.Should().Be(GetInsertModel().Details.First().ProductQuantity);
            insertDetailResult.Remarks.Should().Be(GetInsertModel().Details.First().Remarks);

            _updateShipmentOrderDetailId = insertDetailResult.Id;

            await _repository.UpdateAsync(new List<ShipmentOrderDto> { GetUpdateModel() });
            var updateResult = await _repository.FindByOptionsAsync(GetUpdateModel().OrderNumber);
            updateResult.Data.First().OrderNumber.Should().Be(GetUpdateModel().OrderNumber);
            updateResult.Data.First().TotalAmount.Should().Be(GetUpdateModel().TotalAmount);
            updateResult.Data.First().RecipientName.Should().Be(GetUpdateModel().RecipientName);
            updateResult.Data.First().OperatorUserId.Should().Be(GetUpdateModel().OperatorUserId);
            updateResult.Data.First().Status.Should().Be(GetUpdateModel().Status);
            updateResult.Data.First().FinishDate.Should().Be(GetUpdateModel().FinishDate);
            updateResult.Data.First().DeliveryDate.Should().Be(GetUpdateModel().DeliveryDate);
            updateResult.Data.First().Address.Should().Be(GetUpdateModel().Address);
            updateResult.Data.First().Remark.Should().Be(GetUpdateModel().Remark);

            var updateDetailResult = updateResult.Data.First().Details.First();
            updateDetailResult.Remarks.Should().Be(GetUpdateModel().Details.First().Remarks);

            await _repository.DeleteAsync(new List<ShipmentOrderDto> { GetDeleteModel() });
            var deleteResult = await _repository.FindByOptionsAsync(_orderNumber);
            deleteResult.Data.Count.Should().Be(0);
        }

        public ShipmentOrderDto GetInsertModel() =>
                new ShipmentOrderDto
                {
                    OrderNumber = _orderNumber,
                    TotalAmount = 100000,
                    RecipientName = "接收人",
                    OperatorUserId = 5,
                    Status = OrderSystemPlus.Enums.ShipmentOrderStatus.Finish,
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
                    TotalAmount = 100000,
                    RecipientName = "接收人",
                    OperatorUserId = 5,
                    Status = OrderSystemPlus.Enums.ShipmentOrderStatus.Finish,
                    FinishDate = new DateTime(2023, 06, 16),
                    DeliveryDate = new DateTime(2023, 07, 16),
                    Address = "OO市OO區OO路",
                    Remark = "備註",
                    Details = new List<ShipmentOrderDetailDto>
                    {
                        new ShipmentOrderDetailDto
                        {
                            Id = _updateShipmentOrderDetailId,
                            Remarks = "備註更新",
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
