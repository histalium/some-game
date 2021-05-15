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
                new AddToFieldHandler(game),
                new AttackHeroHandler(game),
                new AttackMinionHandler(game),
                new BuyCardHandler(game),
                new EndTurnHandler(game),
                new ShowFieldHandler(game),
                new ShowFieldRivalHandler(game),
                new ShowHandHandler(game),
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
