using OrderSystemPlus.Enums;

namespace OrderSystemPlus.Models.BusinessActor.Commands
{
    public class ReqCreateProductInventory
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public string Description { get; set; }
    }
}
