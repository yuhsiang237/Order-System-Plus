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
                    var currentQuantity = (await _productInventoryRepository.FindByOptionsAsync(item.ProductId))
                                                .OrderByDescending(o => o.CreatedOn)
                                                .ThenByDescending(o => o.Id)
                                                .FirstOrDefault()
                                                ?.TotalQuantity ?? 0;

                    if (item.Type == AdjustProductInventoryType.Decrease &&
                        item.AdjustQuantity != 0)
                    {
                        var calcQuantity = currentQuantity - Math.Abs(item.AdjustQuantity.Value);
                        if (calcQuantity < 0)
                            throw new Exception("Inventory < 0");
                        dtoList.Add(new ProductInventoryDto
                        {
                            ProductId = item.ProductId.Value,
                            PrevTotalQuantity = currentQuantity,
                            AdjustQuantity = -Math.Abs(item.AdjustQuantity.Value),
                            TotalQuantity = calcQuantity,
                            AdjustProductInventoryType = AdjustProductInventoryType.Decrease,
                            Description = item.Description + $"調整庫存: {currentQuantity}=>{calcQuantity}。",
                            CreatedOn = now,
                            UpdatedOn = now,
                            IsValid = true,
                        });
                    }
                    else if (item.Type == AdjustProductInventoryType.Force &&
                        item.AdjustQuantity != currentQuantity)
                    {
                        var calcQuantity = item.AdjustQuantity - currentQuantity;
                        if (item.AdjustQuantity < 0)
                            throw new Exception("Inventory < 0");

                        dtoList.Add(new ProductInventoryDto
                        {
                            ProductId = item.ProductId.Value,
                            PrevTotalQuantity = currentQuantity,
                            AdjustQuantity = calcQuantity.Value,
                            AdjustProductInventoryType = AdjustProductInventoryType.Force,
                            Description = item.Description + $"調整庫存: {currentQuantity}=>{calcQuantity}。",
                            TotalQuantity = calcQuantity.Value,
                            CreatedOn = now,
                            UpdatedOn = now,
                            IsValid = true,
                        });
                    }
                    else if (item.Type == AdjustProductInventoryType.Increase &&
                        item.AdjustQuantity != 0)
                    {
                        var calcQuantity = currentQuantity + Math.Abs(item.AdjustQuantity.Value);
                        if (calcQuantity < 0)
                            throw new Exception("Inventory < 0");
                        dtoList.Add(new ProductInventoryDto
                        {
                            ProductId = item.ProductId.Value,
                            PrevTotalQuantity = currentQuantity,
                            AdjustQuantity = Math.Abs(item.AdjustQuantity.Value),
                            Description = item.Description + $"調整庫存: {currentQuantity}=>{calcQuantity}。",
                            AdjustProductInventoryType = AdjustProductInventoryType.Increase,
                            TotalQuantity = calcQuantity,
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
            var currentQuantity = (await _productInventoryRepository.FindByOptionsAsync(productId))
                                                .OrderByDescending(o => o.CreatedOn)
                                                .ThenByDescending(o => o.Id)
                                                .FirstOrDefault()
                                                ?.TotalQuantity;
            return currentQuantity;
        }

        public async Task<List<RspGetProductInventoryHistoryList>> GetProductInventoryHistoryListAsync(ReqGetProductInventoryHistoryList req)
        {
            return (await _productInventoryRepository.FindByOptionsAsync(req.ProductId))
                .Select(s => new RspGetProductInventoryHistoryList
                {
                    ProductId = s.ProductId,
                    Quantity = s.AdjustQuantity,
                }).ToList();
        }
    }
}