using System;

namespace SomeGame.Cli
{
    class Program
    {
        private static Player _currentPlayer;

        static void Main()
        {
            var player1 = new Player("player 1");
            var player2 = new Player("player 2");

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
    }
}
