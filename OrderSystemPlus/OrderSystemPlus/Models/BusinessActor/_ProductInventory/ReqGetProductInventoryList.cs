using OrderSystemPlus.Enums;

namespace OrderSystemPlus.Models.BusinessActor
{
    public class ReqGetProductInventoryList
    {
        /// <summary>
        /// 產品名稱
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 產品編號
        /// </summary>
        public string? Number { get; set; }

        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string? SortField { get; set; }
        public SortType? SortType { get; set; }
    }
}
