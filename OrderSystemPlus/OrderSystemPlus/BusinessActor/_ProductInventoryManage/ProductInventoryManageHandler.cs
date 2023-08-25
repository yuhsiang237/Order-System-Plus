using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Enums;
using OrderSystemPlus.Models.BusinessActor;
using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlus.BusinessActor
{
    public class ProductInventoryManageHandler : IProductInventoryManageHandler
    {
        private readonly IProductInventoryRepository _productInventoryRepository;
        private readonly IProductRepository _productRepository;
        private static SemaphoreSlim _updateProductInventorySemaphoreSlim;
        public ProductInventoryManageHandler(
            IProductInventoryRepository productInventoryRepository,
            IProductRepository productRepository)
        {
            _updateProductInventorySemaphoreSlim = new SemaphoreSlim(1, 1);
            _productInventoryRepository = productInventoryRepository;
            _productRepository = productRepository;
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
                    if (!await isProductExist(item.ProductId.Value))
                        throw new Exception($"Not found product id:{item.ProductId}");

                    var currentQuantity = (await GetProductCurrentTotalQuantityAsync(new ReqGetProductCurrentTotalQuantity
                    {
                        ProductId = req[i].ProductId.Value
                    }));

                    if (item.Type == AdjustProductInventoryType.IncreaseDecrease &&
                        item.AdjustQuantity != 0)
                    {
                        var calcQuantity = currentQuantity + item.AdjustQuantity.Value;
                        if (calcQuantity < 0)
                            throw new Exception("Inventory < 0");

                        dtoList.Add(new ProductInventoryDto
                        {
                            ProductId = item.ProductId.Value,
                            PrevTotalQuantity = currentQuantity,
                            AdjustQuantity = item.AdjustQuantity.Value,
                            TotalQuantity = calcQuantity,
                            AdjustProductInventoryType = AdjustProductInventoryType.IncreaseDecrease,
                            Remark = item.Description + $"調整庫存: {currentQuantity}=>{calcQuantity}。",
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
                            Remark = item.Description + $"調整庫存: {currentQuantity}=>{calcQuantity}。",
                            TotalQuantity = calcQuantity.Value,
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

        public async Task<decimal?> GetProductCurrentTotalQuantityAsync(ReqGetProductCurrentTotalQuantity req)
        {
            var currentQuantity = (await _productInventoryRepository.FindByOptionsAsync(req.ProductId))
                                                .OrderByDescending(o => o.CreatedOn)
                                                .ThenByDescending(o => o.Id)
                                                .FirstOrDefault()
                                                ?.TotalQuantity ?? 0;
            return currentQuantity;
        }

        public async Task<List<RspGetProductInventoryHistoryList>> GetProductInventoryHistoryListAsync(ReqGetProductInventoryHistoryList req)
        {
            return (await _productInventoryRepository.FindByOptionsAsync(req.ProductId))
                .Select(s => new RspGetProductInventoryHistoryList
                {
                    Id = s.Id,
                    ProductId = s.ProductId,
                    AdjustQuantity = s.AdjustQuantity,
                    PrevTotalQuantity = s.PrevTotalQuantity,
                    AdjustProductInventoryType = s.AdjustProductInventoryType,
                    TotalQuantity = s.TotalQuantity,
                    Remark = s.Remark,
                    CreatedOn = s.CreatedOn,
                })
                .OrderByDescending(o => o.CreatedOn)
                .ThenByDescending(o => o.Id)
                .ToList();
        }

        private async Task<bool> isProductExist(int productId)
        {
            return (await _productRepository.FindByOptionsAsync(id: productId)).Data.Any();
        }
    }
}