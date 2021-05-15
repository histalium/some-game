using SomeGame.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.TextCommands
{
    internal class ShowHandHandler : CliCommandHandler
    {
        private readonly Game _game;

        public ShowHandHandler(Game game)
            : base("^show hand$", Array.Empty<string>())
        {
            _game = game;
        }

        public override IEnumerable<string> Handle(string[] args)
        {
            foreach (var resource in _game.CurrentPlayer.HandResources)
            {
                yield return $"{resource.Resource.Id}: {resource.Amount}";
            }

            foreach (var card in _game.CurrentPlayer.Hand)
            {
                if (card.Card is ResourceCard resourceCard)
                {
                    yield return $"{card.Id,-4} {card.Card.Name} ({string.Join(", ", resourceCard.Resources.Select(Utilities.CostText))})";
                }
                else if (card.Card is MinionCard minionCard)
                {
                    yield return $"{card.Id,-4} {card.Card.Name} (hp: {minionCard.Health}, atk: {minionCard.Attack})";
                }
                else
                {
                    yield return $"{card.Id,-4} {card.Card.Name}";
                }
            }
        }
    }
}
