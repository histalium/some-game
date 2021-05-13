using SomeGame.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Cli
{
    internal class ShowHeroHandler : CliCommandHandler
    {
        private readonly PlayerGate _gate;

        public ShowHeroHandler(PlayerGate gate)
            : base("^show hero$", Array.Empty<string>())
        {
            _gate = gate;
        }

        public override IEnumerable<string> Handle(string[] args)
        {
            var name = _gate.GetPlayerName();
            var health = _gate.GetPlayerHealth();
            yield return $"{name} (hp: {health})";
        }
    }
}
