//#if TRACE

//namespace Moses.Data.Diagnostics
//{
//    using System;
//    using System.Net.Sockets;
//    using System.IO;
//    using System.Xml.Serialization;
//    using System.Threading;
//    using System.Net;
//    using System.Windows.Forms;

//    public class SqlMonitor  : Control 
//    {
//        TcpListener listener = null;
//        TcpClient client = null;
//        Thread thread = null;

//        protected SqlDebugPacket packet;
//        protected volatile SqlMonitorEventArgs e  = new SqlMonitorEventArgs();

//        public SqlMonitor()
//        {
//            thread = new Thread(new ThreadStart(Listen));
//            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 3450);
//            listener.Start();

//            thread.Start();
//        }

//        public void Close()
//        {
//             this.thread.Abort();
//             listener.Stop();
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
        
//        public void DispatchEvent(){
//            onBroadcastListened(this,new SqlMonitorEventArgs(e) );
//        }

//        public event SqlMonitorEventHandler onBroadcastListened;

//        public delegate void SqlMonitorEventHandler(object sender, SqlMonitorEventArgs e);

//        public delegate void SqlDumpHandler(string command);

//        public static void DumpSql(string commandText)
//        {
//            Thread t = new Thread(new ParameterizedThreadStart(BroadcastDebugSql));
//            t.Start(commandText);
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
    
//    }

//    public class SqlMonitorEventArgs : System.EventArgs
//    {
//        private SqlDebugPacket _packet;

//        public SqlDebugPacket DebugPacket
//        {
//            get { return _packet; }
//            set { _packet = value; }
//        }
        
//        public SqlMonitorEventArgs()
//        {
//        }

//        public SqlMonitorEventArgs(SqlMonitorEventArgs copy)
//        {
//            this._packet.PacketSendTime = copy._packet.PacketSendTime;
//            this._packet.SqlCommandText = String.Copy(copy._packet.SqlCommandText);
//        }
	
//    }
//}

//#endif