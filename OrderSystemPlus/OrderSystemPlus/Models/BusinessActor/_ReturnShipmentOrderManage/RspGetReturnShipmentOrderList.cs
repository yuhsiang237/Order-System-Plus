namespace OrderSystemPlus.Models.BusinessActor
{
    /// <summary>
    /// RspGetReturnShipmentOrderList
    /// </summary>
    public class RspGetReturnShipmentOrderList
    {
        public int TotalCount { get; set; }
        public List<RspGetReturnShipmentOrderListItem> Data { get; set; }
    }
}
