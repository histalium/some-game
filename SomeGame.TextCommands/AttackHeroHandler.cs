using SomeGame.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.TextCommands
{
    internal class AttackHeroHandler : CliCommandHandler
    {
        private readonly Game _game;

        public AttackHeroHandler(Game game)
            : base(@"^attack hero with (?<minion>c\d+)$", new[] { "minion" })
        {
            _game = game;
        }

        public override IEnumerable<string> Handle(string[] args)
        {
            var minion = args[0];
            try
            {
                _game.CurrentPlayer.AttackHero(minion);
                return Array.Empty<string>();
            }
            catch (MinionNotFoundException)
            {
                return new[] { "Minion not found on the field" };
            }
            catch (MinionNotActiveException)
            {
                return new[] { "Minion is not active" };
            }
        }
    }
}
