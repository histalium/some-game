using System;
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

        public event EventHandler<CurrentPlayerChangedEventArgs> CurrentPlayerChanged;

        internal PlayerGate(Player player, Player rival)
        {
            _player = player;
            _rival = rival;

            _player.TurnStarted += PlayerTurnStarted;
            _rival.TurnStarted += RivalTurnStarted;
        }

        private void RivalTurnStarted(object sender, EventArgs e)
        {
            CurrentPlayerChanged?.Invoke(this, new CurrentPlayerChangedEventArgs(_rival.Name));
        }

        private void PlayerTurnStarted(object sender, EventArgs e)
        {
            CurrentPlayerChanged?.Invoke(this, new CurrentPlayerChangedEventArgs(_player.Name));
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

        public void AddMinionToField(string cardId)
        {
            _player.AddMinionToField(cardId);
        }

        public void AttackHero(string minion)
        {
            _player.AttackHero(minion);
        }

        public void AttackMinion(string minion, string minionRival)
        {
            _player.AttackMinion(minion, minionRival);
        }

        public void BuyCard(string cardId)
        {
            _player.BuyCard(cardId);
        }

        public string GetCurrentPlayerName()
        {
            if (_player.IsCurrentPlayer)
            {
                return _player.Name;
            }
            else
            {
                return _rival.Name;
            }
        }

        public void EndTurn()
        {
            _player.EndTurn();
        }
    }
}
