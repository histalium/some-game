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
        private readonly PlayerGate _gate;

        public EndTurnHandler(PlayerGate gate)
            : base("^end turn$", Array.Empty<string>())
        {
            _gate = gate;
        }

        public override IEnumerable<string> Handle(string[] args)
        {
            try
            {
                _gate.EndTurn();
                return Enumerable.Empty<string>();
            }
            catch(NotCurrentPlayerException)
            {
                return new[] { "You are not the current player" };
            }
        }
    }
}
