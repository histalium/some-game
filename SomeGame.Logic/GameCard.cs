using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Logic
{
    public record GameCard
    {
        public string Id { get; init; }

        public Card Card { get; init; }
    }
}
