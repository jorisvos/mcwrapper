using System;

namespace McWrapperLib.API
{
    /// <summary>
    /// Player class that contains Methods for different player related things
    /// </summary>
    public class Player
    {
        private readonly Guid _id;
        
        public Player(Guid id)
            => _id = id;
        public Player(string id)
            => _id = Guid.Parse(id);

        /// <summary>
        /// Sends a json message to the named player
        /// </summary>
        /// <param name="name">_name of the player</param>
        /// <param name="jsonMessage">Json message</param>
        public void SendMessageTo(string name, string jsonMessage) 
            => ServerManager.Instance.ExecuteCommand(_id, "tellraw "+name+" "+jsonMessage);
        
        /// <summary>
        /// Sends a message with the specific color to the named player
        /// </summary>
        /// <param name="name">name of the player</param>
        /// <param name="message">Message</param>
        /// <param name="color">Color</param>
        public void SendMessageTo(string name, string message, string color) 
            => ServerManager.Instance.ExecuteCommand(_id, "tellraw "+name+" {\"text\":\""+message+"\",\"color\":\""+color+"\"}");

        /// <summary>
        /// Calls onPosition with the new Player position
        /// </summary>
        public void RefreshPosition(string name)
            => ServerManager.Instance.ExecuteCommand(_id, "execute at "+name+" run tp "+name+" ~ ~ ~");
    }
}