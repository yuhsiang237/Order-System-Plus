using System.ComponentModel.DataAnnotations;

namespace OrderSystemPlus.Enums
{
    public enum ShipmentOrderStatus
    {
        /// <summary>
        /// 進行中
        /// </summary>
        [Display(Name = "訂單進行中")]
        Processing = 1,

        /// <summary>
        /// 取消
        /// </summary>
        [Display(Name = "取消")]
        Cancel = 2,

        /// <summary>
        /// 訂單已完成
        /// </summary>
        [Display(Name = "已完成")]
        Finish = 3,
    }
}
