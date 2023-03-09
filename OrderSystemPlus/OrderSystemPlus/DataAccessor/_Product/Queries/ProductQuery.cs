using System.Data.SqlClient;

using Dapper;

using OrderSystemPlus.Models;
using OrderSystemPlus.Models.DataAccessor.Queries;

namespace OrderSystemPlus.DataAccessor.Queries
{
    public class ProductQuery : IProductQuery
    {
        public async Task<List<ProductQueryModel>> FindByOptionsAsync(int? id = null, string? name = null, string? number = null)
        {
            string sql = @"
                           SELECT
                              [Id]
                              ,[Number]
                              ,[Name]
                              ,[Price]
                              ,[Description]
                              ,[CurrentUnit]
                              ,[CreatedOn]
                              ,[UpdatedOn]
                              ,[IsValid]
                           FROM [dbo].[Product]";

            var conditions = new List<string>
            {
                "[IsValid] = @IsValid",
            };
            if (id.HasValue)
                conditions.Add("[Id] = @Id");
            if (!string.IsNullOrEmpty(name))
                conditions.Add("[Name] LIKE @Name");
            if (!string.IsNullOrEmpty(number))
                conditions.Add("[Number] = @Number");

            if (conditions.Any())
                sql = string.Concat(sql, $" WHERE {string.Join(" AND ", conditions)}");

            var result = default(List<ProductQueryModel>);
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                result = (await conn.QueryAsync<ProductQueryModel>(sql, new
                {
                    IsValid = true,
                    Name = name,
                    Id = id,
                    Number = number,
                })).ToList();
            }

            return result;
        }
    }
}
