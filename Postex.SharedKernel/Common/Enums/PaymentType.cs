using System.ComponentModel;

namespace Postex.SharedKernel.Common.Enums
{
    public enum PaymentType
    {
        [Description("پرداخت در محل")] Cod = 0,
        [Description("پرداخت آنلاين")] Online = 1,
        [Description("پرداخت در محل ارسال رايگان")] FreePost = 2,
    }
}