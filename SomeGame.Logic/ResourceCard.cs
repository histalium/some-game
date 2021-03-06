using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Logic
{
    public record ResourceCard : Card
    {
        public IReadOnlyCollection<ResourceAmount> Resources { get; init; }
    }
}
