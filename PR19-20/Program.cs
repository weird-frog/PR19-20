using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR19_20
{
    //серверная часть приложения
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Однопоточный сервер запущен");
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 8888);

            Socket sock = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                sock.Bind(ipEndPoint);
                sock.Listen(10);
                while (true)
                {
                    Console.WriteLine($"Слушаем порт {ipEndPoint}");
                    Socket s = sock.Accept();
                    string data = null;

                    byte[] bytes = new byte[1024];
                    int bytesCount = s.Receive(bytes);
                    data += Encoding.UTF8.GetString(bytes, 0, bytesCount);
                    Console.WriteLine("Данные от клиента: " + data + "\n\n");

                    string reply = "Query size: " + data.Length.ToString() + " chars";
                    byte[] msg = Encoding.UTF8.GetBytes(reply);
                    s.Send(msg);

                    if (data.IndexOf("<TheEnd>") > -1)
                    {
                        Console.WriteLine("Соединение завершено.");
                        break;
                    }
                    s.Shutdown(SocketShutdown.Both);
                    s.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}
