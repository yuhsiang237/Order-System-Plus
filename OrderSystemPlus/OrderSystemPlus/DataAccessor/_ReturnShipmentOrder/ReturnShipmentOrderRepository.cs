using System.Data;
using System.Data.SqlClient;
using System.Transactions;

using Dapper;
using OrderSystemPlus.Enums;
using OrderSystemPlus.Models;
using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlus.DataAccessor
{
    public class ReturnShipmentOrderRepository : IReturnShipmentOrderRepository
    {
        public async Task<(int TotalCount, List<ReturnShipmentOrderDto> Data)> FindByOptionsAsync(
            string? returnShipmentOrderNumber = null, 
            string? shipmentOrderNumber = null,
            int? pageIndex = null,
            int? pageSize = null,
            string? sortField = null,
            SortType? sortType = null)
        {
            var conditions = new List<string>
            {
                "[IsValid] = @IsValid",
            };

            if (!string.IsNullOrEmpty(returnShipmentOrderNumber))
                conditions.Add("[ReturnShipmentOrderNumber] = @ReturnShipmentOrderNumber");
            if (!string.IsNullOrEmpty(shipmentOrderNumber))
                conditions.Add("[ShipmentOrderNumber] = @ShipmentOrderNumber");

            var totalCountSql = GetTotalCountStatement(conditions);
            var sorts = new List<string>();
            if (!string.IsNullOrEmpty(sortField))
                sorts.Add($"[{sortField.ToUpper()}] {Enum.GetName(typeof(SortType), sortType)}");
            else
                sorts.Add($"[CreatedOn] DESC");

            var dataSql = GetDataStatement(conditions, sorts, pageIndex, pageSize);

            var rspData = default(List<ReturnShipmentOrderDto>);
            var rspTotalCount = default(int);

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                var reqParams = new
                {
                    IsValid = true,
                    ReturnShipmentOrderNumber = returnShipmentOrderNumber,
                    ShipmentOrderNumber = shipmentOrderNumber,
                };
                rspTotalCount = (await conn.QueryAsync<int>(totalCountSql, reqParams)).First();
                rspData = (await conn.QueryAsync<ReturnShipmentOrderDto>(dataSql, reqParams)).ToList();
            }

            rspData.ForEach(x =>
            {
                x.Details = GetDetails(x.ReturnShipmentOrderNumber);
            });

            return (rspTotalCount, rspData);
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
                              COUNT([ReturnShipmentOrderNumber])
                            FROM
                           [dbo].[ReturnShipmentOrder]";

            if (conditions.Any())
                sql = string.Concat(sql, $" WHERE {string.Join(" AND ", conditions)}");

            return sql;
        }
    }
}
