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
        private readonly List<Card> _discardPile = new List<Card>();
        private readonly List<Card> _hand = new List<Card>();

        public event EventHandler TurnEnded;

        public event EventHandler TurnStarted;

        public Player(string name, List<Card> deck, bool isFirstPlayer)
        {
            Name = name;
            _deck = new Stack<Card>(deck);
            var openingHandSize = isFirstPlayer ? 3 : 5;
            for ( var i = 0; i < openingHandSize; i++)
            {
                _hand.Add(_deck.Pop());
            }
        }

        public string Name { get; }

        public IReadOnlyCollection<Card> Hand
            => _hand.AsReadOnly();

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

        public void SetOtherPlayer(Player player)
        {
            player.TurnEnded += OtherPlayerTurnEnded;
        }

        private void OtherPlayerTurnEnded(object sender, EventArgs e)
        {
            TurnStarted?.Invoke(this, EventArgs.Empty);
        }
    }
}
