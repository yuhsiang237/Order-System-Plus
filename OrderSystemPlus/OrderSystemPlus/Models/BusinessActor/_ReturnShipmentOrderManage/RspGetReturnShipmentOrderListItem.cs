namespace OrderSystemPlus.Models.BusinessActor
{
    /// <summary>
    /// RspGetReturnShipmentOrderListItem
    /// </summary>
    public class RspGetReturnShipmentOrderListItem
    {
        public string ReturnShipmentOrderNumber { get; set; }
        public string ShipmentOrderNumber { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal? TotalReturnAmount { get; set; }
        public string Remark { get; set; }
        public int? OperatorUserId { get; set; }
    }
}
