namespace OrderSystemPlus.Models.BusinessActor
{
    /// <summary>
    /// RspGetShipmentOrderInfo
    /// </summary>
    public class RspGetShipmentOrderInfo
    {
        public string OrderNumber { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? RecipientName { get; set; }
        public int? OperatorUserId { get; set; }
        public int? Status { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }
        public List<RspShipmentOrderDetail> Details { get; set; }
    }
    public class RspShipmentOrderDetail
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public int? ProductId { get; set; }
        public string ProductNumber { get; set; }
        public string ProductName { get; set; }
        public decimal? ProductPrice { get; set; }
        public decimal? ProductQuantity { get; set; }
        public string Remarks { get; set; }
    }
}
