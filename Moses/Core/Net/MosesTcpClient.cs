using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Moses.Net
{
    public class MosesTcpClient
    {
        public static TcpClient client;
        public static NetworkStream stream;
        public static StreamWriter writer;

        public static void Start(string host, int port)
        {
            client = new TcpClient(host,port);
            stream = new NetworkStream(client.Client);
            writer = new StreamWriter(stream);
            new Thread(new ThreadStart(WaitMessage)).Start();
        }

        public static void WriteServer(string input)
        {
            if (client.Connected)
            {
                writer.WriteLine(input);
                writer.Flush();
            }
        }

        public static void WaitMessage()
        {
            while (true)
            {
                if (client.Connected)
                {

                    stream = client.GetStream();
                    StreamReader reader = new StreamReader(stream);

                    Console.WriteLine("Connected.");
                    while (client.Connected)
                    {
                        string received;
                        received = reader.ReadLine();
                        while (null != received)
                        {
                            Console.WriteLine("Server Said:");
                            Console.WriteLine(received);
                            Console.WriteLine();
                            received = null;
                        }

                    }

                }
            }
        }

    }
}
