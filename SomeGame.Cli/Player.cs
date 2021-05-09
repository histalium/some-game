using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Cli
{
    public class Player
    {
        private readonly Stack<Card> _deck = new();
        private readonly List<Card> _discardPile = new();
        private readonly List<Card> _hand = new();
        private readonly Stack<Card> _marketDeck;
        private readonly List<Card> _market = new();
        private readonly List<Minion> _field = new();
        private Player _rival;

        public event EventHandler TurnEnded;

        public event EventHandler TurnStarted;

        public Player(string name, List<Card> deck, List<Card> marketDeck, bool isFirstPlayer)
        {
            Name = name;
            Health = 50;
            _marketDeck = new Stack<Card>(marketDeck);
            _discardPile.AddRange(deck);
            RepopulateDeck();
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

        public int Health { get; private set; }

        public IReadOnlyCollection<Card> Hand
            => _hand.AsReadOnly();

        public IReadOnlyCollection<ResourceAmount> HandResources { get; private set; }

        public IReadOnlyCollection<Card> Market
            => _market.AsReadOnly();

        public IReadOnlyCollection<Minion> Field
            => _field.AsReadOnly();

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
            foreach (var minion in _field)
            {
                minion.Active = true;
            }
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

        public void SetRival(Player player)
        {
            _rival = player;
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
            foreach (var resource in resources)
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

                    if (resource is null)
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

        public void AddMinionToField(string cardId)
        {
            var card = Hand
                .Where(t => t.Id.Equals(cardId))
                .FirstOrDefault();

            if (card is null)
            {
                throw new CardNotFoundException();
            }

            if (card is not MinionCard minionCard)
            {
                throw new InvalidCardTypeException();
            }

            _hand.Remove(card);
            _field.Add(new Minion(minionCard));
        }

        public void AttackHero(string cardId)
        {
            var minion = Field
                .Where(t => t.Card.Id.Equals(cardId))
                .FirstOrDefault();

            if (minion is null)
            {
                throw new MinionNotFoundException(cardId);
            }

            if (!minion.Active)
            {
                throw new MinionNotActiveException();
            }

            _rival.Health -= minion.Attack;
            minion.Active = false;
        }

        public void AttackMinion(string minionId, string minionRivalId)
        {
            var minion = Field
                .Where(t => t.Card.Id.Equals(minionId))
                .FirstOrDefault();

            if (minion is null)
            {
                throw new MinionNotFoundException(minionId);
            }

            if (!minion.Active)
            {
                throw new MinionNotActiveException();
            }

            var minionRival = _rival.Field
                .Where(t => t.Card.Id.Equals(minionRivalId))
                .FirstOrDefault();

            if (minionRival is null)
            {
                throw new MinionNotFoundException(minionRivalId);
            }

            if (minionRival.Health <= minion.Attack)
            {
                minionRival.Health = 0;
                _rival._field.Remove(minionRival);
                _rival._discardPile.Add(minionRival.Card);
            }
            else
            {
                minionRival.Health -= minion.Attack;
            }
            minion.Active = false;
        }
    }
}
