﻿using SomeGame.Logic;
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
        private static IReadOnlyCollection<ResourceAmount> _noResources;

        static void Main()
        {
            _noResources = Enumerable.Empty<ResourceAmount>().ToList().AsReadOnly();

            var resource1a = new ResourceAmount { Resource = _resourceA, Amount = 1 };
            var onlyResource1a = new List<ResourceAmount> { resource1a };
            var resource1b = new ResourceAmount { Resource = _resourceB, Amount = 1 };
            var onlyResource1b = new List<ResourceAmount> { resource1b };

            var player1MarketCards = Enumerable.Range(26, 30)
                .Select(t => new ResourceCard { Id = $"c{t}", Name = $"card {t}", Cost = CalculateCost(t), Resources = onlyResource1b })
                .Cast<Card>()
                .ToList();
            var player2MarketCards = Enumerable.Range(56, 30)
                .Select(t => new ResourceCard { Id = $"c{t}", Name = $"card {t}", Cost = CalculateCost(t), Resources = onlyResource1b })
                .Cast<Card>()
                .ToList();

            var game = new Game("player 1", player1MarketCards, "player 2", player2MarketCards);

            var player1 = game.Player1;
            var player2 = game.Player2;

            player1.TurnStarted += PlayerTurnStarted;
            player2.TurnStarted += PlayerTurnStarted;

            _currentPlayer = player1;

            Console.WriteLine($"{_currentPlayer.Name} is current player");

            while (true)
            {
                var line = Console.ReadLine();

                var addToField = Regex.Match(line, @"^add (?<card>c\d+) to field$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                var attackHero = Regex.Match(line, @"^attack hero with (?<card>c\d+)$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                var attackMinion = Regex.Match(line, @"^attack (?<minionRival>c\d+) with (?<minion>c\d+)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

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
                else if (line.StartsWith("buy ", StringComparison.InvariantCultureIgnoreCase))
                {
                    var cardId = line[4..];
                    BuyCard(_currentPlayer, cardId);
                }
                else if (addToField.Success)
                {
                    AddCardToField(_currentPlayer, addToField.Groups["card"].Value);
                }
                else if (line.Equals("show field", StringComparison.InvariantCultureIgnoreCase))
                {
                    PrintField(_currentPlayer);
                }
                else if (line.Equals("show field rival", StringComparison.InvariantCultureIgnoreCase))
                {
                    var rival = _currentPlayer == player1 ? player2 : player1;
                    PrintField(rival);
                }
                else if (line.Equals("show hero", StringComparison.InvariantCultureIgnoreCase))
                {
                    Console.WriteLine($"{_currentPlayer.Name} (hp: {_currentPlayer.Health})");
                }
                else if (line.Equals("show hero rival", StringComparison.InvariantCultureIgnoreCase))
                {
                    var rival = _currentPlayer == player1 ? player2 : player1;
                    Console.WriteLine($"{rival.Name} (hp: {rival.Health})");
                }
                else if (attackHero.Success)
                {
                    AttackHero(_currentPlayer, attackHero.Groups["card"].Value);
                }
                else if (attackMinion.Success)
                {
                    var minionRival = attackMinion.Groups["minionRival"].Value;
                    var minion = attackMinion.Groups["minion"].Value;
                    AttackMinion(_currentPlayer, minion, minionRival);
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

        private static void PrintHand(Player player)
        {
            foreach (var resource in player.HandResources)
            {
                Console.WriteLine($"{resource.Resource.Id}: {resource.Amount}");
            }

            foreach (var card in player.Hand)
            {
                if (card is ResourceCard resourceCard)
                {
                    Console.WriteLine($"{card.Id,4} {card.Name} ({string.Join(", ", resourceCard.Resources.Select(CostText))})");
                }
                else if (card is MinionCard minionCard)
                {
                    Console.WriteLine($"{card.Id,4} {card.Name} (hp: {minionCard.Health}, atk: {minionCard.Attack})");
                }
                else
                {
                    Console.WriteLine($"{card.Id,4} {card.Name}");
                }
            }
        }

        private static void PrintMarket(Player player)
        {
            foreach (var card in player.Market)
            {
                if (card is ResourceCard resourceCard)
                {
                    Console.WriteLine($"{card.Id,-4} {card.Name,-10} ({string.Join(", ", resourceCard.Resources.Select(CostText))}) cost: {string.Join(", ", card.Cost.Select(CostText))}");
                }
                else
                {
                    Console.WriteLine($"{card.Id,-4} {card.Name,-10} cost: {string.Join(", ", card.Cost.Select(CostText))}");
                }
            }
        }

        private static string CostText(ResourceAmount cost)
        {
            return $"{cost.Amount} {cost.Resource.Id}";
        }

        private static void BuyCard(Player player, string cardId)
        {
            try
            {
                player.BuyCard(cardId);
            }
            catch (CardNotFoundException)
            {
                Console.WriteLine("Card not found in the market");
            }
            catch (NotEnoughResourcesException)
            {
                Console.WriteLine("You don't have enough resources to buy this card");
            }
        }

        private static void AddCardToField(Player player, string cardId)
        {
            try
            {
                player.AddMinionToField(cardId);
            }
            catch (CardNotFoundException)
            {
                Console.WriteLine("Card not found in your hand");
            }
            catch (InvalidCardTypeException)
            {
                Console.WriteLine("Card type can't be added to field");
            }
        }

        private static void PrintField(Player player)
        {
            foreach (var minion in player.Field)
            {
                var activeText = minion.Active ? "active" : "not active";
                Console.WriteLine($"{minion.Card.Id,-4} hp: {minion.Health}, atk: {minion.Attack} ({activeText})");
            }
        }

        private static void AttackHero(Player player, string cardId)
        {
            try
            {
                player.AttackHero(cardId);
            }
            catch (MinionNotFoundException)
            {
                Console.WriteLine("Minion not found on the field");
            }
            catch (MinionNotActiveException)
            {
                Console.WriteLine("Minion is not active");
            }
        }

        private static void AttackMinion(Player player, string minion, string minionRival)
        {
            try
            {
                player.AttackMinion(minion, minionRival);
            }
            catch (MinionNotFoundException ex)
            {
                if (ex.CardId.Equals(minion))
                {
                    Console.WriteLine("Minion not found on your field");
                }
                else
                {
                    Console.WriteLine("Minion not found on your rivals field");
                }
            }
            catch (MinionNotActiveException)
            {
                Console.WriteLine("Minion is not active");
            }
        }
    }
}
