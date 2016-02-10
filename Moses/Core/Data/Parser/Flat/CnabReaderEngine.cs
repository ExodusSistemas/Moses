using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;

namespace Moses.Data.Parser.Flat
{
    public class CnabReaderEngine : FlatFileReaderEngine
    {
        public CnabReaderEngine (IFileReader reader) : base(reader)
	    {
	    }

        public virtual List<ExpandoObject> Read(Stream stream)
        {
            try
             {
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                string line = "";

                List<ExpandoObject> result = new List<ExpandoObject>();

                while ((line = reader.ReadLine()) != null)
                {
                    if (!String.IsNullOrEmpty(line))
                    {
                        var segmentType = line.Substring(7, 1);
                        var lineResult = ReadSegment(line); 
                        result.Add(lineResult);
                    }
                }
                reader.Close();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler arquivo.", ex);
            }
        }

        private ExpandoObject ReadSegment(string line)
        {
            ExpandoObject lineResult = new ExpandoObject();
            var expandoDic = ((IDictionary<string, object>)lineResult);

            //Pega o lote que está sendo lido
 	        IFileReaderSection sectionList = _reader.GetSection(line.Substring(13, 1));
            
            foreach( var f in sectionList.Fields){
                expandoDic[f.Reference] = Extract(line, f);
            }
            
            return lineResult;
        }

        private string Extract(string line, IFileReaderField field){
            return line.Substring(field.Position, field.Length);
        }

    }
}
