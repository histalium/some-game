using SomeGame.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Cli
{
    internal class AddToFieldHandler : CliCommandHandler
    {
        private readonly Game _game;

        public AddToFieldHandler(Game game)
            : base(@"^add (?<card>c\d+) to field$", new[] { "card" })
        {
            _game = game;
        }

        public override IEnumerable<string> Handle(string[] args)
        {
            var cardId = args[0];
            try
            {
                _game.CurrentPlayer.AddMinionToField(cardId);
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
        }
    }
}
