using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Xml.Serialization;
using System.IO;
using System.Net;
using System.Xml;

namespace Moses.Management
{
    public partial class MosesEventServer : Control
    {
        static TcpListener listener;
        static TcpClient client = new TcpClient();

        public void Start()
        {
            Start("127.0.0.1", 3450);
        }

        public void Start(string ipAddress, int port)
        {
            if (!client.Connected)
            {
                listener = new TcpListener(IPAddress.Parse(ipAddress), port);
                listener.Start();
                new Thread(new ThreadStart(WaitConnection)).Start();
            }
        }

        public event MosesEventHandler onMosesEvent;

        public delegate void MosesEventHandler(object sender, MosesEventArgs e);

        public void DispatchEvent(object e)
        {
            onMosesEvent(this, e as MosesEventArgs);
        }

        public void WaitConnection()
        {

            while (true)
            {
                client = listener.AcceptTcpClient();
                
                //recebe os eventos
                XmlSerializer serializer = new XmlSerializer(typeof(MosesEventArgs));
                XmlReader reader = new XmlTextReader(client.GetStream());

                if (serializer.CanDeserialize(reader))
                {
                    MosesEventArgs mEventArg = (MosesEventArgs)serializer.Deserialize(reader);
                    this.Invoke(new ParameterizedThreadStart(DispatchEvent),mEventArg) ;
                }
            }
        }

    }
}
