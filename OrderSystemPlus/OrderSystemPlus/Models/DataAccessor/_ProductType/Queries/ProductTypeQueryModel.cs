namespace OrderSystemPlus.Models.DataAccessor.Queries
{
    /// <summary>
    /// ProductTypeQueryModel
    /// </summary>
    public class ProductTypeQueryModel
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
    }
}
