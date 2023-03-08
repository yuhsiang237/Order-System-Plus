using System.Data.SqlClient;

using Dapper;

using OrderSystemPlus.Models;
using OrderSystemPlus.Models.DataAccessor.Queries;

namespace OrderSystemPlus.DataAccessor.Queries
{
    public class ProductTypeQuery : IProductTypeQuery
    {
        public async Task<List<ProductTypeQueryModel>> FindByOptionsAsync(int? id = null, string? name = null)
        {
            string sql = @"
                           SELECT
                              [Id],
                              [Name],
                              [Description],
                              [CreatedOn],
                              [UpdatedOn],
                              [IsValid]
                           FROM [dbo].[ProductType]";

            var conditions = new List<string>
            {
                "[IsValid] = @IsValid",
            };
            if (id.HasValue)
                conditions.Add("[Id] = @Id");
            if (!string.IsNullOrEmpty(name))
                conditions.Add("[Name] LIKE @Name");

            if (conditions.Any())
                sql = string.Concat(sql, $" WHERE {string.Join(" AND ", conditions)}");

            var result = default(List<ProductTypeQueryModel>);
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                result = (await conn.QueryAsync<ProductTypeQueryModel>(sql,new {
                    IsValid = true,
                    Name = "%" + name + "%",
                    Id = id,
                })).ToList();
            }

            return result;
        }
    }
}
