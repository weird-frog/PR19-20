using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Communicate("localhost", 8888);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                Console.ReadKey();
            }
        }

        private static void Communicate(string hostname, int port)
        {
            byte[] bytes = new byte[1024];

            IPHostEntry ipHost = Dns.GetHostEntry(hostname);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

            Socket sock = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            sock.Connect(ipEndPoint);
            Console.Write("Введите сообщение: ");
            string message = Console.ReadLine();
            Console.WriteLine($"Подключаемся к порту {sock.RemoteEndPoint.ToString()}");

            byte[] data = Encoding.UTF8.GetBytes(message);

            int bytesSent = sock.Send(data);
            int bytesRec = sock.Receive(bytes);

            Console.WriteLine($"\nОтвет от сервера: {Encoding.UTF8.GetString(bytes, 0, bytesRec)} \n\n");

            if (message.IndexOf("<TheEnd>") == -1)
            {
                Communicate(hostname, port);
            }
            sock.Shutdown(SocketShutdown.Both);
            sock.Close();
        }
    }
}
