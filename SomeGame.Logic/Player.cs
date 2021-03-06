using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Logic
{
    public class Player
    {
        private readonly Stack<GameCard> _deck = new();
        private readonly List<GameCard> _discardPile = new();
        private readonly List<GameCard> _hand = new();
        private readonly Stack<GameCard> _marketDeck = new();
        private readonly List<GameCard> _market = new();
        private readonly List<Minion> _field = new();
        private Player _rival;
        private bool _currentPlayer;

        public event EventHandler TurnEnded;

        public event EventHandler TurnStarted;

        public Player(string name, List<GameCard> deck, List<GameCard> marketDeck, bool isFirstPlayer)
        {
            Name = name;
            Health = 50;
            PopulateMarket(marketDeck);
            _discardPile.AddRange(deck);
            RepopulateDeck();
            _currentPlayer = isFirstPlayer;
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

        public IReadOnlyCollection<GameCard> Hand
            => _hand.AsReadOnly();

        public IReadOnlyCollection<ResourceAmount> HandResources { get; private set; }

        public IReadOnlyCollection<GameCard> Market
            => _market.AsReadOnly();

        public IReadOnlyCollection<Minion> Field
            => _field.AsReadOnly();

        public bool IsCurrentPlayer
            => _currentPlayer;

        public void EndTurn()
        {
            ValidateIsCurrentPlayer();

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
            _currentPlayer = false;
            TurnEnded?.Invoke(this, EventArgs.Empty);
        }

        private void PopulateMarket(List<GameCard> cards)
        {
            var random = new Random();
            while (cards.Count > 0)
            {
                var i = random.Next(cards.Count);
                var card = cards[i];
                cards.Remove(card);
                _marketDeck.Push(card);
            }
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
                .Select(t => t.Card)
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
            _currentPlayer = true;
            TurnStarted?.Invoke(this, EventArgs.Empty);
        }

        public void BuyCard(string cardId)
        {
            ValidateIsCurrentPlayer();

            var card = Market
                .Where(t => t.Id.Equals(cardId))
                .FirstOrDefault();

            if (card is null)
            {
                throw new CardNotFoundException();
            }

            if (!HasResources(card.Card.Cost))
            {
                throw new NotEnoughResourcesException();
            }

            _market.Remove(card);
            _discardPile.Add(card);
            RemoveResources(card.Card.Cost);
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
            ValidateIsCurrentPlayer();

            var card = Hand
                .Where(t => t.Id.Equals(cardId))
                .FirstOrDefault();

            if (card is null)
            {
                throw new CardNotFoundException();
            }

            if (card.Card is not MinionCard minionCard)
            {
                throw new InvalidCardTypeException();
            }

            _hand.Remove(card);
            _field.Add(new Minion(minionCard, cardId));
        }

        public void AttackHero(string cardId)
        {
            ValidateIsCurrentPlayer();

            var minion = Field
                .Where(t => t.CardId.Equals(cardId))
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
            ValidateIsCurrentPlayer();

            var minion = Field
                .Where(t => t.CardId.Equals(minionId))
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
                .Where(t => t.CardId.Equals(minionRivalId))
                .FirstOrDefault();

            if (minionRival is null)
            {
                throw new MinionNotFoundException(minionRivalId);
            }

            minionRival.Health -= minion.Attack;
            minion.Health -= minionRival.Attack;

            if (minionRival.Health <= 0)
            {
                RemoveMinionFromField(minionRival, _rival);
            }

            if (minion.Health <= 0)
            {
                RemoveMinionFromField(minion, this);
            }

            minion.Active = false;
        }

        private static void RemoveMinionFromField(Minion minion, Player player)
        {
            player._field.Remove(minion);
            player._discardPile.Add(new GameCard
            {
                Id = minion.CardId,
                Card = minion.Card
            });
        }

        private void ValidateIsCurrentPlayer()
        {
            if (!_currentPlayer)
            {
                throw new NotCurrentPlayerException();
            }
        }
    }
}
