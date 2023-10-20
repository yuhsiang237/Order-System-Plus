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
    public class ReturnShipmentOrderDataAccessorTest
    {
        private IReturnShipmentOrderRepository _repository;
        private DateTime _now;
        private readonly string _returnShipmentOrderNumber;
        private readonly string _shipmentOrderNumber;
        private int _updateReturnShipmentOrderDetailId;
        public ReturnShipmentOrderDataAccessorTest()
        {
            _now = DateTime.Now;
            _returnShipmentOrderNumber = OrderNumberTool.GenerateNumber(OrderNumberTool.Type.ReturnShipment);
            _shipmentOrderNumber = OrderNumberTool.GenerateNumber(OrderNumberTool.Type.Shipment);
            _repository = new ReturnShipmentOrderRepository();
        }

        [Fact]
        public async Task RunAsync()
        {
            await _repository.InsertAsync(new List<ReturnShipmentOrderDto> { GetInsertModel() });
            var insertResult = await _repository.FindByOptionsAsync(GetInsertModel().ReturnShipmentOrderNumber);
            insertResult.Data.Count.Should().Be(1);
            insertResult.Data.First().ReturnShipmentOrderNumber.Should().Be(GetInsertModel().ReturnShipmentOrderNumber);
            insertResult.Data.First().ShipmentOrderNumber.Should().Be(GetInsertModel().ShipmentOrderNumber);
            insertResult.Data.First().TotalReturnAmount.Should().Be(GetInsertModel().TotalReturnAmount);
            insertResult.Data.First().ReturnDate.Should().Be(GetInsertModel().ReturnDate);
            insertResult.Data.First().Remark.Should().Be(GetInsertModel().Remark);
            insertResult.Data.First().OperatorUserId.Should().Be(GetInsertModel().OperatorUserId);

            var insertDetailResult = insertResult.Data.First().Details.First();
            insertDetailResult.ReturnShipmentOrderNumber.Should().Be(GetInsertModel().Details.First().ReturnShipmentOrderNumber);
            insertDetailResult.ShipmentOrderDetailId.Should().Be(GetInsertModel().Details.First().ShipmentOrderDetailId);
            insertDetailResult.ReturnProductQuantity.Should().Be(GetInsertModel().Details.First().ReturnProductQuantity);
            insertDetailResult.Remarks.Should().Be(GetInsertModel().Details.First().Remarks);
            _updateReturnShipmentOrderDetailId = insertDetailResult.Id;

            await _repository.UpdateAsync(new List<ReturnShipmentOrderDto> { GetUpdateModel() });
            var updateResult = await _repository.FindByOptionsAsync(GetUpdateModel().ReturnShipmentOrderNumber);
            updateResult.Data.First().ReturnShipmentOrderNumber.Should().Be(GetUpdateModel().ReturnShipmentOrderNumber);
            updateResult.Data.First().ShipmentOrderNumber.Should().Be(GetUpdateModel().ShipmentOrderNumber);
            updateResult.Data.First().TotalReturnAmount.Should().Be(GetUpdateModel().TotalReturnAmount);
            updateResult.Data.First().ReturnDate.Should().Be(GetUpdateModel().ReturnDate);
            updateResult.Data.First().Remark.Should().Be(GetUpdateModel().Remark);
            updateResult.Data.First().OperatorUserId.Should().Be(GetUpdateModel().OperatorUserId);

            var updateDetailResult = updateResult.Data.First().Details.First();
            updateDetailResult.Remarks.Should().Be(GetUpdateModel().Details.First().Remarks);
            updateDetailResult.ReturnProductQuantity.Should().Be(GetUpdateModel().Details.First().ReturnProductQuantity);

            await _repository.DeleteAsync(new List<ReturnShipmentOrderDto> { GetDeleteModel() });
            var deleteResult = await _repository.FindByOptionsAsync(_returnShipmentOrderNumber);
            deleteResult.Data.Count.Should().Be(0);
        }

        public ReturnShipmentOrderDto GetInsertModel() =>
                new ReturnShipmentOrderDto
                {
                    ReturnShipmentOrderNumber = _returnShipmentOrderNumber,
                    ShipmentOrderNumber = _shipmentOrderNumber,
                    TotalReturnAmount = 10000,
                    OperatorUserId = 999,
                    ReturnDate = new DateTime(2023, 06, 11),
                    Remark = "測試",
                    IsValid = true,
                    CreatedOn = _now,
                    UpdatedOn = _now,
                    Details = new List<ReturnShipmentOrderDetailDto>
                    {
                        new ReturnShipmentOrderDetailDto
                        {
                            ReturnShipmentOrderNumber = _returnShipmentOrderNumber,
                            ShipmentOrderDetailId = 99999999,
                            ReturnProductQuantity = 1,
                            Remarks = "test",
                            CreatedOn = new DateTime(2023, 06, 11),
                            UpdatedOn = new DateTime(2023, 06, 11),
                            IsValid = true,
                        }
                    },
                };

        public ReturnShipmentOrderDto GetUpdateModel() =>
                new ReturnShipmentOrderDto
                {
                    ReturnShipmentOrderNumber = _returnShipmentOrderNumber,
                    ShipmentOrderNumber = _shipmentOrderNumber,
                    TotalReturnAmount = 10000,
                    OperatorUserId = 7777,
                    ReturnDate = new DateTime(2023, 06, 29),
                    Remark = "測試更新",
                    IsValid = true,
                    UpdatedOn = _now,
                    Details = new List<ReturnShipmentOrderDetailDto>
                    {
                        new ReturnShipmentOrderDetailDto
                        {
                            Id = _updateReturnShipmentOrderDetailId,
                            Remarks = "備註更新",
                            ShipmentOrderDetailId = 99999999,
                            ReturnProductQuantity = 30,
                            CreatedOn = new DateTime(2023, 06, 29),
                            UpdatedOn = new DateTime(2023, 06, 29),
                            IsValid = true,
                        }
                    },
                };

        public ReturnShipmentOrderDto GetDeleteModel() =>
                 new ReturnShipmentOrderDto
                 {
                     OperatorUserId = 7,
                     ReturnShipmentOrderNumber = _returnShipmentOrderNumber,
                     UpdatedOn = _now,
                 };
    }
}
