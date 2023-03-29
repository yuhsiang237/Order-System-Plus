namespace OrderSystemPlus.Models.BusinessActor.Commands
{
    public class ReqCreateProduct
    {
        /// <summary>
        /// 產品名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 產品編號
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 產品類別
        /// </summary>
        public List<int> ProductTypeIds { get; set; }

        /// <summary>
        /// 產品價格
        /// </summary>
        public decimal Price { get; set; }


        /// <summary>
        /// 產品描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 產品目前數量
        /// </summary>
        public decimal CurrentUnit { get; set; }
    }
}
