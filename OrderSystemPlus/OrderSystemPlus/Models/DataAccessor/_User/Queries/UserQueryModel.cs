namespace OrderSystemPlus.Models.DataAccessor.Queries
{
    /// <summary>
    /// UserQueryModel
    /// </summary>
    public class UserQueryModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 加鹽
        /// </summary>
        public string? Salt { get; set; }

        /// <summary>
        /// 信箱
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        public string? Account { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>

        public string? Password { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public int? RoleId { get; set; }
    }
}
