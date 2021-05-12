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
        private readonly Game _game;

        public ShowHeroHandler(Game game)
            : base("^show hero$", Array.Empty<string>())
        {
            _game = game;
        }

        public override IEnumerable<string> Handle(string[] args)
        {
            yield return $"{_game.CurrentPlayer.Name} (hp: {_game.CurrentPlayer.Health})";
        }
    }
}
