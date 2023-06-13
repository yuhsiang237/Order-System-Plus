using System.Data;
using System.Data.SqlClient;
using System.Transactions;

using Dapper;

using OrderSystemPlus.Models;
using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlus.DataAccessor
{
    public class ProductRepository : IProductRepository
    {
        public async Task<List<ProductDto>> FindByOptionsAsync(int? id = null, string? name = null, string? number = null)
        {
            string sql = @"
                           SELECT
                              [Id]
                              ,[Number]
                              ,[Name]
                              ,[Price]
                              ,[Description]
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
                conditions.Add("[Name] = @Name");
            if (!string.IsNullOrEmpty(number))
                conditions.Add("[Number] = @Number");

            if (conditions.Any())
                sql = string.Concat(sql, $" WHERE {string.Join(" AND ", conditions)}");

            var result = default(List<ProductDto>);
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                result = (await conn.QueryAsync<ProductDto>(sql, new
                {
                    IsValid = true,
                    Name = name,
                    Id = id,
                    Number = number,
                })).ToList();
            }

            return result;
        }
        public async Task UpdateAsync(IEnumerable<ProductDto> model)
        {
            var sql = @"
                UPDATE [dbo].[Product]
                SET
                  [Name] = @Name,
                  [Price] = @Price,
                  [Number] = @Number,
                  [Description] = @Description,
                  [UpdatedOn] = @UpdatedOn
                WHERE
                    [Id] = @Id
                ";
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                await conn.ExecuteAsync(sql, model);
            }
        }
        public async Task<List<int>> InsertAsync(IEnumerable<ProductDto> model)
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
        private int InsertAsync(ProductDto command, IDbConnection cn)
        {
            var sql = @"
                INSERT INTO [dbo].[Product]
                (
                  [Number]
                  ,[Name]
                  ,[Price]
                  ,[Description]
                  ,[CreatedOn]
                  ,[UpdatedOn]
                  ,[IsValid]
                ) VALUES
                (
                  @Number,
                  @Name,
                  @Price,
                  @Description,
                  @CreatedOn,
                  @UpdatedOn,
                  @IsValid
                )
                SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];
                ";
            return cn.ExecuteScalar<int>(sql, command);
        }
        public async Task DeleteAsync(IEnumerable<ProductDto> model)
        {
            var sql = @"
                UPDATE [dbo].[Product]
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
