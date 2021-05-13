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
        private readonly MinionCard _kikaboo;
        private readonly ResourceCard _oneOfA;

        public Game(string player1Name, IEnumerable<Card> market1, string player2Name, IEnumerable<Card> market2)
        {
            _noResources = Enumerable.Empty<ResourceAmount>().ToList().AsReadOnly();
            _kikaboo = new MinionCard
            {
                Name = "Kikaboo",
                Cost = _noResources,
                Health = 1,
                Attack = 1
            };
            _oneOfA = new ResourceCard
            {
                Name = "One of A",
                Cost = _noResources,
                Resources = new List<ResourceAmount> { new ResourceAmount { Resource = _resourceA, Amount = 1 } }
            };

            var player1Cards = CreateStartDeck(0);
            var player1Market = CreateGameCard(market1, 14);
            Player1 = new Player(player1Name, player1Cards, player1Market.ToList(), true);

            var player2Cards = CreateStartDeck(7);
            var player2Market = CreateGameCard(market2, 14 + market1.Count());
            Player2 = new Player(player2Name, player2Cards, player2Market.ToList(), false);

            Player1.SetRival(Player2);
            Player2.SetRival(Player1);

            Gate1 = new PlayerGate(Player1);
            Gate2 = new PlayerGate(Player2);

            Player1.TurnStarted += PlayerTurnStarted;
            Player2.TurnStarted += PlayerTurnStarted;

            CurrentPlayer = Player1;
        }

        public Player Player1 { get; private set; }

        public Player Player2 { get; private set; }

        public Player CurrentPlayer { get; private set; }

        public PlayerGate Gate1 { get; private set; }

        public PlayerGate Gate2 { get; private set; }

        private List<GameCard> CreateStartDeck(int startId)
        {
            var startDeck = Enumerable.Range(startId, 7)
                .Select(t => t < startId + 2 ? CreateKikaboo($"c{t}") : CreateOneOfA($"c{t}"))
                .Cast<GameCard>()
                .ToList();

            return startDeck;
        }

        private GameCard CreateKikaboo(string cardId)
        {
            return new GameCard
            {
                Id = cardId,
                Card = _kikaboo
            };
        }

        private GameCard CreateOneOfA(string cardId)
        {
            return new GameCard
            {
                Id = cardId,
                Card = _oneOfA
            };
        }

        private static IEnumerable<GameCard> CreateGameCard(IEnumerable<Card> cards, int startId)
        {
            return Zip(cards, Infinite(startId))
                .Select(t => new GameCard
                {
                    Id = $"c{t.Item2}",
                    Card = t.Item1
                });
        }

        private static IEnumerable<(T, U)> Zip<T, U>(IEnumerable<T> t, IEnumerable<U> u)
        {
            var enumeratorT = t.GetEnumerator();
            var enumeratorU = u.GetEnumerator();
            while (enumeratorT.MoveNext() && enumeratorU.MoveNext())
            {
                yield return (enumeratorT.Current, enumeratorU.Current);
            }
        }

        private static IEnumerable<int> Infinite(int start)
        {
            var current = start;
            while (true)
            {
                yield return current;
                current++;
            }
        }

        private void PlayerTurnStarted(object sender, EventArgs e)
        {
            CurrentPlayer = sender as Player;
        }
    }
}
