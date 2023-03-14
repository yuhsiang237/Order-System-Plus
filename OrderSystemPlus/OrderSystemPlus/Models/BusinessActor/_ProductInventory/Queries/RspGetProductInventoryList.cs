using OrderSystemPlus.Enums;

namespace OrderSystemPlus.Models.BusinessActor.Queries
{
    public class RspGetProductInventoryList
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ProductId
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// ActionType
        /// </summary>
        public InventoryActionType ActionType { get; set; }
        /// <summary>
        /// 數量
        /// </summary>
        public decimal Quantity { get; set; }
    }
}
