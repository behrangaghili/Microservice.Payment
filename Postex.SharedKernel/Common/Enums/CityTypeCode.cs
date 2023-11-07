using System.ComponentModel;

namespace Postex.SharedKernel.Common.Enums
{
    public enum CityTypeCode
    {
        [Description("None")] None = 0,
        [Description("تهران")] Tehran = 1,
        [Description("کلانشهر")] G8 = 2,
        [Description("مرکز استان")] StateCenter = 3,
        [Description("شهرستان ها")] SmallCities = 4
    }
}