using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCP_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = null;
            string[] words = new string[] { "auto", "letadlo", "autobus", "zebra", "notebook", "pocitac", "skola", "strom", "program", "mapa" };
            string[] hangman = new string[]
            {
                "\n +---+\n |   |\n     |\n     |\n     |\n     |\n /=======\\",
                "\n +---+\n |   |\n O   |\n     |\n     |\n     |\n /=======\\",
                "\n +---+\n |   |\n O   |\n |   |\n     |\n     |\n /=======\\",
                "\n  +---+\n  |   |\n  O   |\n /|   |\n      |\n      |\n /=======\\",
                "\n  +---+\n  |   |\n  O   |\n /|\\  |\n      |\n      |\n /=======\\",
                "\n  +---+\n  |   |\n  O   |\n /|\\  |\n /    |\n      |\n /=======\\",
                "\n  +---+\n  |   |\n  O   |\n /|\\  |\n / \\  |\n      |\n /=======\\",
            };
            try
            {
                Int32 port = 13000;
                IPAddress localIPAddr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localIPAddr, port);
                server.Start();
                Byte[] bytes = new Byte[256];
                Byte[] msg = new Byte[256];
                String data = null;

                while (true)
                {
                    Console.WriteLine("Waiting for connection...");
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected");

                    Random randGen = new Random();
                    var index = randGen.Next(0, words.Length - 1);

                    char[] choosenWord = words[index].ToCharArray();
                    char[] guess = string.Concat(Enumerable.Repeat("*", choosenWord.Length)).ToCharArray();

                    data = null;

                    NetworkStream stream = client.GetStream();

                    int i;
                    bool win = false;
                    int counter = -1;
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = Encoding.ASCII.GetString(bytes, 0, i);
                        char[] incomingData = data.ToCharArray();
                        bool correct = false;
                        for (int y = 0; y < choosenWord.Length; y++)
                        {
                            if (choosenWord[y] == incomingData[0])
                            {
                                guess[y] = incomingData[0];
                                correct = true;
                            }                   
                        }
                        if (!correct)
                        {
                            counter++;
                        }
                        win = true;
                        foreach (char znak in guess)
                        {
                            if (znak == '*')
                            {
                                win = false;
                            }
                        }
                        if (counter >= 6)
                        {
                            msg = Encoding.ASCII.GetBytes("Prohrál jste! Hádané slovo bylo: " + choosenWord + hangman[counter]);
                            client.Close();
                            Console.WriteLine("\nStiskni Enter pro pokračování...");
                            Console.Read();
                        }
                        if (win)
                        {
                            msg = Encoding.ASCII.GetBytes("Vyhrál jste! Hádané slovo bylo: " + choosenWord);
                            client.Close();
                            Console.WriteLine("\nStiskni Enter pro pokračování...");
                            Console.Read();
                        }
                        else
                        {
                            Console.WriteLine(guess);
                            msg = Encoding.ASCII.GetBytes(guess + hangman[counter]);
                        }
                        
                        stream.Write(msg, 0, msg.Length);
                    }
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server.Stop();
            }

            Console.WriteLine("\nStiskni Enter pro pokračování...");
            Console.Read();
        }
    }
}
