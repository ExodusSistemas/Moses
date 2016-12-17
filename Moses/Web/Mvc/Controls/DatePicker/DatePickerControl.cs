namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class DatePickerControl
    {
        public DatePickerControl()
        {
            this.Enabled = true;
            this.AltField = "";
            this.AltFormat = "";
            this.AppendText = "";
            this.AutoSize = false;
            this.ButtonImage = "";
            this.ButtonImageOnly = false;
            this.ButtonText = "...";
            this.ChangeMonth = false;
            this.ChangeYear = false;
            this.CloseText = "Done";
            this.ConstrainInput = true;
            this.CurrentText = "Today";
            this.DateFormat = "yyyy/MM/dd";
            this.DayNames = new List<string>();
            this.DayNamesMin = new List<string>();
            this.DayNamesShort = new List<string>();
            this.DisplayMode = Moses.Web.Mvc.Controls.DatePickerDisplayMode.Standalone;
            this.DefaultDate = null;
            this.Duration = 500;
            this.FirstDay = 0;
            this.GotoCurrent = false;
            this.HideIfNoPrevNext = false;
            this.IsRTL = false;
            this.MaxDate = null;
            this.MinDate = null;
            this.MonthNames = new List<string>();
            this.MonthNamesShort = new List<string>();
            this.NavigationAsDateFormat = false;
            this.NextText = "Next";
            this.PrevText = "Prev";
            this.ShowButtonPanel = false;
            this.ShowMonthAfterYear = false;
            this.ShowOn = Moses.Web.Mvc.Controls.ShowOn.Both;
        }

        public string AltField { get; set; }

        public string AltFormat { get; set; }

        public string AppendText { get; set; }

        public bool AutoSize { get; set; }

        public string ButtonImage { get; set; }

        public bool ButtonImageOnly { get; set; }

        public string ButtonText { get; set; }

        public bool ChangeMonth { get; set; }

        public bool ChangeYear { get; set; }

        public string CloseText { get; set; }

        public bool ConstrainInput { get; set; }

        public string CurrentText { get; set; }

        public string DateFormat { get; set; }

        public List<string> DayNames { get; set; }

        public List<string> DayNamesMin { get; set; }

        public List<string> DayNamesShort { get; set; }

        public DateTime? DefaultDate { get; set; }

        public Moses.Web.Mvc.Controls.DatePickerDisplayMode DisplayMode { get; set; }

        public int Duration { get; set; }

        public bool Enabled { get; set; }

        public int FirstDay { get; set; }

        public bool GotoCurrent { get; set; }

        public bool HideIfNoPrevNext { get; set; }

        public string ID { get; set; }

        public bool IsRTL { get; set; }

        public DateTime? MaxDate { get; set; }

        public DateTime? MinDate { get; set; }

        public List<string> MonthNames { get; set; }

        public List<string> MonthNamesShort { get; set; }

        public bool NavigationAsDateFormat { get; set; }

        public string NextText { get; set; }

        public string PrevText { get; set; }

        public bool ShowButtonPanel { get; set; }

        public bool ShowMonthAfterYear { get; set; }

        public Moses.Web.Mvc.Controls.ShowOn ShowOn { get; set; }
    }
}

