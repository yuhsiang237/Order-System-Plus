using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Enums;
using OrderSystemPlus.Models.BusinessActor;
using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlus.BusinessActor
{
    public class ProductInventoryControlHandler : IProductInventoryControlHandler
    {
        private readonly IProductInventoryRepository _productInventoryRepository;
        private static SemaphoreSlim _adjustProductInventorySemaphoreSlim;
        public ProductInventoryControlHandler(IProductInventoryRepository
            productInventoryRepository)
        {
            _adjustProductInventorySemaphoreSlim = new SemaphoreSlim(1, 1);
            _productInventoryRepository = productInventoryRepository;
        }

        public async Task<bool> AdjustProductInventoryAsync(List<ReqAdjustProductInventory> req)
        {
            await _adjustProductInventorySemaphoreSlim.WaitAsync();
            try
            {
                var dtoList = new List<ProductInventoryDto>();
                var now = DateTime.Now;
                for (var i = 0; i < req.Count; i++)
                {
                    var item = req[i];
                    var currentInventory = (await _productInventoryRepository.FindByOptionsAsync(item.ProductId))
                                                .Sum(s => s.Quantity);

                    if (item.Type == AdjustProductInventoryType.Decrease &&
                        item.Quantity != 0)
                    {
                        var calcInventory = currentInventory - Math.Abs(item.Quantity.Value);
                        if (calcInventory < 0)
                            throw new Exception("Inventory < 0");
                        dtoList.Add(new ProductInventoryDto
                        {
                            ProductId = item.ProductId.Value,
                            Quantity = -Math.Abs(item.Quantity.Value),
                            CreatedOn = now,
                            UpdatedOn = now,
                            IsValid = true,
                        });
                    }
                    else if (item.Type == AdjustProductInventoryType.Force &&
                        item.Quantity != currentInventory)
                    {
                            var calcInventory = item.Quantity - currentInventory;
                            if (item.Quantity < 0)
                                throw new Exception("Inventory < 0");

                            dtoList.Add(new ProductInventoryDto
                            {
                                ProductId = item.ProductId.Value,
                                Quantity = calcInventory.Value,
                                CreatedOn = now,
                                UpdatedOn = now,
                                IsValid = true,
                            });
                    }
                    else if (item.Type == AdjustProductInventoryType.Increase &&
                        item.Quantity != 0)
                    {
                        var calcInventory = currentInventory + Math.Abs(item.Quantity.Value);
                        if (calcInventory < 0)
                            throw new Exception("Inventory < 0");
                        dtoList.Add(new ProductInventoryDto
                        {
                            ProductId = item.ProductId.Value,
                            Quantity = Math.Abs(item.Quantity.Value),
                            CreatedOn = now,
                            UpdatedOn = now,
                            IsValid = true,
                        });
                    }
                }

                await _productInventoryRepository.InsertAsync(dtoList);

                return true;
            }
            finally
            {
                _adjustProductInventorySemaphoreSlim.Release();
            }
        }

        public async Task<decimal?> GetProductInventoryAsync(int productId)
        {
            var currentInventory = (await _productInventoryRepository.FindByOptionsAsync(productId))
                                        .Sum(s => s.Quantity);
            return currentInventory;
        }
    }
}
