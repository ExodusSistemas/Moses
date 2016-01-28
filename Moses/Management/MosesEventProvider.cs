using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;
using System.Net.Sockets;
using System.Collections.Specialized;
using System.Xml.Serialization;
using Moses.Net;
using Moses;
using System.Threading;

namespace Moses.Management
{
    public class MosesEventProvider : ProviderBase
    {
        private bool _isSet;
        private string _type;
        public static MosesEventProvider Default;
        private bool _canConnect;

        public static void Initialize(EventProviderConfiguration config)
        {
            Default = new MosesEventProvider();
            Default._isSet = true;
            Default._type = config.Type;
            Default._port = Int32.Parse(config.Port);
            Default._server = config.Server;
            Default._canConnect = true;
            Default.ConnectionTest(); //verificar quando forem implementados outros providers
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config.Count == 0)
            {
                _isSet = false;
                return;
            }


            //ainda não sei o que fazer poraqui, mas esse deve ser o padrão [Neto]
            //o path pode vir a ser opcional
            if (string.IsNullOrEmpty(config["path"]) || string.IsNullOrEmpty(config["server"]) ||
                string.IsNullOrEmpty("port") )
                throw new ArgumentException("Argumentos Insuficientes para o MosesEventProvider. Necessários" +
                    "\"path\",\"port\",\"server\"e \"type\"");


            
            //O provider deve possuir algumas possibilidades de saída.
            //saída para arquivo
            //saída para o Console do Manager
            //saída para e-mail
            //nenhuma saída (padrão)

            //arquiteturalmente no .net, o padrão é para cada tipo de saída ter um provider.
            //No caso do moses, isso não foge completamente do escopo, já que as saídas, na verdade,
            //serão processadas pelo moses

            //this._path = config["path"];

            _isSet = true;
            _type = config["type"];
            _port = Int32.Parse(config["port"]);
            _server = config["server"];

            //Testa a conexão
            //Rotina precisa ser feita se for o manager
            if (_type == "Manager")
            {
                ConnectionTest();
            }
        }

        private void ConnectionTest()
        {
            try
            {
                this.BeginProcessEvent(new MosesEvent("MosesEventProvider:ConnectionTest", new MosesEventArgs("Test", MosesEventType.ConnectionTest)));
            }
            catch
            {
                
            }
        }

        public bool IsSet
        {
            get
            {
                return _isSet;
            }
        }

        public override string Description
        {
            get
            {
                return "Notificador de Eventos do Moses";
            }
        }

        public override string Name
        {
            get
            {
                return "MosesEventProvider";
            }
        }

        public string Type
        {
            get
            {
                return _type;
            }
        }

        private string _server;

        public string Server
        {
            get { return _server; }
            set { _server = value; }
        }

        private int _port;

        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        private TcpClient _client = new TcpClient();

        public TcpClient Client
        {
            get 
            {
                if (_client.Connected == false)
                {
                    _client = new TcpClient();
                    _client.SendBufferSize = 50;
                    _client.SendTimeout = 10;
                }
                return _client;
            
            }
            set { _client = value; }
        }

        private Thread _eventThread;

        public Thread EventThread
        {
            get {
                if (_eventThread == null)
                {
                    //_eventThread = new Thread(
                }
                return _eventThread; 
            }
            set { 
                _eventThread = value; 
            }
        }

        /// <summary>
        /// Método de Processamento do Evento
        /// </summary>
        /// <param name="raisedEvent">Evento a ser disparado</param>
        public void ProcessEvent(object raisedEvent)
        {
            MosesEvent mEvent = raisedEvent as MosesEvent;
            if (mEvent != null && this._isSet)
            {
                switch (this.Type)
                {
                    case "Manager":
                        {
                            if (_canConnect)
                            {
                                try
                                {
                                    Client.Connect(Server, Port);//servidor e porta

                                    if (this.Client.Connected)
                                    {
                                        new XmlSerializer(typeof(MosesEventArgs)).Serialize(this.Client.GetStream(), mEvent.Args);
                                    }
                                }
                                catch (System.Exception e)
                                {
                                    //Se não for um teste, dispara o erro
                                    if (mEvent.Args.EventType != MosesEventType.ConnectionTest)
                                    {
                                        throw new MosesMaintenanceException("Falha no envio do MosesEvent", e);
                                    }
                                    else //caso seja um teste, desativa a flag
                                    {
                                        _canConnect = false;
                                    }
                                }
                                finally
                                {
                                    this.Client.Close();
                                }
                            }
                        }
                        break;
                }
            }
        }

        private WaitCallback _mosesEventCallback;

        public WaitCallback MosesEventCallback
        {
            get
            {
                if ( _mosesEventCallback == null)
                {
                    _mosesEventCallback = new WaitCallback(ProcessEvent);
                }
                return _mosesEventCallback;
            }
        }

        internal void BeginProcessEvent(MosesEvent mosesEvent)
        {
            lock (mosesEvent)
            {
                ThreadPool.QueueUserWorkItem(MosesEventCallback, mosesEvent);
            }
        }
    }
}
