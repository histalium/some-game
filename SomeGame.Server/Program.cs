using SomeGame.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SomeGame.Server
{
    class Program
    {
        private static List<Client> _clients = new();

        static void Main(string[] args)
        {
            var port = 8080;
            var listener = new TcpListener(IPAddress.Any, port);

            listener.Start();
            listener.BeginAcceptTcpClient(new AsyncCallback(ConnectClient), listener);

            Console.WriteLine("Press any key to stop server");
            Console.ReadKey();

            listener.Stop();
        }

        private static void ConnectClient(IAsyncResult result)
        {
            var listener = (TcpListener)result.AsyncState;

            var client = listener.EndAcceptTcpClient(result);
            listener.BeginAcceptTcpClient(new AsyncCallback(ConnectClient), listener);

            try
            {
                var ns = client.GetStream();
                var reader = new StreamReader(ns, Encoding.ASCII);
                var line = reader.ReadLine();
                if (line.StartsWith("player ", StringComparison.OrdinalIgnoreCase))
                {
                    var player = line.Substring(7);
                    var c = new Client(ns, player);

                    var other = _clients
                        .Where(t => t != c)
                        .FirstOrDefault();

                    if (other != null)
                    {
                        var card = new ResourceCard
                        {
                            Name = "Market card",
                            Cost = new[] { new ResourceAmount { Amount = 2, Resource = new Resource { Id = "r1" } } }
                        };
                        var marketCards = Enumerable.Range(26, 30)
                            .Select(t => card)
                            .Cast<Card>()
                            .ToList();

                        var game = new Game(other.Name, marketCards, c.Name, marketCards);
                        c.StartGame(game);
                        other.StartGame(game);
                    }

                    _clients.Add(c);
                    c.Run();
                    _clients.Remove(c);
                }
                else
                {
                    var writer = new StreamWriter(ns, Encoding.ASCII);
                    writer.WriteLine("400 invalid command");
                    writer.Flush();
                }

                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
