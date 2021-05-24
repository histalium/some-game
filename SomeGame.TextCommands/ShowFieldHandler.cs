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
        private readonly PlayerGate _gate;

        public ShowFieldHandler(PlayerGate gate)
            : base("^show field$", Array.Empty<string>())
        {
            _gate = gate;
        }

        public override IEnumerable<string> Handle(string[] args)
        {
            return Utilities.PrintField(_gate.GetField());
        }
    }
}
