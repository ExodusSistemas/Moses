//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Net.Sockets;
//using System.IO;
//using System.Threading;
//using System.Net;
//using System.Xml.Serialization;
//using Moses.Management;

//namespace Moses.Net
//{
//    public class MosesTcpServer
//    {
//        static TcpListener listener;
//        static TcpClient client  = new TcpClient();
//        static NetworkStream nstream;

//        public static void Start(string ipAddress,int port)
//        {
//            if (!client.Connected){
//                listener = new TcpListener(IPAddress.Parse(ipAddress), port);
//                listener.Start();
//                new Thread(new ThreadStart(WaitConnection)).Start();
//            }
//        }

//        public static void BroadcastDebugSql(object commandText)
//        {
//            try
//            {
//                //TcpClient client = new TcpClient();
//                //client.SendBufferSize = 300;

//                //string[] a = Configuration.Default.DebugServerAddress.Split(':');
//                //client.Connect(a[0].ToString(), Int32.Parse(a[1]));//servidor e porta

//                //SqlDebugPacket packet = new SqlDebugPacket();
//                //packet.PacketSendTime = DateTime.Now;
//                //packet.SqlCommandText = (string)commandText;

//                //new XmlSerializer(typeof(SqlDebugPacket)).Serialize(client.GetStream(), packet);

//                //client.Close();
//            }
//            catch
//            {

//            }
//        }

//        public static void WriteClient(string input)
//        {
//            if (client.Connected && (input != null))
//            {



//                StreamWriter writer = new StreamWriter(nstream);
//                writer.WriteLine(input);
//                writer.Flush();
//            }
//        }

//        private void Listen()
//        {
//            while (true)
//            {
//                client = listener.AcceptTcpClient();

//                packet = (SqlDebugPacket)new XmlSerializer(typeof(Moses.Data.Diagnostics.SqlDebugPacket)).Deserialize(new StreamReader(client.GetStream()));

//                e.DebugPacket = packet;

//                this.Invoke(new ThreadStart(DispatchEvent));
//                client.Close();
//            }
//        }

//        public void DispatchEvent(MosesEventArgs e)
//        {
//            onMosesEvent(this,e);
//        }

//        public event MosesEventHandler onMosesEvent;

//        public delegate void MosesEventHandler(object sender, MosesEventArgs e);

//        public static void WaitConnection()
//        {

//            while (true)
//            {
//                client = listener.AcceptTcpClient();
//                if (client.Connected)
//                {

//                    MosesEvent mEvent = (MosesEvent)new XmlSerializer(typeof(MosesEvent)).Deserialize(new StreamReader(client.GetStream()));
                    
//                    //StreamReader reader = new StreamReader(nstream);
//                    this.DispatchEvent(mEvent.Args);
//                }
//            }
//        }
//    }        
//}
