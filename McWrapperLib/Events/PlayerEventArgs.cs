using System;

namespace McWrapperLib.Events
{
    public abstract class PlayerEventArgs : EventArgs
    {
        public string Player { get; }

        protected PlayerEventArgs(string player) 
            => Player = player;
    }
}