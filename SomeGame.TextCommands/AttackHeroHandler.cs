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
        private readonly PlayerGate _gate;

        public AttackHeroHandler(PlayerGate gate)
            : base(@"^attack hero with (?<minion>c\d+)$", new[] { "minion" })
        {
            _gate = gate;
        }

        public override IEnumerable<string> Handle(string[] args)
        {
            var minion = args[0];
            try
            {
                _gate.AttackHero(minion);
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
            catch (NotCurrentPlayerException)
            {
                return new[] { "You are not the current player" };
            }
        }
    }
}
