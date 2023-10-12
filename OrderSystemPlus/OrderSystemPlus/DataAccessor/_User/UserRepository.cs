using System.Data.SqlClient;

using Dapper;
using OrderSystemPlus.Enums;
using OrderSystemPlus.Models;
using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlus.DataAccessor
{
    public class UserRepository : IUserRepository
    {
        public async Task<(int TotalCount, List<UserDto> Data)> FindByOptionsAsync(int? id = null, string? email = null, string? account = null, int? pageIndex = null,
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
            if (!string.IsNullOrEmpty(email))
                conditions.Add("[Email] = @Email");
            if (!string.IsNullOrEmpty(account))
                conditions.Add("[Account] = @Account");

            var totalCountSql = GetTotalCountStatement(conditions);

            var sorts = new List<string>();
            if (!string.IsNullOrEmpty(sortField))
                sorts.Add($"[{sortField.ToUpper()}] {Enum.GetName(typeof(SortType), sortType)}");
            else
                sorts.Add($"[CreatedOn] DESC");

            var dataSql = GetDataStatement(conditions, sorts, pageIndex, pageSize);

            var rspData = default(List<UserDto>);
            var rspTotalCount = default(int);

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                var reqParams = new
                {
                    IsValid = true,
                    Email = email,
                    Account = account,
                    Id = id,
                };
                rspTotalCount = (await conn.QueryAsync<int>(totalCountSql, reqParams)).First();
                rspData = (await conn.QueryAsync<UserDto>(dataSql, reqParams)).ToList();
            }
            return (rspTotalCount, rspData);
        }

        public async Task UpdateAsync(IEnumerable<UserDto> model)
        {
            var sql = @"
                UPDATE [dbo].[User]
                SET 
                  [Name] = @Name,
                  [Email] = @Email,
                  [UpdatedOn] = @UpdatedOn
                WHERE
                    [Id] = @Id
                ";
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                await conn.ExecuteAsync(sql, model);
            }
        }
        public async Task<int> InsertAsync(UserDto model)
        {
            var sql = @"
                INSERT INTO [dbo].[User]
                (
                    [Name],
                    [Salt],
                    [Email],
                    [Account],
                    [Password],
                    [RoleId],
                    [CreatedOn],
                    [UpdatedOn],
                    [IsValid]
                ) VALUES
                (
                    @Name,
                    @Salt,
                    @Email,
                    @Account,
                    @Password,
                    @RoleId,
                    @CreatedOn,
                    @UpdatedOn,
                    @IsValid
                )
            SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];
                ";
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                return await conn.ExecuteScalarAsync<int>(sql, model);
            }
        }

        public async Task DeleteAsync(IEnumerable<UserDto> model)
        {
            var sql = @"
                UPDATE [dbo].[User]
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
                              [Salt],
                              [Email],
                              [Account],
                              [Password],
                              [RoleId]
                           FROM [dbo].[User]";

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
                           FROM [dbo].[User]";

            if (conditions.Any())
                sql = string.Concat(sql, $" WHERE {string.Join(" AND ", conditions)}");

            return sql;
        }
    }
}
