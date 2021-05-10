using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Logic
{
    public record MinionCard : Card
    {
        public int Health { get; init; }

        public int Attack { get; init; }
    }
}
