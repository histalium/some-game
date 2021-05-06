using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Cli
{
    public class Minion
    {
        public Minion(MinionCard card)
        {
            Card = card;
            Health = card.Health;
            Attack = card.Attack;
        }

        public MinionCard Card { get; }

        public int Health { get; internal set; }

        public int Attack { get; internal set; }
    }
}
