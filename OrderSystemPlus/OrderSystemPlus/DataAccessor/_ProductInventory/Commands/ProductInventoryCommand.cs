using System.Data.SqlClient;

using Dapper;

using OrderSystemPlus.Models.DataAccessor.Commands;
using OrderSystemPlus.Models;

namespace OrderSystemPlus.DataAccessor.Commands
{
    public class ProductInventoryCommand :
        IInsertCommand<IEnumerable<ProductInventoryCommandModel>>,
        IDeleteCommand<IEnumerable<ProductInventoryCommandModel>>,
        IUpdateCommand<IEnumerable<ProductInventoryCommandModel>>
    {
        public async Task DeleteAsync(IEnumerable<ProductInventoryCommandModel> commands)
        {
            var sql = @"
                UPDATE [dbo].[ProductInventory]
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

        public async Task InsertAsync(IEnumerable<ProductInventoryCommandModel> commands)
        {
            var sql = @"
                INSERT INTO [dbo].[ProductInventory]
                (
                  [ProductId]
                  ,[Quantity]
                  ,[Description]
                  ,[ActionType]
                  ,[CreatedOn]
                  ,[UpdatedOn]
                  ,[IsValid]
                ) VALUES
                (
                  @ProductId,
                  @Quantity,
                  @Description,
                  @ActionType,
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

        public async Task UpdateAsync(IEnumerable<ProductInventoryCommandModel> commands)
        {
            var sql = @"
                UPDATE [dbo].[ProductInventory]
                SET
                  [ProductId] = @ProductId,
                  [Quantity]= @Quantity,
                  [Description]= @Description,
                  [ActionType] = @ActionType,
                  [UpdatedOn]= @UpdatedOn,
                  [IsValid]= @IsValid
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
