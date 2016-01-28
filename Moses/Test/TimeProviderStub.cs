using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moses.Test
{
    public class TimeProviderStub : TimeProvider
    {
        private DateTime _date;
        public override DateTime SetDate(DateTime date)
        {
            return _date = date;
        }

        public override DateTime Now
        {
            get
            {
                return _date;
            }
        }

        public override DateTime Today
        {
            get
            {
                return _date.Date;
            }
        }

        public override DateTime MaxValue
        {
            get
            {
                return DateTime.MaxValue;
            }
        }

        public override DateTime MinValue
        {
            get
            {
                return DateTime.MinValue;
            }
        }
    }
}
