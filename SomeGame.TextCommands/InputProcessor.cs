using SomeGame.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.TextCommands
{
    public class InputProcessor
    {
        private readonly List<CliCommandHandler> _handlers;

        public InputProcessor(PlayerGate gate, Game game)
        {
            _handlers = new List<CliCommandHandler>
            {
                new AddToFieldHandler(gate),
                new AttackHeroHandler(gate),
                new AttackMinionHandler(gate),
                new BuyCardHandler(gate),
                new EndTurnHandler(gate),
                new ShowFieldHandler(gate),
                new ShowFieldRivalHandler(game),
                new ShowHandHandler(gate),
                new ShowHeroHandler(gate),
                new ShowHeroRivalHandler(gate),
                new ShowMarketHandler(game),
            };
        }

        public IEnumerable<string> Process(string input)
        {
            var (match, handler) = _handlers
                .Select(t => (Match: t.CommandPattern.Match(input), Handler: t))
                .Where(t => t.Match.Success)
                .FirstOrDefault();

            if (match?.Success == true)
            {
                var args = handler.ArgNames
                    .Select(t => match.Groups[t].Value)
                    .ToArray();

                return handler.Handle(args);
            }
            else
            {
                return new[] { "invalid command" };
            }
        }
    }
}
