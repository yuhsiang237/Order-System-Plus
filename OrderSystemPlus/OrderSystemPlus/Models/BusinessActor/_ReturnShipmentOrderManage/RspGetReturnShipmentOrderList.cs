namespace OrderSystemPlus.Models.BusinessActor
{
    /// <summary>
    /// RspGetReturnShipmentOrderList
    /// </summary>
    public class RspGetReturnShipmentOrderList
    {
        public string ReturnShipmentOrderNumber { get; set; }
        public string ShipmentOrderNumber { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal? TotalReturnAmount { get; set; }
        public string Remark { get; set; }
        public int? OperatorUserId { get; set; }
    }
}
