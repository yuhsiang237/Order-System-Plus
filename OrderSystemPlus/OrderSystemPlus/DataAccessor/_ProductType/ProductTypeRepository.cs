using System.Data;
using System.Data.SqlClient;
using System.Transactions;

using Dapper;

using OrderSystemPlus.Models;
using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlus.DataAccessor
{
    public class ProductTypeRepository : IProductTypeRepository
    {
        public async Task<List<ProductTypeDto>> FindByOptionsAsync(int? id = null, string? name = null)
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
                conditions.Add("[Name] = @Name");

            if (conditions.Any())
                sql = string.Concat(sql, $" WHERE {string.Join(" AND ", conditions)}");

            var result = default(List<ProductTypeDto>);
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                result = (await conn.QueryAsync<ProductTypeDto>(sql, new
                {
                    IsValid = true,
                    Name = name,
                    Id = id,
                })).ToList();
            }

            return result;
        }
        public async Task UpdateAsync(IEnumerable<ProductTypeDto> model)
        {
            var sql = @"
                UPDATE [dbo].[ProductType]
                SET
                  [Name] = @Name,
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
        public async Task<List<int>> InsertAsync(IEnumerable<ProductTypeDto> model)
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
        private int InsertAsync(ProductTypeDto command, IDbConnection cn)
        {
            var sql = @"
                INSERT INTO [dbo].[ProductType]
                (
                  [Name]
                  ,[Description]
                  ,[CreatedOn]
                  ,[UpdatedOn]
                  ,[IsValid]
                ) VALUES
                (
                  @Name,
                  @Description,
                  @CreatedOn,
                  @UpdatedOn,
                  @IsValid
                ) 
            SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];
                ";
            return cn.ExecuteScalar<int>(sql, command);
        }
        public async Task DeleteAsync(IEnumerable<ProductTypeDto> model)
        {
            var sql = @"
                UPDATE [dbo].[ProductType]
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
