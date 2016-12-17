namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Runtime.CompilerServices;

    public class AppearanceSettings
    {
        public AppearanceSettings()
        {
            bool flag;
            bool flag2;
            bool flag3;
            this.ShowFooter = flag = false;
            this.RightToLeft = flag2 = flag;
            this.HighlightRowsOnHover = flag3 = flag2;
            this.ShowRowNumbers = this.AlternateRowBackground = flag3;
            this.RowNumbersColumnWidth = 0x19;
            this.Caption = "";
            this.ScrollBarOffset = 0x12;
            this.ShrinkToFit = true;
        }

        public bool AlternateRowBackground { get; set; }

        public string Caption { get; set; }

        public bool HighlightRowsOnHover { get; set; }

        public bool RightToLeft { get; set; }

        public int RowNumbersColumnWidth { get; set; }

        public int ScrollBarOffset { get; set; }

        public bool ShowFooter { get; set; }

        public bool ShowRowNumbers { get; set; }

        public bool ShrinkToFit { get; set; }
    }
}

