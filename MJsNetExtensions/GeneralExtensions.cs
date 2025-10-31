namespace MJsNetExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;


    /// <summary>
    /// Summary description for Extensions
    /// </summary>
    public static class GeneralExtensions
    {
        #region Time, Date, TimeSpan
        /// <summary>
        /// See https://stackoverflow.com/questions/152774/is-there-a-better-way-to-trim-a-datetime-to-a-specific-precision
        /// Usage, e.g.:
        /// DateTime.Now.Trim(TimeSpan.TicksPerDay),
        /// DateTime.Now.Trim(TimeSpan.TicksPerHour),
        /// DateTime.Now.Trim(TimeSpan.TicksPerMillisecond),
        /// DateTime.Now.Trim(TimeSpan.TicksPerMinute),
        /// DateTime.Now.Trim(TimeSpan.TicksPerSecond)
        /// </summary>
        /// <param name="date"></param>
        /// <param name="roundTicks"></param>
        /// <returns></returns>
        public static DateTime Trim(this DateTime date, long roundTicks)
        {
            return new DateTime(date.Ticks - date.Ticks % roundTicks, date.Kind);
        }

        /// <summary>
        /// Truncate To Milliseconds - useful for printing time in log, etc.
        /// </summary>
        /// <param name="timespan">The <see cref="TimeSpan"/> to truncate.</param>
        /// <returns></returns>
        public static TimeSpan TruncateToMilliseconds(this TimeSpan timespan)
        {
            return new TimeSpan(timespan.Ticks - timespan.Ticks % TimeSpan.FromMilliseconds(1).Ticks);
        }

        /// <summary>
        /// Truncate To Seconds - useful for printing time in log, etc.
        /// </summary>
        /// <param name="timespan">The <see cref="TimeSpan"/> to truncate.</param>
        /// <returns></returns>
        public static TimeSpan TruncateToSeconds(this TimeSpan timespan)
        {
            return new TimeSpan(timespan.Ticks - timespan.Ticks % TimeSpan.FromSeconds(1).Ticks);
        }

        /// <summary>
        /// Truncate To Minutes - useful for printing time in log, etc.
        /// </summary>
        /// <param name="timespan">The <see cref="TimeSpan"/> to truncate.</param>
        /// <returns></returns>
        public static TimeSpan TruncateToMinutes(this TimeSpan timespan)
        {
            return new TimeSpan(timespan.Ticks - timespan.Ticks % TimeSpan.FromMinutes(1).Ticks);
        }

        /// <summary>
        /// Truncate To Hours - useful for printing time in log, etc.
        /// </summary>
        /// <param name="timespan">The <see cref="TimeSpan"/> to truncate.</param>
        /// <returns></returns>
        public static TimeSpan TruncateToHours(this TimeSpan timespan)
        {
            return new TimeSpan(timespan.Ticks - timespan.Ticks % TimeSpan.FromHours(1).Ticks);
        }

        /// <summary>
        /// Truncate To Days - useful for printing time in log, etc.
        /// </summary>
        /// <param name="timespan">The <see cref="TimeSpan"/> to truncate.</param>
        /// <returns></returns>
        public static TimeSpan TruncateToDays(this TimeSpan timespan)
        {
            return new TimeSpan(timespan.Ticks - timespan.Ticks % TimeSpan.FromDays(1).Ticks);
        }
        #endregion Time, Date, TimeSpan

        #region Comparing
        /// <summary>
        /// Returns the maximal value of its both parameters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x">The first parameter</param>
        /// <param name="y">The secnd parameter</param>
        /// <returns>The maximal value of its both parameters</returns>
        public static T Max<T>(T x, T y)
        {
            return (Comparer<T>.Default.Compare(x, y) > 0) ? x : y;
        }

        /// <summary>
        /// Returns the minimal value of its both parameters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x">The first parameter</param>
        /// <param name="y">The secnd parameter</param>
        /// <returns>The minimal value of its both parameters</returns>
        public static T Min<T>(T x, T y)
        {
            return (Comparer<T>.Default.Compare(x, y) > 0) ? y : x;
        }
        #endregion Comparing
    }
}
