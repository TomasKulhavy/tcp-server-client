using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCP_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress server = IPAddress.Parse("127.0.0.1");
            TcpClientService.Connect(server.ToString(), "Connected");
            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }
    }
}
