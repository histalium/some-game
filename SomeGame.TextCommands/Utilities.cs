using SomeGame.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.TextCommands
{
    internal static class Utilities
    {
        internal static string CostText(ResourceAmount cost)
        {
            return $"{cost.Amount} {cost.Resource.Id}";
        }

        internal static IEnumerable<string> PrintField(Player player)
        {
            foreach (var minion in player.Field)
            {
                var activeText = minion.Active ? "active" : "not active";
                yield return $"{minion.CardId,-4} hp: {minion.Health}, atk: {minion.Attack} ({activeText})";
            }
        }
    }
}
