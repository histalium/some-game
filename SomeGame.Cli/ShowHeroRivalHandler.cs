using SomeGame.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Cli
{
    internal class ShowHeroRivalHandler : CliCommandHandler
    {
        private readonly PlayerGate _gate;

        public ShowHeroRivalHandler(PlayerGate gate)
            : base("^show hero rival$", Array.Empty<string>())
        {
            _gate = gate;
        }

        public override IEnumerable<string> Handle(string[] args)
        {
            var name = _gate.GetRivalName();
            var health = _gate.GetRivalHealth();
            yield return $"{name} (hp: {health})";
        }
    }
}
