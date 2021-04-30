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
            TurnEnded?.Invoke(this, EventArgs.Empty);
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
