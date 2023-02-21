using System.Data.SqlClient;

using Dapper;

using OrderSystemPlus.Models.DataAccessor.Queries;

namespace OrderSystemPlus.DataAccessor.Queries
{
    public class UserQuery : IUserQuery
    {
        private readonly string _connectStr;

        public UserQuery()
        {
            _connectStr = @"Server=.\SQLExpress;Database=OrderSystemDB;Trusted_Connection=True;ConnectRetryCount=0";
        }

        public async Task<List<UserQueryModel>> FindByOptionsAsync(int? id, string? email, string? account)
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

            if (!string.IsNullOrEmpty(email))
                conditions.Add("[Email] = @Email");
            if (!string.IsNullOrEmpty(account))
                conditions.Add("[Account] = @Account");

            if (conditions.Any())
                sql = string.Concat(sql, $" WHERE {string.Join(" AND ", conditions)}");

            var result = default(List<UserQueryModel>);
            using (SqlConnection conn = new SqlConnection(_connectStr))
            {
                result = (await conn.QueryAsync<UserQueryModel>(sql,new {
                    IsValid = true,
                    Email = email,
                    Account = account,
                    Id = id,
                })).ToList();
            }

            return result;
        }
    }
}
