using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Globalization;

namespace Moses.Data.Diagnostics
{
    public class DebuggerWriter : TextWriter
    {
        private bool isOpen;
        private static UnicodeEncoding encoding;
        private readonly int level;
        private readonly string category;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebuggerWriter"/> class.
        /// </summary>
        public DebuggerWriter()
            : this(0, Debugger.DefaultCategory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DebuggerWriter"/> class with the specified level and category.
        /// </summary>
        /// <param name="level">A description of the importance of the messages.</param>
        /// <param name="category">The category of the messages.</param>
        public DebuggerWriter(int level, string category)
            : this(level, category, CultureInfo.CurrentCulture)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DebuggerWriter"/> class with the specified level, category and format provider.
        /// </summary>
        /// <param name="level">A description of the importance of the messages.</param>
        /// <param name="category">The category of the messages.</param>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> object that controls formatting.</param>
        public DebuggerWriter(int level, string category, IFormatProvider formatProvider)
            : base(formatProvider)
        {
            this.level = level;
            this.category = category;
            this.isOpen = true;
        }

        protected override void Dispose(bool disposing)
        {
            isOpen = false;
            base.Dispose(disposing);
        }

        public override void Write(char value)
        {
            if (!isOpen)
            {
                throw new ObjectDisposedException(null);
            }
            Debugger.Log(level, category, value.ToString());
        }

        public override void Write(string value)
        {
            if (!isOpen)
            {
                throw new ObjectDisposedException(null);
            }
            if (value != null)
            {
                Debugger.Log(level, category, value);
            }
        }

        public override void Write(char[] buffer, int index, int count)
        {
            if (!isOpen)
            {
                throw new ObjectDisposedException(null);
            }
            if (buffer == null || index < 0 || count < 0 || buffer.Length - index < count)
            {
                Debugger.Log(0, "0", "Starting Wrtire");
                base.Write(buffer, index, count); // delegate throw exception to base class
                Debugger.Log(0, "0", "Ending Wrtire");
            }
            Debugger.Log(level, category, new string(buffer, index, count));
        }

        public override Encoding Encoding
        {
            get
            {
                if (encoding == null)
                {
                    encoding = new UnicodeEncoding(false, false);
                }
                return encoding;
            }
        }

        public int Level
        {
            get { return level; }
        }

        public string Category
        {
            get { return category; }
        }
    }
}
