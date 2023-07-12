namespace OrderSystemPlus.Models.BusinessActor
{
    /// <summary>
    /// ReqCreateShipmentOrder
    /// </summary>
    public class ReqCreateShipmentOrder
    {
        public string? RecipientName { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }
        public List<ShipmentOrderDetailModel> Details { get; set; }
        public class ShipmentOrderDetailModel
        {
            public int? ProductId { get; set; }
            public decimal ProductQuantity { get; set; }
            public string Remarks { get; set; }
        }
    }
}
