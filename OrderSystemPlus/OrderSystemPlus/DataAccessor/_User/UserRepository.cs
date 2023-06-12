using System.Data.SqlClient;

using Dapper;

using OrderSystemPlus.Models;
using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlus.DataAccessor
{
    public class UserRepository : IUserRepository
    {
        public async Task<List<UserDto>> FindByOptionsAsync(int? id = null, string? email = null, string? account = null)
        {
            string sql = @"
                           SELECT
                              [Id],
                              [Name],
                              [Salt],
                              [Email],
                              [Account],
                              [Password],
                              [RoleId]
                           FROM [dbo].[User]";

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

            if (conditions.Any())
                sql = string.Concat(sql, $" WHERE {string.Join(" AND ", conditions)}");

            var result = default(List<UserDto>);
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                result = (await conn.QueryAsync<UserDto>(sql, new
                {
                    IsValid = true,
                    Email = email,
                    Account = account,
                    Id = id,
                })).ToList();
            }

            return result;
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
                return conn.ExecuteScalar<int>(sql, model);
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
    }
}
