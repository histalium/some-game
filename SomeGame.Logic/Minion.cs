using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Logic
{
    public class Minion
    {
        public Minion(MinionCard card, string cardId)
        {
            Card = card;
            CardId = cardId;
            Health = card.Health;
            Attack = card.Attack;
            Active = false;
        }

        public string CardId { get; }

        public MinionCard Card { get; }

        public int Health { get; internal set; }

        public int Attack { get; internal set; }

        public bool Active { get; internal set; }
    }
}
