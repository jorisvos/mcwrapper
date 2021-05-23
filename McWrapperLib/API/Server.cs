using System;

namespace McWrapperLib.API
{
    /// <summary>
    /// Server class that contains Methods for different server related things
    /// </summary>
    public class Server
    {
        private readonly Guid _id;

        public Server(Guid id)
            => _id = id;
        public Server(string id)
            => _id = Guid.Parse(id);

        /// <summary>
        /// Broadcasts a json tellraw message to all players
        /// </summary>
        /// <param name="jsonMessage">Json message for tellraw command</param>
        public void BroadcastMessage(string jsonMessage) 
            => RunCommand("tellraw @a "+jsonMessage);

        /// <summary>
        /// Broadcasts a message to all Players
        /// </summary>
        /// <param name="message">message to broadcast</param>
        /// <param name="color">Color of message</param>
        public void BroadcastMessage(string message, string color) 
            => RunCommand("tellraw @a {\"text\":\""+message+"\",\"color\":\""+color+"\"}");

        /// <summary>
        /// Runs a command
        /// </summary>
        /// <param name="command">The command that should be run</param>
        /// <example>RunCommand("kick Notch");</example>
        public void RunCommand(string command)
            => ServerManager.Instance.ExecuteCommand(_id, command);
    }
}