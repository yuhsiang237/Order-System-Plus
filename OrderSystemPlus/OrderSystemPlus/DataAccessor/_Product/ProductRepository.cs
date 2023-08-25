using System.Data;
using System.Data.SqlClient;
using System.Transactions;

using Dapper;
using OrderSystemPlus.Enums;
using OrderSystemPlus.Models;
using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlus.DataAccessor
{
    public class ProductRepository : IProductRepository
    {
        public async Task<(int TotalCount, List<ProductDto> Data)> FindByOptionsAsync(
            int? id = null, string? name = null, string? number = null, string? likeName = null, string? likeNumber = null,
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
            if (!string.IsNullOrEmpty(number))
                conditions.Add("[Number] = @Number");
            if (!string.IsNullOrEmpty(likeName))
                conditions.Add("[Name] LIKE @LikeName");
            if (!string.IsNullOrEmpty(likeNumber))
                conditions.Add("[Number] LIKE @LikeNumber");

            var totalCountSql = GetTotalCountStatement(conditions);

            var sorts = new List<string>();
            if (!string.IsNullOrEmpty(sortField))
                sorts.Add($"[{sortField.ToUpper()}] {Enum.GetName(typeof(SortType), sortType)}");
            else
                sorts.Add($"[CreatedOn] DESC");

            var dataSql = GetDataStatement(conditions, sorts, pageIndex, pageSize);

            var rspData = default(List<ProductDto>);
            var rspTotalCount = default(int);

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                var reqParams = new
                {
                    IsValid = true,
                    Name = name,
                    Id = id,
                    Number = number,
                    LikeName = "%" + likeName + "%",
                    LikeNumber = "%" + likeNumber + "%",
                };
                rspTotalCount = (await conn.QueryAsync<int>(totalCountSql, reqParams)).First();
                rspData = (await conn.QueryAsync<ProductDto>(dataSql, reqParams)).ToList();
            }
            return (rspTotalCount, rspData);
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
                              [Id]
                              ,[Number]
                              ,[Name]
                              ,[Price]
                              ,[Description]
                              ,[CreatedOn]
                              ,[UpdatedOn]
                              ,[IsValid]
                           FROM [dbo].[Product]";

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
                           FROM [dbo].[Product]";

            if (conditions.Any())
                sql = string.Concat(sql, $" WHERE {string.Join(" AND ", conditions)}");

            return sql;
        }
    }
}
