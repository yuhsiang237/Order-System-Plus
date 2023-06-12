using OrderSystemPlus.Enums;

namespace OrderSystemPlus.Models.BusinessActor
{
    public class ReqUpdateProductInventory
    {
        public AdjustProductInventoryType Type { get; set; }
        public int? ProductId { get; set; }
        public decimal? Quantity { get; set; }
    }
}