﻿using SomeGame.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Cli
{
    internal class ShowFieldRivalHandler : CliCommandHandler
    {
        private readonly Game _game;

        public ShowFieldRivalHandler(Game game)
            : base("^show field rival$", Array.Empty<string>())
        {
            _game = game;
        }

        public override IEnumerable<string> Handle(string[] args)
        {
            var rival = _game.CurrentPlayer == _game.Player1 ? _game.Player2 : _game.Player1;
            return Program.PrintField(rival);
        }
    }
}