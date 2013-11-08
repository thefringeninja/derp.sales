using System;

namespace Derp.Sales
{
    public static class SystemTime
    {
        public static Func<DateTime> Clock = () => DateTime.UtcNow;

        public static DateTime Now
        {
            get { return Clock(); }
        }
    }
}