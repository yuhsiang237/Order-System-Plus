using System.Data;
using System.Data.SqlClient;
using System.Transactions;

using Dapper;

using OrderSystemPlus.Models;
using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlus.DataAccessor
{
    public class ProductTypeRelationshipRepository : IProductTypeRelationshipRepository
    {
        public async Task<List<ProductTypeRelationshipDto>> FindByOptionsAsync(int? productId = null, int? productTypeId = null)
        {

            string sql = @"
                           SELECT
                                [ProductId],
                                [ProductTypeId]
                           FROM [dbo].[ProductTypeRelationship]";

            var conditions = new List<string>
            {
            };

            if (productId.HasValue)
                conditions.Add("[ProductId] = @ProductId");
            if (productTypeId.HasValue)
                conditions.Add("[ProductTypeId] = @ProductTypeId");

            if (conditions.Any())
                sql = string.Concat(sql, $" WHERE {string.Join(" AND ", conditions)}");

            var result = default(List<ProductTypeRelationshipDto>);
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                result = (await conn.QueryAsync<ProductTypeRelationshipDto>(sql, new
                {
                    ProductId = productId,
                    ProductTypeId = productTypeId,
                })).ToList();
            }

            return result;
        }
        public async Task<List<int>> InsertAsync(IEnumerable<ProductTypeRelationshipDto> model)
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

        private int InsertAsync(ProductTypeRelationshipDto command, IDbConnection cn)
        {
            var sql = @"
                INSERT INTO [dbo].[ProductTypeRelationship]
                (
                    [ProductId],
                    [ProductTypeId]
                ) VALUES
                (
                    @ProductId,
                    @ProductTypeId
                )
                SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];
                ";
            return cn.ExecuteScalar<int>(sql, command);
        }

        public async Task DeleteAsync(ProductTypeRelationshipDto model)
        {
            var sql = @"
                UPDATE [dbo].[ProductTypeRelationship]
                SET 
                    [IsValid] = 0,
                    [UpdatedOn] = @UpdatedOn

                ";
            var conditions = new List<string>
            {
            };

            if (model.ProductId.HasValue)
                conditions.Add("[ProductId] = @ProductId");
            if (model.ProductTypeId.HasValue)
                conditions.Add("[ProductTypeId] = @ProductTypeId");

            if (conditions.Any())
                sql = string.Concat(sql, $" WHERE {string.Join(" AND ", conditions)}");

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                await conn.ExecuteAsync(sql, model);
            }
        }
    }
}
