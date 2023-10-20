using System.Data;
using System.Data.SqlClient;
using System.Transactions;

using Dapper;
using OrderSystemPlus.Enums;
using OrderSystemPlus.Models;
using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlus.DataAccessor
{
    public class ShipmentOrderRepository : IShipmentOrderRepository
    {
        public async Task<(int TotalCount, List<ShipmentOrderDto> Data)> FindByOptionsAsync(
            string? orderNumber = null,
            int? pageIndex = null,
            int? pageSize = null,
            string? sortField = null,
            SortType? sortType = null)
        {
            var conditions = new List<string>
            {
                "[IsValid] = @IsValid",
            };
            if (!string.IsNullOrEmpty(orderNumber))
                conditions.Add("[OrderNumber] = @OrderNumber");

            var totalCountSql = GetTotalCountStatement(conditions);
            var sorts = new List<string>();
            if (!string.IsNullOrEmpty(sortField))
                sorts.Add($"[{sortField.ToUpper()}] {Enum.GetName(typeof(SortType), sortType)}");
            else
                sorts.Add($"[CreatedOn] DESC");

            var dataSql = GetDataStatement(conditions, sorts, pageIndex, pageSize);

            var rspData = default(List<ShipmentOrderDto>);
            var rspTotalCount = default(int);

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                var reqParams = new
                {
                    IsValid = true,
                    OrderNumber = orderNumber,
                };
                rspTotalCount = (await conn.QueryAsync<int>(totalCountSql, reqParams)).First();
                rspData = (await conn.QueryAsync<ShipmentOrderDto>(dataSql, reqParams)).ToList();
            }

            rspData.ForEach(x =>
            {
                x.Details = GetDetails(x.OrderNumber);
            });

            return (rspTotalCount, rspData);
        }

        public List<ShipmentOrderDetailDto> GetDetails(string orderNumber)
        {
            string sql = @"
                           SELECT
                                [Id]
                                ,[OrderNumber]
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
            var insertList = command.Where(s => s.Id == 0).ToList();
            var updateList = command.Where(s => s.Id != 0).ToList();

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

            var updateSql = @"
                UPDATE [dbo].[ShipmentOrderDetail]
                SET
                    [Remarks] = @Remarks,
                    [UpdatedOn] = @UpdatedOn
                WHERE
                    [Id] = @Id";

            var insertCount = cn.Execute(insertsql, insertList);
            var updateCount = cn.Execute(updateSql, updateList);
            if (!(insertList.Count == insertCount &&
                updateList.Count == updateCount))
            {
                throw new Exception("UpdateInsertDetailAsync Error");
            }
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
        /// <summary>
        /// GetDataStatement
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="sorts"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private string GetDataStatement(
            List<string> conditions,
            List<string> sorts,
            int? pageIndex,
            int? pageSize)
        {
            var sql = @"SELECT
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

            if (conditions.Any())
                sql = string.Concat(sql, $" WHERE {string.Join(" AND ", conditions)}");
            if (sorts.Any())
                sql = string.Concat(sql, Environment.NewLine, " ORDER BY ", string.Join(" , ", sorts));
            if (pageIndex > 0 || pageSize > 0)
                sql = string.Concat(sql, Environment.NewLine, " OFFSET ", (pageIndex - 1) * pageSize, " ROWS FETCH NEXT ", pageSize, " ROWS ONLY");

            return sql;
        }

        /// <summary>
        /// GetTotalCountStatement
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        private string GetTotalCountStatement(List<string> conditions)
        {
            var sql = @"SELECT
                              COUNT([OrderNumber])
                           FROM [dbo].[ShipmentOrder]";

            if (conditions.Any())
                sql = string.Concat(sql, $" WHERE {string.Join(" AND ", conditions)}");

            return sql;
        }
    }
}
