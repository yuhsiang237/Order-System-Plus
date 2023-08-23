namespace OrderSystemPlus.Models.BusinessActor
{
    public class RspGetProductList
    {
        public int TotalCount { get; set; }
        public List<RspGetProductListItem> Data { get; set; }
    }
}
