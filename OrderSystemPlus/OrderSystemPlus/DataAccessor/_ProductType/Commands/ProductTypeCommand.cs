using System.Data.SqlClient;

using Dapper;

using OrderSystemPlus.Models.DataAccessor.Commands;
using OrderSystemPlus.Models;

namespace OrderSystemPlus.DataAccessor.Commands
{
    public class ProductTypeCommand :
        IInsertCommand<IEnumerable<ProductTypeCommandModel>>,
        IDeleteCommand<IEnumerable<ProductTypeCommandModel>>,
        IUpdateCommand<IEnumerable<ProductTypeCommandModel>>
    {
        public async Task DeleteAsync(IEnumerable<ProductTypeCommandModel> commands)
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
                await conn.ExecuteAsync(sql, commands);
            }
        }

        public async Task InsertAsync(IEnumerable<ProductTypeCommandModel> commands)
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
                ";
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                await conn.ExecuteAsync(sql, commands);
            }
        }

        public async Task UpdateAsync(IEnumerable<ProductTypeCommandModel> commands)
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
                await conn.ExecuteAsync(sql, commands);
            }
        }
    }
}
