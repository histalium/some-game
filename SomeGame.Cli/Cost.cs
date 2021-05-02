using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Cli
{
    public record Cost
    {
        public int Amount { get; init; }

        public Resource Resource { get; init; }
    }
}
