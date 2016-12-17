namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Web.Script.Serialization;

    internal class DatePickerRenderer
    {
        private string _dateFormat = "";
        private string _dateSeparator = "";
        private Moses.Web.Mvc.Controls.DatePickerControl _model;

        public DatePickerRenderer(Moses.Web.Mvc.Controls.DatePickerControl datePicker)
        {
            this._model = datePicker;
            this._dateSeparator = Thread.CurrentThread.CurrentCulture.DateTimeFormat.DateSeparator;
            this._model.DateFormat = datePicker.DateFormat.Replace("/", this._dateSeparator);
            this._dateFormat = "{0:" + datePicker.DateFormat + "}";
        }

        private string ConvertDotNetDateFormatToJQuery(string dateFormat)
        {
            dateFormat = dateFormat.Replace("yy", "y");
            if (dateFormat.IndexOf("MMMM") > 0)
            {
                dateFormat = dateFormat.Replace("MMMM", "MM");
            }
            else if (dateFormat.IndexOf("MMM") > 0)
            {
                dateFormat = dateFormat.Replace("MMM", "M");
            }
            else if (dateFormat.IndexOf("MM") > 0)
            {
                dateFormat = dateFormat.Replace("MM", "mm");
            }
            else if (dateFormat.IndexOf("M") > 0)
            {
                dateFormat = dateFormat.Replace("M", "n");
            }
            if (dateFormat.IndexOf("DDDD") > 0)
            {
                dateFormat = dateFormat.Replace("DDDD", "DD");
                return dateFormat;
            }
            if (dateFormat.IndexOf("DDD") > 0)
            {
                dateFormat = dateFormat.Replace("DDD", "D");
                return dateFormat;
            }
            if (dateFormat.IndexOf("DD") > 0)
            {
                dateFormat = dateFormat.Replace("DD", "dd");
                return dateFormat;
            }
            if (dateFormat.IndexOf("D") > 0)
            {
                dateFormat = dateFormat.Replace("D", "d");
            }
            return dateFormat;
        }

        private string FormatDateToJavascript(DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                int num2 = dateTime.Value.Month - 1;
                return $"new Date({dateTime.Value.Year.ToString()},{num2.ToString()},{dateTime.Value.Day.ToString()})";
            }
            return "";
        }

        private string GetControlEditorJavascript() => 
            ("<style type='text/css'>body .ui-datepicker { z-index: 2000 }</style>" + $"<script type='text/javascript'>var {this._model.ID}_dpid = {{ {this.GetStartupOptions()} }};</script>");

        private string GetStandaloneJavascript()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("<input type='text' id='{0}' name='{0}' />", this._model.ID);
            builder.AppendFormat("<style type='text/css'>body .ui-datepicker { z-index: 2000 }</style>", new object[0]);
            builder.Append("<script type='text/javascript'>\n");
            builder.Append("jQuery(document).ready(function() {");
            builder.AppendFormat("jQuery('#{0}').datepicker({{", this._model.ID);
            builder.Append(this.GetStartupOptions());
            builder.Append("});");
            builder.Append("});");
            builder.Append("</script>");
            return builder.ToString();
        }

        private string GetStartupOptions()
        {
            IJsonService serializer = Configuration.Json;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("id: '{0}'", this._model.ID);
            sb.AppendFormatIfNotNullOrEmpty(this._model.AltField, ",altField: '{0}'");
            sb.AppendFormatIfNotNullOrEmpty(this._model.AltFormat, ",altFormat: '{0}'");
            sb.AppendFormatIfNotNullOrEmpty(this._model.AppendText, ",appendText: '{0}'");
            sb.AppendIfTrue(this._model.AutoSize, ",autoSize: true");
            sb.AppendFormatIfNotNullOrEmpty(this._model.ButtonImage, ",buttonImage: '{0}'");
            sb.AppendIfTrue(this._model.ButtonImageOnly, ",buttonImageOnly: true");
            sb.AppendFormatIfTrue(this._model.ButtonText != "...", ",buttonText: '{0}'", new object[] { this._model.ButtonText });
            sb.AppendIfTrue(this._model.ChangeMonth, ",changeMonth: true");
            sb.AppendIfTrue(this._model.ChangeYear, ",changeYear: true");
            sb.AppendFormatIfTrue(this._model.CloseText != "Done", ",closeText: '{0}'", new object[] { this._model.CloseText });
            sb.AppendIfFalse(this._model.ConstrainInput, ",constrainInput: false");
            sb.AppendFormatIfTrue(this._model.CurrentText != "Today", ",currentText: '{0}'", new object[] { this._model.CurrentText });
            sb.AppendFormat(",dateFormat: '{0}'", this.ConvertDotNetDateFormatToJQuery(this._model.DateFormat));
            sb.AppendFormatIfTrue(this._model.DayNames.Count > 0, ",dayNames: {0}", new object[] { serializer.Serialize(this._model.DayNames) });
            sb.AppendFormatIfTrue(this._model.DayNamesMin.Count > 0, ",dayNamesMin: {0}", new object[] { serializer.Serialize(this._model.DayNamesMin) });
            sb.AppendFormatIfTrue(this._model.DayNamesShort.Count > 0, ",dayNamesShort: {0}", new object[] { serializer.Serialize(this._model.DayNamesShort) });
            sb.AppendFormatIfTrue(this._model.DefaultDate.HasValue, ",defaultDate: {0}", new object[] { this.FormatDateToJavascript(this._model.DefaultDate) });
            sb.AppendFormatIfTrue(this._model.Duration != 500, ",duration: {0}", new object[] { this._model.Duration.ToString() });
            sb.AppendIfFalse(this._model.Enabled, ",disabled: true");
            sb.AppendFormatIfTrue(this._model.FirstDay != 0, ",firstDay: {0}", new object[] { this._model.FirstDay.ToString() });
            sb.AppendIfTrue(this._model.GotoCurrent, ",gotoCurrent: true");
            sb.AppendIfTrue(this._model.HideIfNoPrevNext, ",hideIfNoPrevNext: true");
            sb.AppendIfTrue(this._model.IsRTL, "isRTL: true");
            sb.AppendFormatIfNotNull(this._model.MaxDate, ",minDate: {0}", new object[] { this.FormatDateToJavascript(this._model.MinDate) });
            sb.AppendFormatIfNotNull(this._model.MinDate, ",maxDate: {0}", new object[] { this.FormatDateToJavascript(this._model.MaxDate) });
            sb.AppendFormatIfTrue(this._model.MonthNames.Count > 0, ",monthNames: {0}", new object[] { serializer.Serialize(this._model.MonthNames) });
            sb.AppendFormatIfTrue(this._model.MonthNamesShort.Count > 0, ",monthNamesShort: {0}", new object[] { serializer.Serialize(this._model.MonthNamesShort) });
            sb.AppendIfTrue(this._model.NavigationAsDateFormat, ",navigationAsDateFormat: true");
            sb.AppendFormatIfTrue(this._model.NextText != "Next", ",nextText: '{0}'", new object[] { this._model.NextText });
            sb.AppendFormatIfTrue(this._model.PrevText != "Prev", ",prevText: '{0}'", new object[] { this._model.PrevText });
            sb.AppendIfTrue(this._model.ShowButtonPanel, ",showButtonPanel: true");
            sb.AppendIfTrue(this._model.ShowMonthAfterYear, ",showMonthAfterYear: true");
            sb.AppendFormat(",showOn: '{0}'", this._model.ShowOn.ToString().ToLower());
            return sb.ToString();
        }

        public string RenderHtml()
        {
            Moses.Web.Mvc.Controls.Guard.IsNotNullOrEmpty(this._model.ID, "ID", "You need to set ID for this JQDatePicker instance.");
            if (this._model.DisplayMode == Moses.Web.Mvc.Controls.DatePickerDisplayMode.Standalone)
            {
                return this.GetStandaloneJavascript();
            }
            return this.GetControlEditorJavascript();
        }
    }
}

