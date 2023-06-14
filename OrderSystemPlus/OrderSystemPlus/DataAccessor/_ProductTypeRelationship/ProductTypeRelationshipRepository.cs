﻿using System.Data;
using System.Data.SqlClient;
using System.Transactions;

using Dapper;

using OrderSystemPlus.Models;
using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlus.DataAccessor
{
    public class ProductTypeRelationshipRepository : IProductTypeRelationshipRepository
    {
        public async Task<List<ProductTypeRelationshipDto>> FindByOptionsAsync(int? productId = null, int? productTypeId = null)
        {

            string sql = @"
                           SELECT
                                [ProductId],
                                [ProductTypeId]
                           FROM [dbo].[ProductTypeRelationship]";

            var conditions = new List<string>
            {
            };

            if (productId.HasValue)
                conditions.Add("[ProductId] = @ProductId");
            if (productTypeId.HasValue)
                conditions.Add("[ProductTypeId] = @ProductTypeId");

            if (conditions.Any())
                sql = string.Concat(sql, $" WHERE {string.Join(" AND ", conditions)}");

            var result = default(List<ProductTypeRelationshipDto>);
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                result = (await conn.QueryAsync<ProductTypeRelationshipDto>(sql, new
                {
                    ProductId = productId,
                    ProductTypeId = productTypeId,
                })).ToList();
            }

            return result;
        }
        public async Task<List<int>> RefreshAsync(IEnumerable<ProductTypeRelationshipDto> model)
        {
            var result = new List<int>();
            using (var ts = new TransactionScope())
            {
                using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
                {
                    foreach (var item in model)
                        Delete(item, conn);

                    Insert(model, conn);

                    ts.Complete();
                }
            }

            return await Task.FromResult(result);
        }

        private int Insert(IEnumerable<ProductTypeRelationshipDto> command, IDbConnection cn)
        {
            var sql = @"
                INSERT INTO [dbo].[ProductTypeRelationship]
                (
                    [ProductId],
                    [ProductTypeId]
                ) VALUES
                (
                    @ProductId,
                    @ProductTypeId
                )
                ";
            return cn.Execute(sql, command);
        }

        private void Delete(ProductTypeRelationshipDto command, IDbConnection cn)
        {
            var sql = @"
                DELETE FROM [dbo].[ProductTypeRelationship]
                ";
            var conditions = new List<string> { };

            if (command.ProductId.HasValue)
                conditions.Add("[ProductId] = @ProductId");

            if (conditions.Any())
                sql = string.Concat(sql, $" WHERE {string.Join(" AND ", conditions)}");

            cn.Execute(sql, command);
        }
    }
}
