namespace OrderSystemPlus.Models.BusinessActor
{
    public class RspGetUserList
    {
        public int TotalCount { get; set; }
        public List<RspGetUserListItem> Data { get; set; }
    }
}
