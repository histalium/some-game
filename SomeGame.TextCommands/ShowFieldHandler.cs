using SomeGame.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.TextCommands
{
    internal class ShowFieldHandler : CliCommandHandler
    {
        private readonly Game _game;

        public ShowFieldHandler(Game game)
            : base("^show field$", Array.Empty<string>())
        {
            _game = game;
        }

        public override IEnumerable<string> Handle(string[] args)
        {
            return Utilities.PrintField(_game.CurrentPlayer);
        }
    }
}
