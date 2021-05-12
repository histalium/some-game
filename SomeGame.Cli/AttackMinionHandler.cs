using SomeGame.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Cli
{
    internal class AttackMinionHandler : CliCommandHandler
    {
        private readonly Game _game;

        public AttackMinionHandler(Game game)
            : base(@"^attack (?<minionRival>c\d+) with (?<minion>c\d+)", new[] { "minion", "minionRival" })
        {
            _game = game;
        }

        public override IEnumerable<string> Handle(string[] args)
        {
            var minion = args[0];
            var minionRival = args[1];
            try
            {
                _game.CurrentPlayer.AttackMinion(minion, minionRival);
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
        }
    }
}
