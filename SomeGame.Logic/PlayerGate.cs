using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Logic
{
    public class PlayerGate
    {
        private readonly Player _player;

        internal PlayerGate(Player player)
        {
            _player = player;
        }

        public string GetPlayerName()
        {
            return _player.Name;
        }

        public int GetPlayerHealth()
        {
            return _player.Health;
        }
    }
}
