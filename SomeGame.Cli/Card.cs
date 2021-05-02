using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Cli
{
    public record Card
    {
        public string Id { get; init; }

        public string Name { get; init; }

        public IReadOnlyCollection<Cost> Cost { get; init; }
    }
}
