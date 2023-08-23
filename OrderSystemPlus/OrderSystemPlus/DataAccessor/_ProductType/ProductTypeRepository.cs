using System.Data;
using System.Data.SqlClient;
using System.Transactions;

using Dapper;
using OrderSystemPlus.Enums;
using OrderSystemPlus.Models;
using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlus.DataAccessor
{
    public class ProductTypeRepository : IProductTypeRepository
    {
        public async Task<(int TotalCount, List<ProductTypeDto> Data)> FindByOptionsAsync(
            int? id = null,
            string? name = null,
            string? likeName = null,
            int? pageIndex = null,
            int? pageSize = null,
            string? sortField = null,
            SortType? sortType = null)
        {
            var conditions = new List<string>
            {
                "[IsValid] = @IsValid",
            };

            if (id.HasValue)
                conditions.Add("[Id] = @Id");
            if (!string.IsNullOrEmpty(name))
                conditions.Add("[Name] = @Name");
            if (!string.IsNullOrEmpty(likeName))
                conditions.Add("[Name] LIKE @LikeName");

            var totalCountSql = GetTotalCountStatement(conditions);

            var sorts = new List<string>();
            if (!string.IsNullOrEmpty(sortField))
                sorts.Add($"[{sortField.ToUpper()}] {Enum.GetName(typeof(SortType), sortType)}");
            else
                sorts.Add($"[CreatedOn] DESC");

            var dataSql = GetDataStatement(conditions, sorts, pageIndex, pageSize);

            var rspData = default(List<ProductTypeDto>);
            var rspTotalCount = default(int);

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                rspTotalCount = (await conn.QueryAsync<int>(totalCountSql, new
                {
                    IsValid = true,
                    Name = name,
                    LikeName = "%" + likeName + "%",
                    Id = id,
                })).First();

                rspData = (await conn.QueryAsync<ProductTypeDto>(dataSql, new
                {
                    IsValid = true,
                    Name = name,
                    LikeName = "%" + likeName + "%",
                    Id = id,
                })).ToList();
            }
            return (rspTotalCount, rspData);
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
                              [Id],
                              [Name],
                              [Description],
                              [CreatedOn],
                              [UpdatedOn],
                              [IsValid]
                           FROM [dbo].[ProductType]";

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
                              COUNT([Id])
                           FROM [dbo].[ProductType]";

            if (conditions.Any())
                sql = string.Concat(sql, $" WHERE {string.Join(" AND ", conditions)}");

            return sql;
        }
    }
}
