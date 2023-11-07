using System.ComponentModel;

namespace Postex.SharedKernel.Common.Enums
{
    public enum CourierServiceCode
    {
        [Description("همه ")] All = 0,
        [Description("پست پیشتاز")] PostPishtaz = 1,
        [Description("پست سفارشی")] PostSefareshi = 2,
        [Description("پست ویژه")] PostVizhe = 3,
        [Description("چاپار")] Chapar = 4,
        [Description("چاپار اکسپرس")] ChaparExpress = 5,
        [Description("ماهکس")] Mahex = 6,
        [Description("کالارسان")] Kalaresan = 7,
        [Description("تعارف")] Taroff = 8,
        [Description("لینک")] Link = 9,
        [Description("پیشروپست")] PishroPost = 10,
        [Description("اسپید")] Speed = 11,
        [Description("پیک هاب")] Paykhub = 12,
        [Description("آرامکس")] Aramex = 13,
        [Description("اکوپیک")] EcoPeyk = 14,
    }
}