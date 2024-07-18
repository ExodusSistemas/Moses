using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moses
{
    /// <summary>
    /// TimeProvider
    /// </summary>
    public class TimeProvider
    {
        private static TimeProvider _current = null;

        /// <summary>
        /// Current
        /// </summary>
        public static TimeProvider Current
        {
            get
            {
                _current ??= new TimeProvider();
                return _current;
            }
        }

        /// <summary>
        /// TimeProvider
        /// </summary>
        public TimeProvider()
        {

        }

        /// <summary>
        /// Now
        /// </summary>
        public virtual DateTime Now
        {
            get
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// Today
        /// </summary>
        public virtual DateTime Today
        {
            get
            {
                return DateTime.Today;
            }
        }

        /// <summary>
        /// MaxValue
        /// </summary>
        public virtual DateTime MaxValue
        {
            get
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// MinValue
        /// </summary>
        public virtual DateTime MinValue
        {
            get
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// SetDate
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public virtual DateTime SetDate(DateTime date)
        {
            return date;
        }
    }
}
