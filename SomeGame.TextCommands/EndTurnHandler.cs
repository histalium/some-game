using SomeGame.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SomeGame.TextCommands
{
    internal class EndTurnHandler : CliCommandHandler
    {
        private readonly Game _game;

        public EndTurnHandler(Game game)
            : base("^end turn$", Array.Empty<string>())
        {
            _game = game;
        }

        public override IEnumerable<string> Handle(string[] args)
        {
            _game.CurrentPlayer.EndTurn();
            return Enumerable.Empty<string>();
        }
    }
}
