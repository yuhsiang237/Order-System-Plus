﻿using System.Data;
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
                                ,[OrderType]
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

            return result;
        }
        public async Task UpdateAsync(IEnumerable<ShipmentOrderDto> model)
        {
            var sql = @"
                UPDATE [dbo].[ShipmentOrder]
                SET
                   [OrderType] = @OrderType,
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
                    ,[OrderType]
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
                    ,@OrderType
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
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                await conn.ExecuteAsync(sql, model);
            }
        }
    }
}
