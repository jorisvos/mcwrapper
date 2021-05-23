namespace McWrapperLib.Events
{
    public class PlayerChatEventArgs : PlayerEventArgs
    {
        public string Message { get; }

        public PlayerChatEventArgs(string player, string message) : base(player) 
            => Message = message;
    }
}