namespace OrderSystemPlus.Models.BusinessActor
{
    /// <summary>
    /// ReqCreateReturnShipmentOrder
    /// </summary>
    public class ReqCreateReturnShipmentOrder
    {
        public string ShipmentOrderNumber { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Remark { get; set; }
        public List<ReturnShipmentOrderDetail2> Details { get; set; }
        public class ReturnShipmentOrderDetail2
        {
            public int Id { get; set; }
            public int? ShipmentOrderDetailId { get; set; }
            public decimal? ReturnProductQuantity { get; set; }
            public string Remarks { get; set; }
        }
    }
}
