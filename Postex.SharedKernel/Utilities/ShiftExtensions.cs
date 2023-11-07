namespace Postex.SharedKernel.Utilities
{
    public static class ShiftExtensions
    {
        public static int ToShift(this DateTime dateTime)
        {
            if (dateTime.TimeOfDay >= new TimeSpan(9, 00, 00) && dateTime.TimeOfDay <= new TimeSpan(12, 00, 00))
            {
                return 1;
            }
            if (dateTime.TimeOfDay >= new TimeSpan(14, 00, 00) && dateTime.TimeOfDay <= new TimeSpan(17, 00, 00))
            {
                return 2;
            }
            if (dateTime.TimeOfDay >= new TimeSpan(18, 00, 00) && dateTime.TimeOfDay <= new TimeSpan(21, 00, 00))
            {
                return 3;
            }
            return 1;
        }
    }
}
