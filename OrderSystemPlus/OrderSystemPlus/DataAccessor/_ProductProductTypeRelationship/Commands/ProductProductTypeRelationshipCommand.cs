using System.Data.SqlClient;

using Dapper;

using OrderSystemPlus.Models.DataAccessor.Commands;
using OrderSystemPlus.Models;

namespace OrderSystemPlus.DataAccessor.Commands
{
    public class ProductProductTypeRelationshipCommand :
        IInsertCommand<IEnumerable<ProductProductTypeRelationshipCommandModel>>,
        IDeleteCommand<IEnumerable<ProductProductTypeRelationshipCommandModel>>
    {
        public async Task DeleteAsync(IEnumerable<ProductProductTypeRelationshipCommandModel> commands)
        {
            var sql = @"
                DELETE FROM [dbo].[ProductProductTypeRelationship]
                WHERE
                    [Id] = @Id
                ";
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                await conn.ExecuteAsync(sql, commands);
            }
        }

        public async Task InsertAsync(IEnumerable<ProductProductTypeRelationshipCommandModel> commands)
        {
            var sql = @"
                INSERT INTO [dbo].[ProductProductTypeRelationship]
                (
                 [ProductId]
                 ,[ProductTypeId]
                ) VALUES
                (
                  @ProductId
                 ,@ProductTypeId
                )
                ";
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                await conn.ExecuteAsync(sql, commands);
            }
        }
    }
}
