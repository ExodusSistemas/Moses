using System;
using System.Collections.Generic;
using System.Web.Management;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace Moses.Web.Management
{
    public class MosesWebEventProvider : WebEventProvider
    {
        //private string _providerName = "MosesWebEventProvider";
        private string _path = string.Empty;
        private static XmlSerializer serializer = new XmlSerializer(typeof(WebBaseEvent));

        public override void Initialize(string name,
            System.Collections.Specialized.NameValueCollection config)
        {
            if (string.IsNullOrEmpty(config["path"]))
                throw new ArgumentException("Caminho inválido/inexistente.");

            this._path = config["path"];
            base.Initialize(name, config);
        }

        /// <summary>
        /// Escreve no XML o evento disparado pela aplicação
        /// </summary>
        /// <param name="raisedEvent"></param>
        public override void ProcessEvent(WebBaseEvent raisedEvent)
        {
            //trabalhando nisso (Neto)
            
            //if (raisedEvent is CodeEvent)
            //{
            //    XmlDocument document = new XmlDocument();
            //    Stream stream = null;
            //    try
            //    {
            //        stream = File.OpenWrite(_path);
            //        document.Load(stream);

            //        XmlElement eventElement = document["Events"].AppendChild(document.CreateElement("Event")) as XmlElement;

            //        XmlAttribute aCode = eventElement.Attributes.Append(document.CreateAttribute("Code"));
            //        aCode.Value = raisedEvent.EventCode.ToString();

            //        XmlAttribute aId = eventElement.Attributes.Append(document.CreateAttribute("ID"));
            //        aId.Value = raisedEvent.EventID.ToString();

            //        XmlAttribute aTime = eventElement.Attributes.Append(document.CreateAttribute("EventTime"));
            //        aTime.Value = raisedEvent.EventTime.ToString();

            //        eventElement.InnerText = raisedEvent.Message.ToString();

            //        document.Save(stream);
            //    }
            //    finally
            //    {
            //        if (stream != null)
            //        {
            //            stream.Close();
            //        }
            //    }
                
            //}
        }

        public override void Flush(){}

        public override void Shutdown(){}
    }
}
