using System.Data;
using System.Data.SqlClient;
using System.Transactions;

using Dapper;

using OrderSystemPlus.Models;
using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlus.DataAccessor
{
    public class ShipmentOrderRepository : IShipmentOrderRepository
    {
        public async Task<List<ShipmentOrderDto>> FindByOptionsAsync(string? orderNumber = null)
        {
            string sql = @"
                           SELECT
                                [OrderNumber]
                                ,[TotalAmount]
                                ,[RecipientName]
                                ,[OperatorUserId]
                                ,[Status]
                                ,[FinishDate]
                                ,[DeliveryDate]
                                ,[Address]
                                ,[Remark]
                                ,[CreatedOn]
                                ,[UpdatedOn]
                                ,[IsValid]
                           FROM [dbo].[ShipmentOrder]";

            var conditions = new List<string>
            {
                "[IsValid] = @IsValid",
            };

            if (!string.IsNullOrEmpty(orderNumber))
                conditions.Add("[OrderNumber] = @OrderNumber");

            if (conditions.Any())
                sql = string.Concat(sql, $" WHERE {string.Join(" AND ", conditions)}");

            var result = default(List<ShipmentOrderDto>);
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                result = (await conn.QueryAsync<ShipmentOrderDto>(sql, new
                {
                    IsValid = true,
                    OrderNumber = orderNumber,
                })).ToList();
            }

            result.ForEach(x =>
            {
                x.Details = GetDetails(x.OrderNumber);
            });

            return result;
        }

        public List<ShipmentOrderDetailDto> GetDetails(string orderNumber)
        {
            string sql = @"
                           SELECT
                                [OrderNumber]
                                ,[ProductId]
                                ,[ProductNumber]
                                ,[ProductName]
                                ,[ProductPrice]
                                ,[ProductQuantity]
                                ,[Remarks]
                                ,[CreatedOn]
                                ,[UpdatedOn]
                                ,[IsValid]
                           FROM [dbo].[ShipmentOrderDetail]
                           WHERE 
                                [OrderNumber] = @OrderNumber
                                AND [IsValid] = 1";
            var result = default(List<ShipmentOrderDetailDto>);
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                result = (conn.Query<ShipmentOrderDetailDto>(sql, new
                {
                    OrderNumber = orderNumber,
                })).ToList();
            }
            return result;
        }

        public async Task UpdateAsync(IEnumerable<ShipmentOrderDto> model)
        {
            var sql = @"
                UPDATE [dbo].[ShipmentOrder]
                SET
                   [TotalAmount] = @TotalAmount,
                   [RecipientName] = @RecipientName,
                   [OperatorUserId] = @OperatorUserId,
                   [Status] = @Status,
                   [FinishDate] = @FinishDate,
                   [DeliveryDate] = @DeliveryDate,
                   [Address]= @Address,
                   [Remark] = @Remark,
                   [UpdatedOn] = @UpdatedOn
                WHERE
                   [OrderNumber] = @OrderNumber
                   AND IsValid = 1
                ";
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                await conn.ExecuteAsync(sql, model);
                foreach (var item in model)
                {
                    UpdateInsertDetailAsync(item.Details, conn);
                }
            }
        }
        public async Task<List<string>> InsertAsync(IEnumerable<ShipmentOrderDto> model)
        {
            var result = new List<string>();
            using (var ts = new TransactionScope())
            {
                using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
                {
                    foreach (var item in model)
                    {
                        InsertAsync(item, conn);
                        UpdateInsertDetailAsync(item.Details, conn);
                    }
                }
                ts.Complete();
            }

            return await Task.FromResult(result);
        }
        private void InsertAsync(ShipmentOrderDto command, IDbConnection cn)
        {
            var sql = @"
                INSERT INTO [dbo].[ShipmentOrder]
                (
                    [OrderNumber]
                    ,[TotalAmount]
                    ,[RecipientName]
                    ,[OperatorUserId]
                    ,[Status]
                    ,[FinishDate]
                    ,[DeliveryDate]
                    ,[Address]
                    ,[Remark]
                    ,[CreatedOn]
                    ,[UpdatedOn]
                    ,[IsValid]
                ) VALUES
                (
                    @OrderNumber
                    ,@TotalAmount
                    ,@RecipientName
                    ,@OperatorUserId
                    ,@Status
                    ,@FinishDate
                    ,@DeliveryDate
                    ,@Address
                    ,@Remark
                    ,@CreatedOn
                    ,@UpdatedOn
                    ,@IsValid
                );
                ";
            var count = cn.Execute(sql, command);
            if (count != 1) { throw new Exception("InsertAsync"); }
        }

        private void UpdateInsertDetailAsync(List<ShipmentOrderDetailDto> command, IDbConnection cn)
        {
            var deletesql = @"
                UPDATE [dbo].[ShipmentOrderDetail]
                SET
                    [IsValid] = 0,
                    [UpdatedOn] = @UpdatedOn
                WHERE
                    [OrderNumber] = @OrderNumber";

            var insertsql = @"
                INSERT INTO [dbo].[ShipmentOrderDetail]
                (
                    [OrderNumber]
                    ,[ProductId]
                    ,[ProductNumber]
                    ,[ProductName]
                    ,[ProductPrice]
                    ,[ProductQuantity]
                    ,[Remarks]
                    ,[CreatedOn]
                    ,[UpdatedOn]
                    ,[IsValid]
                ) VALUES
                (
                    @OrderNumber
                    ,@ProductId
                    ,@ProductNumber
                    ,@ProductName
                    ,@ProductPrice
                    ,@ProductQuantity
                    ,@Remarks
                    ,@CreatedOn
                    ,@UpdatedOn
                    ,@IsValid
                );
                ";
            cn.Execute(deletesql, command);
            var count = cn.Execute(insertsql, command);
            if (count != command.Count) { throw new Exception("InsertDetailAsync"); }
        }

        public async Task DeleteAsync(IEnumerable<ShipmentOrderDto> model)
        {
            var sql = @"
                UPDATE [dbo].[ShipmentOrder]
                SET
                    [IsValid] = 0,
                    [UpdatedOn] = @UpdatedOn
                WHERE
                    [OrderNumber] = @OrderNumber
                ";

            var sqlDetail = @"
                UPDATE [dbo].[ShipmentOrderDetail]
                SET
                    [IsValid] = 0,
                    [UpdatedOn] = @UpdatedOn
                WHERE
                    [OrderNumber] = @OrderNumber
                ";

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                await conn.ExecuteAsync(sql, model);
                await conn.ExecuteAsync(sqlDetail, model);
            }
        }
    }
}
