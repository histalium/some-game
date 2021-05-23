using System;

namespace SomeGame.Logic
{
    public class CurrentPlayerChangedEventArgs : EventArgs
    {
        public CurrentPlayerChangedEventArgs(string currentPlayerName)
        {
            CurrentPlayerName = currentPlayerName;
        }

        public string CurrentPlayerName { get; }
    }
}
