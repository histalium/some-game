using SomeGame.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.TextCommands
{
    internal class AddToFieldHandler : CliCommandHandler
    {
        private readonly PlayerGate _gate;

        public AddToFieldHandler(PlayerGate gate)
            : base(@"^add (?<card>c\d+) to field$", new[] { "card" })
        {
            _gate = gate;
        }

        public override IEnumerable<string> Handle(string[] args)
        {
            var cardId = args[0];
            try
            {
                _gate.AddMinionToField(cardId);
                return Array.Empty<string>();
            }
            catch (CardNotFoundException)
            {
                return new[] { "Card not found in your hand" };
            }
            catch (InvalidCardTypeException)
            {
                return new[] { "Card type can't be added to field" };
            }
            catch (NotCurrentPlayerException)
            {
                return new[] { "You are not the current player" };
            }
        }
    }
}
