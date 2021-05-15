﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Logic
{
    public class PlayerGate
    {
        private readonly Player _player;
        private readonly Player _rival;

        internal PlayerGate(Player player, Player rival)
        {
            _player = player;
            _rival = rival;
        }

        public string GetPlayerName()
        {
            return _player.Name;
        }

        public int GetPlayerHealth()
        {
            return _player.Health;
        }

        public string GetRivalName()
        {
            return _rival.Name;
        }

        public int GetRivalHealth()
        {
            return _rival.Health;
        }

        public IReadOnlyCollection<ResourceAmount> GetHandResources()
        {
            return _player.HandResources;
        }

        public IReadOnlyCollection<GameCard> GetHand()
        {
            return _player.Hand;
        }
    }
}
