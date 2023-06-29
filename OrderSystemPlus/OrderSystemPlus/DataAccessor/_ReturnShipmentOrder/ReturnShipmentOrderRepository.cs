using System.Data;
using System.Data.SqlClient;
using System.Transactions;

using Dapper;

using OrderSystemPlus.Models;
using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlus.DataAccessor
{
    public class ReturnShipmentOrderRepository : IReturnShipmentOrderRepository
    {
        public async Task<List<ReturnShipmentOrderDto>> FindByOptionsAsync(string? returnShipmentOrderNumber = null)
        {
            string sql = @"
                           SELECT
                                [ReturnShipmentOrderNumber]
                                ,[ShipmentOrderNumber]
                                ,[TotalReturnAmount]
                                ,[OperatorUserId]
                                ,[ReturnDate]
                                ,[Remark]
                                ,[IsValid]
                                ,[CreatedOn]
                                ,[UpdatedOn]
                           FROM [dbo].[ReturnShipmentOrder]";

            var conditions = new List<string>
            {
                "[IsValid] = @IsValid",
            };

            if (!string.IsNullOrEmpty(returnShipmentOrderNumber))
                conditions.Add("[ReturnShipmentOrderNumber] = @ReturnShipmentOrderNumber");

            if (conditions.Any())
                sql = string.Concat(sql, $" WHERE {string.Join(" AND ", conditions)}");

            var result = default(List<ReturnShipmentOrderDto>);
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                result = (await conn.QueryAsync<ReturnShipmentOrderDto>(sql, new
                {
                    IsValid = true,
                    ReturnShipmentOrderNumber = returnShipmentOrderNumber,
                })).ToList();
            }

            result.ForEach(x =>
            {
                x.Details = GetDetails(x.ReturnShipmentOrderNumber);
            });

            return result;
        }

        public List<ReturnShipmentOrderDetailDto> GetDetails(string returnShipmentOrderNumber)
        {
            string sql = @"
                           SELECT
                                [Id]
                                ,[ReturnShipmentOrderNumber]
                                ,[ShipmentOrderDetailId]
                                ,[ReturnProductQuantity]
                                ,[Remarks]
                                ,[CreatedOn]
                                ,[UpdatedOn]
                                ,[IsValid]
                           FROM [dbo].[ReturnShipmentOrderDetail]
                           WHERE 
                                [ReturnShipmentOrderNumber] = @ReturnShipmentOrderNumber
                                AND [IsValid] = 1";
            var result = default(List<ReturnShipmentOrderDetailDto>);
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                result = (conn.Query<ReturnShipmentOrderDetailDto>(sql, new
                {
                    ReturnShipmentOrderNumber = returnShipmentOrderNumber,
                })).ToList();
            }
            return result;
        }

        public async Task UpdateAsync(IEnumerable<ReturnShipmentOrderDto> model)
        {
            var sql = @"
                UPDATE [dbo].[ReturnShipmentOrder]
                SET
                   [TotalReturnAmount] = @TotalReturnAmount,
                   [ReturnDate] = @ReturnDate,
                   [OperatorUserId] = @OperatorUserId,
                   [Remark] = @Remark,
                   [UpdatedOn] = @UpdatedOn
                WHERE
                   [ReturnShipmentOrderNumber] = @ReturnShipmentOrderNumber
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
        public async Task<List<string>> InsertAsync(IEnumerable<ReturnShipmentOrderDto> model)
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
        private void InsertAsync(ReturnShipmentOrderDto command, IDbConnection cn)
        {
            var sql = @"
                INSERT INTO [dbo].[ReturnShipmentOrder]
                (
                    [ReturnShipmentOrderNumber]
                    ,[ShipmentOrderNumber]
                    ,[TotalReturnAmount]
                    ,[ReturnDate]
                    ,[Remark]
                    ,[OperatorUserId]
                    ,[IsValid]
                    ,[CreatedOn]
                    ,[UpdatedOn]
                ) VALUES
                (
                    @ReturnShipmentOrderNumber
                    ,@ShipmentOrderNumber
                    ,@TotalReturnAmount
                    ,@ReturnDate
                    ,@Remark
                    ,@OperatorUserId
                    ,@IsValid
                    ,@CreatedOn
                    ,@UpdatedOn
                );
                ";
            var count = cn.Execute(sql, command);
            if (count != 1) { throw new Exception("InsertAsync"); }
        }

        private void UpdateInsertDetailAsync(List<ReturnShipmentOrderDetailDto> command, IDbConnection cn)
        {
            var insertList = command.Where(s => s.Id == 0).ToList();
            var updateList = command.Where(s => s.Id != 0).ToList();

            var insertsql = @"
                INSERT INTO [dbo].[ReturnShipmentOrderDetail]
                (
                    [ReturnShipmentOrderNumber]
                    ,[ShipmentOrderDetailId]
                    ,[ReturnProductQuantity]
                    ,[Remarks]
                    ,[CreatedOn]
                    ,[UpdatedOn]
                    ,[IsValid]
                ) VALUES
                (
                    @ReturnShipmentOrderNumber
                    ,@ShipmentOrderDetailId
                    ,@ReturnProductQuantity
                    ,@Remarks
                    ,@CreatedOn
                    ,@UpdatedOn
                    ,@IsValid
                );
                ";

            var updateSql = @"
                UPDATE [dbo].[ReturnShipmentOrderDetail]
                SET
                    [Remarks] = @Remarks,
                    [ReturnProductQuantity] = @ReturnProductQuantity,
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

        public async Task DeleteAsync(IEnumerable<ReturnShipmentOrderDto> model)
        {
            var sql = @"
                UPDATE [dbo].[ReturnShipmentOrder]
                SET
                    [OperatorUserId] = @OperatorUserId,
                    [IsValid] = 0,
                    [UpdatedOn] = @UpdatedOn
                WHERE
                    [ReturnShipmentOrderNumber] = @ReturnShipmentOrderNumber
                ";

            var sqlDetail = @"
                UPDATE [dbo].[ReturnShipmentOrderDetail]
                SET
                    [IsValid] = 0,
                    [UpdatedOn] = @UpdatedOn
                WHERE
                    [ReturnShipmentOrderNumber] = @ReturnShipmentOrderNumber
                ";

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                await conn.ExecuteAsync(sql, model);
                await conn.ExecuteAsync(sqlDetail, model);
            }
        }
    }
}
