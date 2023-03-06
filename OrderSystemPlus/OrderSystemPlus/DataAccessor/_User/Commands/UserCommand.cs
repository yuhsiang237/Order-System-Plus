﻿using OrderSystemPlus.Models.DataAccessor.Commands;

using Dapper;

using System.Data.SqlClient;
using OrderSystemPlus.Models;

namespace OrderSystemPlus.DataAccessor.Commands
{
    public class UserCommand :
        IInsertCommand<IEnumerable<UserCommandModel>>,
        IRefreshCommand<IEnumerable<UserCommandModel>>,
        IDeleteCommand<IEnumerable<UserCommandModel>>,
        IUpdateCommand<IEnumerable<UserCommandModel>>
    {
        public async Task DeleteAsync(IEnumerable<UserCommandModel> commands)
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
                await conn.ExecuteAsync(sql, commands);
            }
        }

        public async Task InsertAsync(IEnumerable<UserCommandModel> commands)
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
                ";
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                await conn.ExecuteAsync(sql, commands);
            }
        }

        public async Task RefreshAsync(IEnumerable<UserCommandModel> commands)
        {
            var deleteSql = @"
                UPDATE [dbo].[User]
                SET 
                    [IsValid] = 0,
                    [UpdatedOn] = @UpdatedOn
                WHERE
                    [Id] = @Id
                ";
            var insertSql = @"
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
                ";
         
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                conn.Open();
                using (var trans = conn.BeginTransaction()) { 
                    await conn.ExecuteAsync(deleteSql, commands,trans);
                    await conn.ExecuteAsync(insertSql, commands, trans);
                    trans.Commit();
                }
            }
        }

        public async Task UpdateAsync(IEnumerable<UserCommandModel> commands)
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
                await conn.ExecuteAsync(sql, commands);
            }
        }
    }
}
