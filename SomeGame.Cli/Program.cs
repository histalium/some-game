using System;
using System.Collections.Generic;
using System.Linq;

namespace SomeGame.Cli
{
    class Program
    {
        private static Player _currentPlayer;

        private static Resource _resourceA = new Resource { Id = "ra" };
        private static Resource _resourceB = new Resource { Id = "rb" };
        private static Resource _resourceC = new Resource { Id = "rc" };

        static void Main()
        {
            var noResources = Enumerable.Empty<Cost>().ToList().AsReadOnly();

            var player1Cards = Enumerable.Range(0, 13)
                .Select(t => new Card { Id = $"c{t}", Name = $"card {t}", Cost = noResources })
                .ToList();
            var player2Cards = Enumerable.Range(13, 13)
                .Select(t => new Card { Id = $"c{t}", Name = $"card {t}", Cost = noResources })
                .ToList();

            var player1MarketCards = Enumerable.Range(26, 30)
                .Select(t => new Card { Id = $"c{t}", Name = $"card {t}", Cost = CalculateCost(t) })
                .ToList();
            var player2MarketCards = Enumerable.Range(56, 30)
                .Select(t => new Card { Id = $"c{t}", Name = $"card {t}", Cost = CalculateCost(t) })
                .ToList();

            var player1 = new Player("player 1", player1Cards, player1MarketCards, true);
            var player2 = new Player("player 2", player2Cards, player2MarketCards, false);

            player1.TurnStarted += PlayerTurnStarted;
            player2.TurnStarted += PlayerTurnStarted;

            player1.SetOtherPlayer(player2);
            player2.SetOtherPlayer(player1);

            _currentPlayer = player1;

            Console.WriteLine($"{_currentPlayer.Name} is current player");

            while (true)
            {
                var line = Console.ReadLine();

                if (line.Equals("end turn", StringComparison.InvariantCultureIgnoreCase))
                {
                    _currentPlayer.EndTurn();
                }
                else if (line.Equals("show hand", StringComparison.InvariantCultureIgnoreCase))
                {
                    PrintHand(_currentPlayer);
                }
                else if (line.Equals("show market", StringComparison.InvariantCultureIgnoreCase))
                {
                    PrintMarket(_currentPlayer);
                }
                else
                {
                    Console.WriteLine("invalid command");
                }
            }
        }

        private static IReadOnlyCollection<Cost> CalculateCost(int number)
        {
            if ( number % 3 == 0)
            {
                return new List<Cost> { new Cost { Amount = 1, Resource = _resourceA } };
            }

            if (number % 3 == 1)
            {
                return new List<Cost> { new Cost { Amount = 1, Resource = _resourceB } };
            }

            if (number % 3 == 3 && number % 5 == 0)
            {
                return new List<Cost> 
                { 
                    new Cost { Amount = 1, Resource = _resourceC },
                    new Cost { Amount = 1, Resource = _resourceA }
                };
            }

            return new List<Cost> { new Cost { Amount = 2, Resource = _resourceB } };
        }

        private static void PlayerTurnStarted(object sender, EventArgs e)
        {
            _currentPlayer = sender as Player;
            Console.WriteLine($"{_currentPlayer.Name} is current player");
        }

        private static void PrintHand(Player player)
        {
            foreach (var card in player.Hand)
            {
                Console.WriteLine($"{card.Id,4} {card.Name}");
            }
        }

        private static void PrintMarket(Player player)
        {
            foreach (var card in player.Market)
            {
                Console.WriteLine($"{card.Id, -4} {card.Name, -10} {string.Join(", ", card.Cost.Select(CostText))}");
            }
        }

        private static string CostText(Cost cost)
        {
            return $"{cost.Amount} {cost.Resource.Id}";
        }
    }
}
