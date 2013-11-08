using System;
using System.Globalization;

namespace Derp.Sales.Domain
{
    public class IsoWeek : IEquatable<IsoWeek>, IComparable<IsoWeek>
    {
        private readonly int week;
        private readonly int year;

        private IsoWeek(int year, int week)
        {
            if (week <= 0 || week > 53)
            {
                throw new ArgumentOutOfRangeException("week");
            }
            this.year = year;
            this.week = week;
        }

        #region IComparable<IsoWeek> Members

        public int CompareTo(IsoWeek other)
        {
            return year == other.year
                ? week.CompareTo(other.week)
                : year.CompareTo(other.year);
        }

        #endregion

        #region IEquatable<IsoWeek> Members

        public bool Equals(IsoWeek other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return year == other.year && week == other.week;
        }

        #endregion

        public static bool operator ==(IsoWeek a, IsoWeek b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(IsoWeek a, IsoWeek b)
        {
            return !(a == b);
        }

        public static bool operator >(IsoWeek a, IsoWeek b)
        {
            return a.CompareTo(b) > 0;
        }

        public static bool operator <(IsoWeek a, IsoWeek b)
        {
            return a.CompareTo(b) < 0;
        }

        public static bool operator >=(IsoWeek a, IsoWeek b)
        {
            return a.CompareTo(b) >= 0;
        }

        public static bool operator <=(IsoWeek a, IsoWeek b)
        {
            return a.CompareTo(b) <= 0;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((IsoWeek) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (year*397) ^ week;
            }
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

            var weekOfYear = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                time,
                CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday);
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

        public static IsoWeek FromString(string s)
        {
            var parts = s.Split('-');
            var year = parts[0];
            var week = parts[1].Remove(0, 1);
            return new IsoWeek(Int32.Parse(year), Int32.Parse(week));
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
