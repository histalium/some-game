using SomeGame.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.TextCommands
{
    internal class ShowMarketHandler : CliCommandHandler
    {
        private readonly PlayerGate _gate;

        public ShowMarketHandler(PlayerGate gate)
            : base("^show market$", Array.Empty<string>())
        {
            _gate = gate;
        }

        public override IEnumerable<string> Handle(string[] args)
        {
            foreach (var card in _gate.GetMarket())
            {
                if (card.Card is ResourceCard resourceCard)
                {
                    yield return $"{card.Id,-4} {card.Card.Name,-10} ({string.Join(", ", resourceCard.Resources.Select(Utilities.CostText))}) cost: {string.Join(", ", card.Card.Cost.Select(Utilities.CostText))}";
                }
                else
                {
                    yield return $"{card.Id,-4} {card.Card.Name,-10} cost: {string.Join(", ", card.Card.Cost.Select(Utilities.CostText))}";
                }
            }
        }
    }
}
