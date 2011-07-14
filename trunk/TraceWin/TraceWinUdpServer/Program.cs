using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TraceWinUdpServer
{
    class Program
    {
        private static int port = -1;
        static void Main(string[] args)
        {

            try { 
                
                port = Int32.Parse(args[0]);
                runServer();
            
            }
            catch (Exception e) {
                Console.WriteLine("ERROR = Invalid PORT: " + e.Message);
                FileHelper.WriteLog("ERROR = Invalid PORT: "+e.Message);
            }
              
        }

        public static void runServer() {
            int recv;
            byte[] data = new byte[1024];
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);

            Socket newsock = new Socket(AddressFamily.InterNetwork,
                            SocketType.Dgram, ProtocolType.Udp);

            newsock.Bind(ipep);
            Console.WriteLine("Listening UDP on port " + port + ".");
            Console.WriteLine("Waiting for a TraceWin clients...");

            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint tmpRemote = (EndPoint)(sender);

            recv = newsock.ReceiveFrom(data, ref tmpRemote);

            Console.WriteLine("Message received from {0}:", tmpRemote.ToString());
            Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));

            while (true)
            {
                data = new byte[1024];
                recv = newsock.ReceiveFrom(data, ref tmpRemote);
                string remoteMsg = Encoding.ASCII.GetString(data, 0, recv);
                Console.WriteLine(remoteMsg);
                FileHelper.WriteLog(remoteMsg);
            }
            //newsock.Close();
        
        }
    }
}
