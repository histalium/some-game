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
        private static List<Card> _defaultMarket;

        static void Main(string[] args)
        {
            _defaultMarket = BuildDefaultMarket();

            var port = 8080;
            var listener = new TcpListener(IPAddress.Any, port);

            listener.Start();
            listener.BeginAcceptTcpClient(new AsyncCallback(ConnectClient), listener);

            Console.WriteLine("Press any key to stop server");
            Console.ReadKey();

            listener.Stop();
        }

        private static List<Card> BuildDefaultMarket()
        {
            var ra = new Resource { Id = "ra" };
            var rb = new Resource { Id = "rb" };
            var twoOfA = new ResourceCard
            {
                Name = "Two of A",
                Cost = new[] { new ResourceAmount { Amount = 3, Resource = ra } },
                Resources = new[] { new ResourceAmount { Amount = 2, Resource = ra } }
            };
            var oneOfB = new ResourceCard
            {
                Name = "One of B",
                Cost = new[] { new ResourceAmount { Amount = 2, Resource = ra } },
                Resources = new[] { new ResourceAmount { Amount = 1, Resource = rb } }
            };
            var woop = new MinionCard
            {
                Name = "Woop",
                Cost = new[] { new ResourceAmount { Amount = 2, Resource = rb } },
                Health = 2,
                Attack = 2
            };
            var fiveTwoOfAs = Enumerable.Range(0, 5)
                .Select(t => twoOfA)
                .Cast<Card>();
            var tenWoops = Enumerable.Range(0, 10)
                .Select(t => woop)
                .Cast<Card>();
            var fifteenOneOfBs = Enumerable.Range(0, 15)
                .Select(t => oneOfB)
                .Cast<Card>();

            return fifteenOneOfBs
                .Concat(fiveTwoOfAs)
                .Concat(tenWoops)
                .ToList();
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
                var writer = new StreamWriter(ns, Encoding.ASCII);

                var name = "anonymous";

                var line = reader.ReadLine();
                while (line is not null)
                {
                    if (line.StartsWith("player ", StringComparison.OrdinalIgnoreCase))
                    {
                        name = line.Substring(7);
                    }
                    else if (line.Equals("find game", StringComparison.OrdinalIgnoreCase))
                    {
                        var other = _clients
                            .FirstOrDefault();

                        if (other != null)
                        {
                            _clients.Remove(other);
                            var c = new Client(ns, name);
                            var game = new Game(other.Name, _defaultMarket, c.Name, _defaultMarket);
                            c.StartGame(game.Gate2, game);
                            other.StartGame(game.Gate1, game);
                            c.Run();
                        }
                        else
                        {
                            var c = new Client(ns, name);
                            _clients.Add(c);
                            c.Run();
                        }
                    }
                    else
                    {
                        writer.WriteLine("invalid command");
                        writer.Flush();
                    }
                    line = reader.ReadLine();
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
