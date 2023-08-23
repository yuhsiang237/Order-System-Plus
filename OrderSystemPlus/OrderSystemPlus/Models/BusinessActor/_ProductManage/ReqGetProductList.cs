namespace OrderSystemPlus.Models.BusinessActor
{
    public class ReqGetProductList
    {
        /// <summary>
        /// 產品名稱
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 產品編號
        /// </summary>
        public string? Number { get; set; }
    }
}
