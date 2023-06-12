namespace OrderSystemPlus.Models.DataAccessor
{
    /// <summary>
    /// ProductDto
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 產品名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 產品編號
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 產品價格
        /// </summary>
        public decimal Price { get; set; }


        /// <summary>
        /// 產品描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 產品數量
        /// </summary>
        public decimal? Quantity { get; set; }

        /// <summary>
        /// 是否為生效資料
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 資料建立時間
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 資料更新時間
        /// </summary>
        public DateTime UpdatedOn { get; set; }
    }
}
