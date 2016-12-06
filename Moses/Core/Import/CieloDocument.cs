using System;
using System.Collections.Generic;

namespace Moses.Import
{
    public class CieloDocument
    {
        public DateTime StatementStart { get; set; }

        public DateTime StatementEnd { get; set; }

        public List<CieloTransaction> Transactions { get; set; }
    }
}