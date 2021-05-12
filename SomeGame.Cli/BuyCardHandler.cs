using SomeGame.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Cli
{
    internal class BuyCardHandler : CliCommandHandler
    {
        private readonly Game _game;

        public BuyCardHandler(Game game)
            : base(@"^buy (?<card>c\d+)$", new[] { "card" })
        {
            _game = game;
        }

        public override IEnumerable<string> Handle(string[] args)
        {
            var cardId = args[0];
            try
            {
                _game.CurrentPlayer.BuyCard(cardId);
                return Array.Empty<string>();
            }
            catch (CardNotFoundException)
            {
                return new[] { "Card not found in the market" };
            }
            catch (NotEnoughResourcesException)
            {
                return new[] { "You don't have enough resources to buy this card" };
            }
        }
    }
}
