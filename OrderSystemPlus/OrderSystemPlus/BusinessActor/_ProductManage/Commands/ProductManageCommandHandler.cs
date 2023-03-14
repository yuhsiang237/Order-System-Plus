using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.DataAccessor.Queries;
using OrderSystemPlus.Models.BusinessActor.Commands;
using OrderSystemPlus.Models.DataAccessor.Commands;

namespace OrderSystemPlus.BusinessActor.Commands
{
    public class ProductManageCommandHandler :
        ICommandHandler<ReqCreateProductType>,
        ICommandHandler<ReqUpdateProductType>,
        ICommandHandler<ReqDeleteProductType>,
        ICommandHandler<ReqCreateProduct>,
        ICommandHandler<ReqUpdateProduct>,
        ICommandHandler<ReqDeleteProduct>
    {
        private readonly IInsertCommand<IEnumerable<ProductTypeCommandModel>> _productTypeInsert;
        private readonly IUpdateCommand<IEnumerable<ProductTypeCommandModel>> _productTypeUpdate;
        private readonly IDeleteCommand<IEnumerable<ProductTypeCommandModel>> _productTypeDelete;
        private readonly IInsertCommand<IEnumerable<ProductCommandModel>> _productInsert;
        private readonly IUpdateCommand<IEnumerable<ProductCommandModel>> _productUpdate;
        private readonly IDeleteCommand<IEnumerable<ProductCommandModel>> _productDelete;
        private readonly IProductTypeQuery _productTypeQuery;
        private readonly IProductQuery _productQuery;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="insertCommand"></param>
        /// <param name="query"></param>
        public ProductManageCommandHandler(
            IInsertCommand<IEnumerable<ProductTypeCommandModel>> productTypeInsert,
            IUpdateCommand<IEnumerable<ProductTypeCommandModel>> productTypeUpdate,
            IDeleteCommand<IEnumerable<ProductTypeCommandModel>> productTypeDelete,
            IInsertCommand<IEnumerable<ProductCommandModel>> productInsert,
            IUpdateCommand<IEnumerable<ProductCommandModel>> productUpdate,
            IDeleteCommand<IEnumerable<ProductCommandModel>> productDelete,
            IProductTypeQuery productTypeQuery,
            IProductQuery productQuery)
        {
            _productTypeInsert = productTypeInsert;
            _productTypeUpdate = productTypeUpdate;
            _productTypeDelete = productTypeDelete;
            _productInsert = productInsert;
            _productUpdate = productUpdate;
            _productDelete = productDelete;
            _productTypeQuery = productTypeQuery;
            _productQuery = productQuery;
        }

        /// <summary>
        /// Create Product
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task HandleAsync(ReqCreateProduct command)
        {
            var hasExist = (await _productQuery.FindByOptionsAsync(null, command.Name, command.Number)).Any();
            if (!hasExist)
            {
                await _productInsert.InsertAsync(
                    new List<ProductCommandModel>
                    {
                        new ProductCommandModel
                        {
                            Number = command.Number,
                            CurrentUnit = command.CurrentUnit,
                            Price = command.Price,
                            Name = command.Name,
                            Description = command.Description,
                            CreatedOn = DateTime.Now,
                            UpdatedOn = DateTime.Now,
                            IsValid = true,
                        }
                    });
            }
            else
            {
                throw new Exception("已存在");
            }
        }

        /// <summary>
        /// Create ProductType
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task HandleAsync(ReqCreateProductType command)
        {
            var hasExist = (await _productTypeQuery.FindByOptionsAsync(null, command.Name)).Any();
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
                            IsValid = true,
                        }
                    });
            }
            else
            {
                throw new Exception("已存在");
            }
        }

        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task HandleAsync(ReqUpdateProduct command)
        {
            await _productUpdate.UpdateAsync(
                new List<ProductCommandModel>
                {
                    new ProductCommandModel
                    {
                        Id  = command.Id,
                        Number = command.Number,
                        CurrentUnit = command.CurrentUnit,
                        Price = command.Price,
                        Name = command.Name,
                        Description = command.Description,
                        UpdatedOn = DateTime.Now,
                        IsValid = true,
                    }
                });
        }

        /// <summary>
        /// Update ProductType
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task HandleAsync(ReqUpdateProductType command)
        {
            await _productTypeUpdate.UpdateAsync(
                new List<ProductTypeCommandModel>
                {
                    new ProductTypeCommandModel
                    {
                        Id  = command.Id,
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
        public async Task HandleAsync(ReqDeleteProductType command)
        {
            await _productTypeDelete.DeleteAsync(new List<ProductTypeCommandModel>
            {
                new ProductTypeCommandModel
                {
                    Id = command.Id,
                    UpdatedOn= DateTime.Now,
                }
            });
        }

        /// <summary>
        /// Delete Product
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task HandleAsync(ReqDeleteProduct command)
        {
            await _productDelete.DeleteAsync(new List<ProductCommandModel>
            {
                new ProductCommandModel
                {
                    Id = command.Id,
                    UpdatedOn= DateTime.Now,
                }
            });
        }
    }
}
