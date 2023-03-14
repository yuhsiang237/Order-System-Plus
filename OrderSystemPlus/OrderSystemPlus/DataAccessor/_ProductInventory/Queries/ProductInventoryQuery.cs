using System.Data.SqlClient;

using Dapper;

using OrderSystemPlus.Models;
using OrderSystemPlus.Models.DataAccessor.Queries;

namespace OrderSystemPlus.DataAccessor.Queries
{
    public class ProductInventoryQuery : IProductInventoryQuery
    {
        public async Task<List<ProductInventoryQueryModel>> FindByOptionsAsync(int? id = null, int? productId = null)
        {
            string sql = @"
                           SELECT
                              [Id]
                              ,[ProductId]
                              ,[Quantity]
                              ,[ActionType]
                              ,[Description]
                              ,[CreatedOn]
                              ,[UpdatedOn]
                              ,[IsValid]
                           FROM [dbo].[ProductInventory]";

            var conditions = new List<string>
            {
                "[IsValid] = @IsValid",
            };
            if (id.HasValue)
                conditions.Add("[Id] = @Id");
            if (productId.HasValue)
                conditions.Add("[ProductId] = @ProductId");

            if (conditions.Any())
                sql = string.Concat(sql, $" WHERE {string.Join(" AND ", conditions)}");

            var result = default(List<ProductInventoryQueryModel>);
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                result = (await conn.QueryAsync<ProductInventoryQueryModel>(sql, new
                {
                    IsValid = true,
                    ProductId = productId,
                    Id = id,
                })).ToList();
            }

            return result;
        }
    }
}
