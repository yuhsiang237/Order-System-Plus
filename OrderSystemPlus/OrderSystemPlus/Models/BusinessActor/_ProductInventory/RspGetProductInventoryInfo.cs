using OrderSystemPlus.Enums;

namespace OrderSystemPlus.Models.BusinessActor
{
    public class RspGetProductInventoryInfo
    {
        public int? ProductId { get; set; }
        public decimal? Quantity { get; set; }
    }
}