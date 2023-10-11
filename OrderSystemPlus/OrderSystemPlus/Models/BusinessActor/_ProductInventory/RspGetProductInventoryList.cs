namespace OrderSystemPlus.Models.BusinessActor
{
    public class RspGetProductInventoryList
    {
        public int TotalCount { get; set; }
        public List<RspGetProductInventoryListItem> Data { get; set; }
    }
}
