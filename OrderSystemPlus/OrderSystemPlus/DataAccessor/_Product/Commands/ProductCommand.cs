using System.Data.SqlClient;

using Dapper;

using OrderSystemPlus.Models.DataAccessor.Commands;
using OrderSystemPlus.Models;

namespace OrderSystemPlus.DataAccessor.Commands
{
    public class ProductCommand :
        IInsertCommand<IEnumerable<ProductCommandModel>>,
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

        public async Task InsertAsync(IEnumerable<ProductCommandModel> commands)
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
                ";
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                await conn.ExecuteAsync(sql, commands);
            }
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
