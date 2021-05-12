using SomeGame.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Cli
{
    internal class ShowMarketHandler : CliCommandHandler
    {
        private readonly Game _game;

        public ShowMarketHandler(Game game)
            : base("^show market$", Array.Empty<string>())
        {
            _game = game;
        }

        public override IEnumerable<string> Handle(string[] args)
        {
            foreach (var card in _game.CurrentPlayer.Market)
            {
                if (card.Card is ResourceCard resourceCard)
                {
                    yield return $"{card.Id,-4} {card.Card.Name,-10} ({string.Join(", ", resourceCard.Resources.Select(Program.CostText))}) cost: {string.Join(", ", card.Card.Cost.Select(Program.CostText))}";
                }
                else
                {
                    yield return $"{card.Id,-4} {card.Card.Name,-10} cost: {string.Join(", ", card.Card.Cost.Select(Program.CostText))}";
                }
            }
        }
    }
}
