using OrderSystemPlus.Enums;

namespace OrderSystemPlus.Models.BusinessActor
{
    public class ReqGetProductTypeList
    {
        public string? Name { get; set; }
        public int? Id { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string SortField { get; set; }
        public SortType? SortType { get; set; }
    }
}
