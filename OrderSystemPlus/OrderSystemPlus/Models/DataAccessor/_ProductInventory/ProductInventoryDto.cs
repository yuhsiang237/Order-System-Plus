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
        /// Product Id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// ActionType
        /// </summary>
        public InventoryActionType ActionType { get; set; }
        /// <summary>
        /// 產品庫存量
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 產品描述
        /// </summary>
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsValid { get; set; }
    }
}
