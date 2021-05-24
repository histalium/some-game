using SomeGame.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.TextCommands
{
    internal class ShowFieldRivalHandler : CliCommandHandler
    {
        private readonly PlayerGate _gate;

        public ShowFieldRivalHandler(PlayerGate gate)
            : base("^show field rival$", Array.Empty<string>())
        {
            _gate = gate;
        }

        public override IEnumerable<string> Handle(string[] args)
        {
            return Utilities.PrintField(_gate.GetFieldRival());
        }
    }
}
