using System;
using System.Globalization;

namespace Derp.Sales.Domain
{
    public class IsoWeek
    {
        private readonly int year;
        private readonly int week;

        private IsoWeek(int year, int week)
        {
            this.year = year;
            this.week = week;
        }

        // This presumes that weeks start with Monday.
        // Week 1 is the 1st week of the year with a Thursday in it.
        private static Tuple<int, int> GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            
            var year = time.Year;

            var weekOfYear = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            if (weekOfYear == 53 && (day > DayOfWeek.Thursday || day == DayOfWeek.Sunday))
            {
                year--;
            }
            return Tuple.Create(year, weekOfYear);
        } 

        public static IsoWeek FromDate(DateTime dateTime)
        {
            var week = GetIso8601WeekOfYear(dateTime);
            return new IsoWeek(week.Item1, week.Item2);
        }


        public static implicit operator string(IsoWeek isoWeek)
        {
            return isoWeek.ToString();
        }

        public override string ToString()
        {
            return String.Format("{0:D4}-W{1:D2}", year, week);
        }
    }
}
