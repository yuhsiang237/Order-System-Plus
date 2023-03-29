using System.Data.SqlClient;

using Dapper;

using OrderSystemPlus.Models.DataAccessor.Commands;
using OrderSystemPlus.Models;
using System.Transactions;
using System.Data;

namespace OrderSystemPlus.DataAccessor.Commands
{
    public class ProductCommand :
        IInsertCommand<IEnumerable<ProductCommandModel>, List<int>>,
        IDeleteCommand<IEnumerable<ProductCommandModel>>,
        IUpdateCommand<IEnumerable<ProductCommandModel>>
    {
        public async Task DeleteAsync(IEnumerable<ProductCommandModel> commands)
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
                await conn.ExecuteAsync(sql, commands);
            }
        }

        public async Task<List<int>> InsertAsync(IEnumerable<ProductCommandModel> commands)
        {
            var result = new List<int>();
            using (var ts = new TransactionScope())
            {
                using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
                {
                    result = commands
                        .Select(s => InsertAsync(s, conn))
                        .ToList();

                    if (!result.Any(a => a == 0))
                        ts.Complete();
                }
            }

            return await Task.FromResult(result);
        }

        private int InsertAsync(ProductCommandModel command, IDbConnection cn)
        {
            var sql = @"
                INSERT INTO [dbo].[Product]
                (
                  [Number]
                  ,[Name]
                  ,[Price]
                  ,[Description]
                  ,[CurrentUnit]
                  ,[CreatedOn]
                  ,[UpdatedOn]
                  ,[IsValid]
                ) VALUES
                (
                  @Number,
                  @Name,
                  @Price,
                  @Description,
                  @CurrentUnit,
                  @CreatedOn,
                  @UpdatedOn,
                  @IsValid
                )
                SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];
                ";

            return cn.ExecuteScalar<int>(sql, command);
        }

        public async Task UpdateAsync(IEnumerable<ProductCommandModel> commands)
        {
            var sql = @"
                UPDATE [dbo].[Product]
                SET
                  [Name] = @Name,
                  [Price] = @Price,
                  [Number] = @Number,
                  [Description] = @Description,
                  [CurrentUnit] = @CurrentUnit,
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
