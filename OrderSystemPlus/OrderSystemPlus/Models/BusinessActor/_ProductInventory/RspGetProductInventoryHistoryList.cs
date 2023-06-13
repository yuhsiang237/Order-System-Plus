using OrderSystemPlus.Enums;

namespace OrderSystemPlus.Models.BusinessActor
{
    public class RspGetProductInventoryHistoryList
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
        /// 備註
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime? CreatedOn { get; set; }
    }
}