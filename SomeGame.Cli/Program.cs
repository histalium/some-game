using System;
using System.Linq;

namespace SomeGame.Cli
{
    class Program
    {
        private static Player _currentPlayer;

        static void Main()
        {
            var player1Cards = Enumerable.Range(0, 13)
                .Select(t => new Card { Id = $"c{t}", Name = $"card {t}" })
                .ToList();
            var player2Cards = Enumerable.Range(13, 13)
                .Select(t => new Card { Id = $"c{t}", Name = $"card {t}" })
                .ToList();

            var player1 = new Player("player 1", player1Cards, true);
            var player2 = new Player("player 2", player2Cards, false);

            player1.TurnStarted += PlayerTurnStarted;
            player2.TurnStarted += PlayerTurnStarted;

            player1.SetOtherPlayer(player2);
            player2.SetOtherPlayer(player1);

            _currentPlayer = player1;

            Console.WriteLine($"{_currentPlayer.Name} is current player");

            while (true)
            {
                var line = Console.ReadLine();

                if (line.Equals("end turn", StringComparison.InvariantCultureIgnoreCase))
                {
                    _currentPlayer.EndTurn();
                }
                else if (line.Equals("show hand", StringComparison.InvariantCultureIgnoreCase))
                {
                    PrintHand(_currentPlayer);
                }
                else
                {
                    Console.WriteLine("invalid command");
                }
            }
        }

        private static void PlayerTurnStarted(object sender, EventArgs e)
        {
            _currentPlayer = sender as Player;
            Console.WriteLine($"{_currentPlayer.Name} is current player");
        }

        private static void PrintHand(Player player)
        {
            foreach (var card in _currentPlayer.Hand)
            {
                Console.WriteLine($"{card.Id,4} {card.Name}");
            }
        }
    }
}
