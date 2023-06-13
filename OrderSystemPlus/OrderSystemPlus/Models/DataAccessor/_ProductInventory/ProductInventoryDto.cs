using OrderSystemPlus.Enums;

namespace OrderSystemPlus.Models.DataAccessor
{
    /// <summary>
    /// ProductInventoryDto
    /// </summary>
    public class ProductInventoryDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// AdjustProductInventoryType
        /// </summary>
        public AdjustProductInventoryType AdjustProductInventoryType { get; set; }
        
        /// <summary>
        /// 調整數量
        /// </summary>
        public decimal? AdjustQuantity { get; set; }

        /// <summary>
        /// 調整前總數量
        /// </summary>
        public decimal? PrevTotalQuantity { get; set; }

        /// <summary>
        /// 調整後總數量
        /// </summary>
        public decimal? TotalQuantity { get; set; }

        /// <summary>
        /// 產品描述
        /// </summary>
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsValid { get; set; }
    }
}
