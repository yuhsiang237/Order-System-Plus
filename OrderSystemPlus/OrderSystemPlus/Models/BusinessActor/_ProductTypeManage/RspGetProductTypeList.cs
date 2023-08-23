namespace OrderSystemPlus.Models.BusinessActor
{
    public class RspGetProductTypeList
    {
        public int TotalCount { get; set; }
        public List<RspGetProductTypeListItem> Data { get; set; }
    }
}
