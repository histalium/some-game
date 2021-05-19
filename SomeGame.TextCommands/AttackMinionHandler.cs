using SomeGame.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.TextCommands
{
    internal class AttackMinionHandler : CliCommandHandler
    {
        private readonly PlayerGate _gate;

        public AttackMinionHandler(PlayerGate gate)
            : base(@"^attack (?<minionRival>c\d+) with (?<minion>c\d+)", new[] { "minion", "minionRival" })
        {
            _gate = gate;
        }

        public override IEnumerable<string> Handle(string[] args)
        {
            var minion = args[0];
            var minionRival = args[1];
            try
            {
                _gate.AttackMinion(minion, minionRival);
                return Array.Empty<string>();
            }
            catch (MinionNotFoundException ex)
            {
                if (ex.CardId.Equals(minion))
                {
                    return new[] { "Minion not found on your field" };
                }
                else
                {
                    return new[] { "Minion not found on your rivals field" };
                }
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
