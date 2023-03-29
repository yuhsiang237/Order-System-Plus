using System.Data.SqlClient;

using Dapper;

using OrderSystemPlus.Models;
using OrderSystemPlus.Models.DataAccessor.Queries;

namespace OrderSystemPlus.DataAccessor.Queries
{
    public class ProductProductTypeRelationshipQuery : IProductProductTypeRelationshipQuery
    {
        public async Task<List<ProductProductTypeRelationshipQueryModel>> FindByOptionsAsync(List<int>? productIds = null, List<int>? productTypeIds = null)
        {
            string sql = @"
                           SELECT
                              [Id]
                              ,[ProductId]
                              ,[ProductTypeId]
                           FROM [dbo].[ProductProductTypeRelationship]";

            var conditions = new List<string>
            {
            };
            if (productIds != null)
                conditions.Add("[ProductId] IN @ProductIds");
            if (productTypeIds != null)
                conditions.Add("[productTypeId] IN @ProductTypeIds");

            if (conditions.Any())
                sql = string.Concat(sql, $" WHERE {string.Join(" AND ", conditions)}");

            var result = default(List<ProductProductTypeRelationshipQueryModel>);
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                result = (await conn.QueryAsync<ProductProductTypeRelationshipQueryModel>(sql, new
                {
                    ProductIds = productIds,
                    productTypeIds = productTypeIds,
                })).ToList();
            }

            return result;
        }
    }
}
