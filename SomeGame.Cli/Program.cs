using SomeGame.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SomeGame.Cli
{
    class Program
    {
        private static Player _currentPlayer;

        private static readonly Resource _resourceA = new() { Id = "ra" };
        private static readonly Resource _resourceB = new() { Id = "rb" };
        private static readonly Resource _resourceC = new() { Id = "rc" };

        static void Main()
        {
            var resource1a = new ResourceAmount { Resource = _resourceA, Amount = 1 };
            var onlyResource1a = new List<ResourceAmount> { resource1a };
            var resource1b = new ResourceAmount { Resource = _resourceB, Amount = 1 };
            var onlyResource1b = new List<ResourceAmount> { resource1b };

            var player1MarketCards = Enumerable.Range(26, 30)
                .Select(t => new ResourceCard { Name = $"card {t}", Cost = CalculateCost(t), Resources = onlyResource1b })
                .Cast<Card>()
                .ToList();
            var player2MarketCards = Enumerable.Range(56, 30)
                .Select(t => new ResourceCard { Name = $"card {t}", Cost = CalculateCost(t), Resources = onlyResource1b })
                .Cast<Card>()
                .ToList();

            var game = new Game("player 1", player1MarketCards, "player 2", player2MarketCards);

            var player1 = game.Player1;
            var player2 = game.Player2;

            player1.TurnStarted += PlayerTurnStarted;
            player2.TurnStarted += PlayerTurnStarted;

            _currentPlayer = player1;

            Console.WriteLine($"{_currentPlayer.Name} is current player");

            var commandHandlers = new List<CliCommandHandler>
            {
                new AddToFieldHandler(game),
                new AttackHeroHandler(game),
                new AttackMinionHandler(game),
                new BuyCardHandler(game),
                new EndTurnHandler(game),
                new ShowFieldHandler(game),
                new ShowFieldRivalHandler(game),
                new ShowHandHandler(game),
                new ShowHeroHandler(game),
                new ShowHeroRivalHandler(game),
                new ShowMarketHandler(game),
            };

            while (true)
            {
                var line = Console.ReadLine();
                var match = commandHandlers
                    .Select(t => (Match: t.CommandPattern.Match(line), Handler: t))
                    .Where(t => t.Match.Success)
                    .FirstOrDefault();

                if (match.Match?.Success == true)
                {
                    var args = match.Handler.ArgNames
                        .Select(t => match.Match.Groups[t].Value)
                        .ToArray();

                    foreach (var write in match.Handler.Handle(args))
                    {
                        Console.WriteLine(write);
                    }
                }
                else
                {
                    Console.WriteLine("invalid command");
                }
            }
        }

        private static IReadOnlyCollection<ResourceAmount> CalculateCost(int number)
        {
            if (number % 3 == 0)
            {
                return new List<ResourceAmount> { new ResourceAmount { Amount = 1, Resource = _resourceA } };
            }

            if (number % 3 == 1)
            {
                return new List<ResourceAmount> { new ResourceAmount { Amount = 1, Resource = _resourceB } };
            }

            if (number % 3 == 3 && number % 5 == 0)
            {
                return new List<ResourceAmount>
                {
                    new ResourceAmount { Amount = 1, Resource = _resourceC },
                    new ResourceAmount { Amount = 1, Resource = _resourceA }
                };
            }

            return new List<ResourceAmount> { new ResourceAmount { Amount = 2, Resource = _resourceB } };
        }

        private static void PlayerTurnStarted(object sender, EventArgs e)
        {
            _currentPlayer = sender as Player;
            Console.WriteLine($"{_currentPlayer.Name} is current player");
        }

        internal static string CostText(ResourceAmount cost)
        {
            return $"{cost.Amount} {cost.Resource.Id}";
        }

        internal static IEnumerable<string> PrintField(Player player)
        {
            foreach (var minion in player.Field)
            {
                var activeText = minion.Active ? "active" : "not active";
                yield return $"{minion.CardId,-4} hp: {minion.Health}, atk: {minion.Attack} ({activeText})";
            }
        }
    }
}
