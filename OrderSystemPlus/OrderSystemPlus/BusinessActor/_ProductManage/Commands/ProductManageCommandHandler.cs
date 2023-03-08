using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.DataAccessor.Queries;
using OrderSystemPlus.Models.BusinessActor.Commands;
using OrderSystemPlus.Models.DataAccessor.Commands;

namespace OrderSystemPlus.BusinessActor.Commands
{
    public class ProductManageCommandHandler :
        ICommandHandler<ReqProductTypeCreate>,
        ICommandHandler<ReqProductTypeUpdate>,
        ICommandHandler<ReqProductTypeDelete>
    {
        private readonly IInsertCommand<IEnumerable<ProductTypeCommandModel>> _productTypeInsert;
        private readonly IUpdateCommand<IEnumerable<ProductTypeCommandModel>> _productTypeUpdate;
        private readonly IDeleteCommand<IEnumerable<ProductTypeCommandModel>> _productTypeDelete;
        private readonly IProductTypeQuery _query;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="insertCommand"></param>
        /// <param name="query"></param>
        public ProductManageCommandHandler(
            IInsertCommand<IEnumerable<ProductTypeCommandModel>> insert,
            IUpdateCommand<IEnumerable<ProductTypeCommandModel>> update,
            IDeleteCommand<IEnumerable<ProductTypeCommandModel>> delete,
            IProductTypeQuery query)
        {
            _productTypeInsert = insert;
            _productTypeUpdate = update;
            _productTypeDelete = delete;
            _query = query;
        }

        /// <summary>
        /// Create ProductType
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task HandleAsync(ReqProductTypeCreate command)
        {
            var hasExist = (await _query.FindByOptionsAsync(null, command.Name)).Any();
            if (!hasExist)
            {
                await _productTypeInsert.InsertAsync(
                    new List<ProductTypeCommandModel>
                    {
                        new ProductTypeCommandModel
                        {
                            Name = command.Name,
                            Description = command.Description,
                            CreatedOn = DateTime.Now,
                            UpdatedOn = DateTime.Now,
                        }
                    });
            }
        }

        /// <summary>
        /// Update ProductType
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task HandleAsync(ReqProductTypeUpdate command)
        {
            await _productTypeUpdate.UpdateAsync(
                new List<ProductTypeCommandModel>
                {
                    new ProductTypeCommandModel
                    {
                        Name = command.Name,
                        Description = command.Description,
                        UpdatedOn = DateTime.Now,
                    }
                });
        }

        /// <summary>
        /// Delete ProductType
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task HandleAsync(ReqProductTypeDelete command)
        {
            await _productTypeDelete.DeleteAsync(new List<ProductTypeCommandModel>
            {
                new ProductTypeCommandModel
                {
                    Id = command.Id,
                }
            });
        }
    }
}
