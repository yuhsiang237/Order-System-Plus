using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.DataAccessor.Queries;
using OrderSystemPlus.Enums;
using OrderSystemPlus.Models.BusinessActor.Commands;
using OrderSystemPlus.Models.DataAccessor.Commands;

namespace OrderSystemPlus.BusinessActor.Commands
{
    public class ProductInventoryCommandHandler :
        ICommandHandler<ReqCreateProductInventory>
    {
        private readonly IInsertCommand<IEnumerable<ProductInventoryCommandModel>> _productInventoryInsert;
        private readonly IProductQuery _productQuery;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="productInventoryInsert"></param>
        /// <param name="productInventoryQuery"></param>
        /// <param name="productQuery"></param>
        public ProductInventoryCommandHandler(
            IInsertCommand<IEnumerable<ProductInventoryCommandModel>> productInventoryInsert,
            IProductQuery productQuery)
        {
            _productInventoryInsert = productInventoryInsert;
            _productQuery = productQuery;
        }


        /// <summary>
        /// Create ProductInventory
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task HandleAsync(ReqCreateProductInventory command)
        {
            var hasProductExist = (await _productQuery.FindByOptionsAsync(command.ProductId, null, null)).Any();
            if (!hasProductExist)
            {
                await _productInventoryInsert.InsertAsync(
                    new List<ProductInventoryCommandModel>
                    {
                        new ProductInventoryCommandModel
                        {
                            ProductId = command.ProductId,
                            Quantity = command.Quantity,
                            Description = command.Description,
                            ActionType = InventoryActionType.Mauaual,
                            CreatedOn = DateTime.Now,
                            UpdatedOn = DateTime.Now,
                            IsValid = true,
                        }
                    });
            }
            else
            {
                throw new Exception("不存在商品");
            }
        }
    }
}
