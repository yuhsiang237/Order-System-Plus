namespace OrderSystemPlus.Models.DataAccessor
{
    /// <summary>
    /// ProductTypeDto
    /// </summary>
    public class ProductTypeDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 產品分類名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 產品描述
        /// </summary>
        public string Description { get; set; }

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
