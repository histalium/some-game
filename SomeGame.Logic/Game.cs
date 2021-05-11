using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Logic
{
    public class Game
    {
        private readonly ReadOnlyCollection<ResourceAmount> _noResources;
        private readonly Resource _resourceA = new() { Id = "ra" };

        public Game(string player1Name, IEnumerable<Card> market1, string player2Name, IEnumerable<Card> market2)
        {
            _noResources = Enumerable.Empty<ResourceAmount>().ToList().AsReadOnly();
            var player1Cards = CreateStartDeck(0);
            Player1 = new Player(player1Name, player1Cards, market1.ToList(), true);

            var player2Cards = CreateStartDeck(7);
            Player2 = new Player(player2Name, player2Cards, market2.ToList(), false);

            Player1.SetRival(Player2);
            Player2.SetRival(Player1);
        }

        public Player Player1 { get; private set; }

        public Player Player2 { get; private set; }

        private List<Card> CreateStartDeck(int startId)
        {
            var startDeck = Enumerable.Range(startId, 7)
                .Select(t => t < startId + 2 ? CreateKikaboo($"c{t}") : CreateOneOfA($"c{t}"))
                .Cast<Card>()
                .ToList();

            return startDeck;
        }

        private Card CreateKikaboo(string cardId)
        {
            return new MinionCard
            {
                Id = cardId,
                Name = "Kikaboo",
                Cost = _noResources,
                Health = 1,
                Attack = 1
            };
        }

        private Card CreateOneOfA(string cardId)
        {
            return new ResourceCard
            {
                Id = cardId,
                Name = "One of A",
                Cost = _noResources,
                Resources = new List<ResourceAmount> { new ResourceAmount { Resource = _resourceA, Amount = 1 } }
            };
        }
    }
}
