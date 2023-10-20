namespace OrderSystemPlus.Models.BusinessActor
{
    /// <summary>
    /// RspGetShipmentOrderList
    /// </summary>
    public class RspGetShipmentOrderList
    {
        public int TotalCount { get; set; }
        public List<RspGetShipmentOrderListItem> Data { get; set; }
    }
}
