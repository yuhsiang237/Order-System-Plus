using OrderSystemPlus.Enums;

namespace OrderSystemPlus.Models.BusinessActor
{
    public class ReqGetUserList
    {
        public string? Account { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string? SortField { get; set; }
        public SortType? SortType { get; set; }
    }
}
