using System.Data;
using System.Data.SqlClient;
using System.Transactions;

using Dapper;

using OrderSystemPlus.Models;
using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlus.DataAccessor
{
    public class ProductInventoryRepository : IProductInventoryRepository
    {
        public async Task<List<ProductInventoryDto>> FindByOptionsAsync(int? productId = null, int? Id = null)
        {

            string sql = @"
                           SELECT
                                [Id]
                                ,[ProductId]
                                ,[AdjustQuantity]
                                ,[PrevTotalQuantity]
                                ,[TotalQuantity]
                                ,[AdjustProductInventoryType]
                                ,[Remark]
                                ,[CreatedOn]
                                ,[UpdatedOn]
                                ,[IsValid]
                           FROM [dbo].[ProductInventory]";

            var conditions = new List<string>
            {
                "[IsValid] = @IsValid",
            };

            if (productId.HasValue)
                conditions.Add("[ProductId] = @ProductId");
            if (Id.HasValue)
                conditions.Add("[Id] = @Id");

            if (conditions.Any())
                sql = string.Concat(sql, $" WHERE {string.Join(" AND ", conditions)}");

            var result = default(List<ProductInventoryDto>);
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                result = (await conn.QueryAsync<ProductInventoryDto>(sql, new
                {
                    IsValid = true,
                    ProductId = productId,
                    Id = Id,
                })).ToList();
            }

            return result;
        }
        public async Task<List<int>> InsertAsync(IEnumerable<ProductInventoryDto> model)
        {
            var result = new List<int>();
            using (var ts = new TransactionScope())
            {
                using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
                {
                    result = model
                        .Select(s => InsertAsync(s, conn))
                        .ToList();

                    if (!result.Any(a => a == 0))
                        ts.Complete();
                }
            }

            return await Task.FromResult(result);
        }

        private int InsertAsync(ProductInventoryDto command, IDbConnection cn)
        {
            var sql = @"
                INSERT INTO [dbo].[ProductInventory]
                (
                    [ProductId]
                    ,[AdjustQuantity]
                    ,[PrevTotalQuantity]
                    ,[TotalQuantity]
                    ,[AdjustProductInventoryType]
                    ,[Remark]
                    ,[CreatedOn]
                    ,[UpdatedOn]
                    ,[IsValid]
                ) VALUES
                (
                    @ProductId,
                    @AdjustQuantity,
                    @PrevTotalQuantity,
                    @TotalQuantity,
                    @AdjustProductInventoryType,
                    @Remark,
                    @CreatedOn,
                    @UpdatedOn,
                    @IsValid
                )
                SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];
                ";
            return cn.ExecuteScalar<int>(sql, command);
        }

        public async Task DeleteAsync(IEnumerable<ProductInventoryDto> model)
        {
            var sql = @"
                UPDATE [dbo].[ProductInventory]
                SET 
                    [IsValid] = 0,
                    [UpdatedOn] = @UpdatedOn
                WHERE
                    [Id] = @Id
                ";
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                await conn.ExecuteAsync(sql, model);
            }
        }
    }
}
