namespace McWrapperLib.Events
{
    public class PlayerPositionEventArgs : PlayerEventArgs
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public PlayerPositionEventArgs(string player, int x, int y, int z) : base(player)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}