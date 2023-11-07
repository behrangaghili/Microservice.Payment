namespace Postex.Payment.Infrastructure.PaymentMethods.Mellat;

public class SamanResult
{
    public static string SamanVerifyResult(string ID)
    {
        string result = "";
        switch (ID)
        {
            case "-2":
                result = "تراکنش یافت نشد.";
                break;
            case "-6":
                result = "بیش از نیم ساعت از زمان اجرای تراکنش گذشته است.";
                break;
            case "0":
                result = "موفق";
                break;
            case "2":
                result = "درخواست تکراری می باشد.";
                break;
            case "-105":
                result = "ترمینال ارسالی در س یستم موجود نمی باشد";
                break;
            case "-104":
                result = "ترمینال ارسالی غیرفعال می باشد";
                break;
            case "-106":
                result = "آدرس آ ی پی درخواستی غیر مجاز می باشد";
                break;
        }
        return result;
    }
    public static string SamanPaymentResult(string ID)
    {
        string result = "";
        switch (ID)
        {
            case "1":
                result = "کاربر انصراف داده است";
                break;
            case "2":
                result = "پرداخت با موفقیت انجام شد";
                break;
            case "3":
                result = "پرداخت انجام نشد.";
                break;
            case "4":
                result = "کاربر در بازه زمانی تعیین شده پاسخی ارسال نکرده است.";
                break;
            case "5":
                result = "پارامترهای ارسالی نامعتبر است.";
                break;
            case "8":
                result = "آدرس سرور پذیرنده نامعتبر است )در پرداختهای بر پایه توکن(";
                break;
            case "10":
                result = "توکن ارسال شده یافت نشد.";
                break;
            case "11":
                result = "با این شماره ترمینال فقط تراکنش های توکنی قابل پرداخت هستند.";
                break; 
            case "12":
                result = "شماره ترمینال ارسال شده یافت نشد.";
                break;
        }
        return result;
    }
}