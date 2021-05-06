using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Cli
{
    public class Player
    {
        private readonly Stack<Card> _deck;
        private readonly List<Card> _discardPile = new();
        private readonly List<Card> _hand = new();
        private readonly Stack<Card> _marketDeck;
        private readonly List<Card> _market = new();

        public event EventHandler TurnEnded;

        public event EventHandler TurnStarted;

        public Player(string name, List<Card> deck, List<Card> marketDeck, bool isFirstPlayer)
        {
            Name = name;
            _marketDeck = new Stack<Card>(marketDeck);
            _deck = new Stack<Card>(deck);
            var openingHandSize = isFirstPlayer ? 3 : 5;
            for (var i = 0; i < openingHandSize; i++)
            {
                _hand.Add(_deck.Pop());
            }
            UpdateHandResources();
            for (var i = 0; i < 4; i++)
            {
                _market.Add(_marketDeck.Pop());
            }
        }

        public string Name { get; }

        public IReadOnlyCollection<Card> Hand
            => _hand.AsReadOnly();

        public IReadOnlyCollection<ResourceAmount> HandResources { get; private set; }

        public IReadOnlyCollection<Card> Market
            => _market.AsReadOnly();

        public void EndTurn()
        {
            _discardPile.AddRange(_hand);
            _hand.Clear();
            for (var i = 0; i < 5; i++)
            {
                if (_deck.Count == 0)
                {
                    RepopulateDeck();
                }
                _hand.Add(_deck.Pop());
            }
            UpdateHandResources();
            TurnEnded?.Invoke(this, EventArgs.Empty);
        }

        private void RepopulateDeck()
        {
            var random = new Random();
            while (_discardPile.Count > 0)
            {
                var i = random.Next(_discardPile.Count);
                var card = _discardPile[i];
                _discardPile.Remove(card);
                _deck.Push(card);
            }
        }

        private void UpdateHandResources()
        {
            HandResources = _hand
               .OfType<ResourceCard>()
               .SelectMany(t => t.Resources)
               .GroupBy(t => t.Resource)
               .Select(t => new ResourceAmount
               {
                   Resource = t.Key,
                   Amount = t.Sum(u => u.Amount)
               })
               .ToList();
        }

        public void SetOtherPlayer(Player player)
        {
            player.TurnEnded += OtherPlayerTurnEnded;
        }

        private void OtherPlayerTurnEnded(object sender, EventArgs e)
        {
            TurnStarted?.Invoke(this, EventArgs.Empty);
        }

        public void BuyCard(string cardId)
        {
            var card = Market
                .Where(t => t.Id.Equals(cardId))
                .FirstOrDefault();

            if (card is null)
            {
                throw new CardNotFoundException();
            }

            if (!HasResources(card.Cost))
            {
                throw new NotEnoughResourcesException();
            }

            _market.Remove(card);
            _discardPile.Add(card);
            RemoveResources(card.Cost);
            _market.Add(_marketDeck.Pop());
        }

        private bool HasResources(IReadOnlyCollection<ResourceAmount> resources)
        {
            foreach(var resource in resources)
            {
                var inHand = HandResources
                    .Where(t => t.Resource == resource.Resource)
                    .FirstOrDefault();

                if (inHand is null)
                {
                    return false;
                }

                if (inHand.Amount < resource.Amount)
                {
                    return false;
                }
            }

            return true;
        }

        private bool RemoveResources(IReadOnlyCollection<ResourceAmount> resources)
        {
            HandResources = HandResources
                .Select(t =>
                {
                    var resource = resources
                        .Where(u => u.Resource == t.Resource)
                        .FirstOrDefault();

                    if(resource is null)
                    {
                        return t;
                    }

                    return new ResourceAmount
                    {
                        Resource = t.Resource,
                        Amount = t.Amount - resource.Amount
                    };
                })
                .ToList();

            return true;
        }
    }
}
