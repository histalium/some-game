using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Cli
{
    public class Player
    {
        public event EventHandler TurnEnded;

        public event EventHandler TurnStarted;

        public Player(string name)
        {
            Name = name;
        }

        public string Name { get; }

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
