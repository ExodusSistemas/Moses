using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;

namespace Moses.Data.Parser.Flat
{
    public class FlatFileReaderEngine
    {
        protected IFileReader _reader;

        public FlatFileReaderEngine(IFileReader fileReader)
        {
            _reader = fileReader;
        }
    }

}
