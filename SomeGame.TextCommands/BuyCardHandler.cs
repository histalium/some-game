using SomeGame.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.TextCommands
{
    internal class BuyCardHandler : CliCommandHandler
    {
        private readonly PlayerGate _gate;

        public BuyCardHandler(PlayerGate gate)
            : base(@"^buy (?<card>c\d+)$", new[] { "card" })
        {
            _gate = gate;
        }

        public override IEnumerable<string> Handle(string[] args)
        {
            var cardId = args[0];
            try
            {
                _gate.BuyCard(cardId);
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
            catch (NotCurrentPlayerException)
            {
                return new[] { "You are not the current player" };
            }
        }
    }
}
