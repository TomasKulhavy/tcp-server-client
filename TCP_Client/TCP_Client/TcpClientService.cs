using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace TCP_Client
{
    class TcpClientService
    {
        public static void Connect(String server, String message)
        {
            try
            {
                Int32 port = 13000;
                TcpClient client = new TcpClient(server, port);
                Byte[] bytes = new Byte[256];

                NetworkStream stream = client.GetStream();

                int i;
                stream.Write(Encoding.ASCII.GetBytes(message));
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    Console.Clear();
                    string responseData = Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine(responseData);
                    if (responseData == "Vyhrál jste!" || responseData == "Prohrál jste!")
                    {
                        Console.WriteLine(responseData);
                        Console.WriteLine("\nStiskni Enter pro pokračování...");
                        Console.Read();
                    }
                    else
                    {
                        Console.Write("Zvolte znak: ");
                        string choice = Console.ReadLine();
                        Byte[] choiceBytes = Encoding.ASCII.GetBytes(choice);
                        stream.Write(choiceBytes);
                    }
                }

                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\nStiskni Enter pro pokračování...");
            Console.Read();
        }
    }
}