using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Decks
{
    public class DeckLogic
    {
        private readonly Action<Deck> _addDeck;

        public DeckLogic(Action<Deck> addDeck)
        {
            _addDeck = addDeck;
        }

        public void CreateDeck(string player, string name)
        {
            var deck = new Deck
            {
                Id = Guid.NewGuid(),
                Name = name,
                Player = player
            };

            _addDeck(deck);
        }
    }
}
