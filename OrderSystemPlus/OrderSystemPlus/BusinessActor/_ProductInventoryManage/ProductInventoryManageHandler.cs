using System.Linq;

using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Enums;
using OrderSystemPlus.Models.BusinessActor;
using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlus.BusinessActor
{
    public class ProductInventoryManageHandler : IProductInventoryManageHandler
    {
        private readonly IProductInventoryRepository _productInventoryRepository;
        private static SemaphoreSlim _updateProductInventorySemaphoreSlim;
        public ProductInventoryManageHandler(IProductInventoryRepository
            productInventoryRepository)
        {
            _updateProductInventorySemaphoreSlim = new SemaphoreSlim(1, 1);
            _productInventoryRepository = productInventoryRepository;
        }

        public async Task<bool> HandleAsync(List<ReqUpdateProductInventory> req)
        {
            await _updateProductInventorySemaphoreSlim.WaitAsync();
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
                _updateProductInventorySemaphoreSlim.Release();
            }
        }

        public async Task<decimal?> GetProductInventoryInfoAsync(int? productId)
        {
            var currentInventory = (await _productInventoryRepository.FindByOptionsAsync(productId))
                                        .Sum(s => s.Quantity);
            return currentInventory;
        }

        public async Task<List<RspGetProductInventoryHistoryList>> GetProductInventoryHistoryListAsync(ReqGetProductInventoryHistoryList req)
        {
            return (await _productInventoryRepository.FindByOptionsAsync(req.ProductId))
                .Select(s => new RspGetProductInventoryHistoryList
                {
                    ProductId = s.ProductId,
                    Quantity = s.Quantity,
                }).ToList();
        }
    }
}