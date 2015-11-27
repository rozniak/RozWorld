/**
 * RozWorld.Network.RozServer -- RozWorld Server
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

/**
 * [ NOTICE! ]
 * 
 * This code is subject to massive changes, do not expect a lot of the established code in here to remain as is!
 */

using RozWorld.Entity;
using RozWorld.Entity.Player;
using RozWorld.Network.Chat;
using RozWorld.IO;
using RozWorld.World;

using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Net;
using System.Net.Sockets;


namespace RozWorld.Network
{
    public class RozServer
    {
        // port 8420
        public const double SERVER_VERSION = 0.01;
        public const string SERVER_IMPLEMENTATION = "RozWorld Vanilla";

        // Tell the game if this is live or not
        public readonly bool IsLive;

        // Network client and ports
        private readonly int NetPort;
        private UdpClient NetClient;
        private Timer NetDaemon;

        private ServerPacketHandler PacketHandler;

        private List<JoiningClient> JoiningPlayers;
        private List<ServerPlayerHandler> Players;

        public ServerOptions ServerConfiguration;
        public readonly string SaveDirectory;

        private List<World.World> Worlds;


        public RozServer(bool useConfigs, bool onlineMode, List<string> serverOptions = null)
        {
            ServerConfiguration = new ServerOptions(this, onlineMode);

            if (useConfigs)
            {
                if (serverOptions == null)
                {
                    LoadConfigs();
                }
                else
                {
                    if (serverOptions.Count > 0)
                    {
                        foreach (string line in serverOptions)
                        {
                            LoadConfigFromStrings(Files.SplitFirstInstance(": ", line));
                        }
                    }
                    else
                    {
                        LoadDefaultConfigs();
                    }
                }
            }
            else
            {
                LoadDefaultConfigs();
            }

            bool successfulPort = false;

            do
            {
                // Obsolete code
                //NetPort = Globals.Rand.Next(49152, 65536);

                try
                {
                    NetClient = new UdpClient(NetPort);
                    successfulPort = true;
                }
                catch
                {
                    // Do nothing
                }
            } while (!successfulPort);

            SaveDirectory = Environment.CurrentDirectory;

            IsLive = true;

            NetDaemon = new Timer(1);
            NetDaemon.Elapsed += new ElapsedEventHandler(NetDaemonTickEvent);
        }

        public RozServer(int port, bool useConfigs, bool onlineMode, List<string> serverOptions = null)
        {
            NetPort = port;

            ServerConfiguration = new ServerOptions(this, onlineMode);

            if (useConfigs)
            {
                if (serverOptions == null)
                {
                    LoadConfigs();
                }
                else
                {
                    if (serverOptions.Count > 0)
                    {
                        foreach (string line in serverOptions)
                        {
                            LoadConfigFromStrings(Files.SplitFirstInstance(": ", line));
                        }
                    }
                    else
                    {
                        LoadDefaultConfigs();
                    }
                }
            }
            else
            {
               LoadDefaultConfigs();
            }

            try
            {
                NetClient = new UdpClient(NetPort);
                SaveDirectory = Environment.CurrentDirectory;
                IsLive = true;
            }
            catch
            {
                IsLive = false;
            }
        }


        /// <summary>
        /// Event handler for the NetDaemon.Elapsed timer event.
        /// </summary>
        void NetDaemonTickEvent(object sender, ElapsedEventArgs e)
        {
            try
            {
                IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, NetPort);

                byte[] receivedPacket = NetClient.Receive(ref clientEndPoint);
                byte[] responsePacket = PacketHandler.HandlePacket(receivedPacket, clientEndPoint.Address);

                NetClient.Send(responsePacket, responsePacket.Length, clientEndPoint);
            }
            catch
            {
                // Do nothing
            }
        }


        /// <summary>
        /// Loads the server configuration file from the disk.
        /// </summary>
        private void LoadConfigs()
        {
            IList<string> configContents = Files.GetTextFile(Environment.CurrentDirectory + "\\server.conf");

            if (configContents.Count > 0)
            {
                foreach (string line in configContents)
                {
                    LoadConfigFromStrings(Files.SplitFirstInstance(": ", line));
                }
            }
            else
            {
                CreateDefaultConfigs();
                LoadConfigs();
            }
        }


        /// <summary>
        /// Loads the configuration setting from its identifier and attribute.
        /// </summary>
        /// <param name="configString">The two length array containing the split identifier and attribute of the setting.</param>
        private void LoadConfigFromStrings(string[] configString)
        {
            if (configString[0] != "" && configString[1] != "" && configString.Length == 2)
            {
                switch (configString[0].ToLower())
                {
                    case "difficulty":
                        int difficultyValue = 0;
                        bool successfulParse = int.TryParse(configString[1], out difficultyValue);

                        if (successfulParse)
                        {
                            ServerConfiguration.Difficulty = difficultyValue;
                        }

                        break;

                    case "motd":
                        ServerConfiguration.MessageOfTheDay = configString[1];
                        break;

                    case "max-clients":
                        int maxClientsValue = 0;
                        bool _successfulParse = int.TryParse(configString[1], out maxClientsValue);

                        if (_successfulParse)
                        {
                            ServerConfiguration.MaxClients = maxClientsValue;
                        }

                        break;

                    case "message-format":
                        ServerConfiguration.ChatFormat.MessageFormat = configString[1];
                        break;

                    case "allow-colours":
                        if (configString[1].ToUpper() == "TRUE")
                        {
                            ServerConfiguration.ChatFormat.AllowColours = true;
                        }
                        else if (configString[1].ToUpper() == "FALSE")
                        {
                            ServerConfiguration.ChatFormat.AllowColours = false;
                        }

                        break;

                    case "chat-prefix":
                        ServerConfiguration.ChatFormat.Prefix = configString[1];
                        break;

                    case "chat-suffix":
                        ServerConfiguration.ChatFormat.Suffix = configString[1];
                        break;

                    case "lvl-skillpoints":
                        ushort skillPointsReward = 0;
                        bool __successfulParse = ushort.TryParse(configString[1], out skillPointsReward);

                        if (__successfulParse)
                        {
                            ServerConfiguration.SkillpointReward = skillPointsReward;
                        }

                        break;
                }
            }
        }


        /// <summary>
        /// Loads the default server configuration.
        /// </summary>
        private void LoadDefaultConfigs()
        {
            ServerConfiguration.MaxClients = 10;
            ServerConfiguration.MessageOfTheDay = "A RozWorld Server";
            ServerConfiguration.Difficulty = 2;
        }


        /// <summary>
        /// Creates a default configuration file for the server.
        /// </summary>
        private void CreateDefaultConfigs()
        {
            // TODO: update this!
            Files.PutTextFile(Environment.CurrentDirectory + "\\server.conf", new string[] { "difficulty: 2", "motd: A RozWorld Server", "max-clients: 10", "message-format: %prefix%%grpre%%name%%suffix%%message%", "allow-colours: true", "chat-prefix: <", "chat-suffix: >" });
        }


        /// <summary>
        /// Loads all worlds found in the server's directory.
        /// </summary>
        private void LoadWorlds()
        {
            // load worlds here
        }


        /// <summary>
        /// Attempt to load the world with the specified name from the disk.
        /// </summary>
        /// <param name="worldName">The name of the world to load.</param>
        /// <returns>Whether the world was successfully loaded or not.</returns>
        public bool LoadWorld(string worldName)
        {
            if (WorldExists(worldName))
            {
                // TODO: load the world
                return true;
            }

            return false;
        }


        /// <summary>
        /// Creates a new world for the server and loads it.
        /// </summary>
        /// <param name="worldName">The name of the world to create, this will also be used for the directory name.</param>
        /// <param name="loadWorld">Whether the world should be loaded once it is created.</param>
        /// <returns>Whether the world was successfully created or not.</returns>
        public bool CreateWorld(string worldName, bool loadWorld = true)
        {
            if (!WorldExists(worldName))
            {
                World.World world = new World.World(this, worldName);

                return true;
            }

            return false;
        }


        /// <summary>
        /// Checks if the world with the specified name exists in the server's loaded worlds.
        /// </summary>
        /// <param name="worldName">The name of the world to check.</param>
        /// <returns>Whether the world exists in the loaded worlds or not.</returns>
        public bool WorldExists(string worldName)
        {
            bool worldExists = false;
            int i = 0;

            do
            {
                if (worldName == Worlds[i].Name)
                {
                    worldExists = true;
                }

                i++;
            } while (!worldExists && i <= Worlds.Count - 1);

            return worldExists;
        }


        /// <summary>
        /// Attempts to get the world of the specified name.
        /// </summary>
        /// <param name="worldName">The name of the world to get.</param>
        /// <returns>The world instance if it exists, null otherwise.</returns>
        public World.World GetWorld(string worldName)
        {
            World.World world = null;

            if (WorldExists(worldName))
            {
                int i = 0;

                do
                {
                    if (Worlds[i].Name == worldName)
                    {
                        world = Worlds[i];
                    }

                    i++;
                } while (world == null && i <= Worlds.Count - 1);
            }

            return world;
        }


        /// <summary>
        /// Gets all instances of the worlds currently loaded on the server.
        /// </summary>
        /// <returns>The instances of all worlds currently loaded on the server.</returns>
        public World.World[] GetWorlds()
        {
            return Worlds.ToArray();
        }


        /// <summary>
        /// Attempts to kick the player with the specified name off the server.
        /// </summary>
        /// <param name="nickName">The nickname of the player to kick.</param>
        /// <returns>Whether the player was successfully kicked or not.</returns>
        public bool KickPlayer(string nickName)
        {
            // TODO: actually handle kicking a player
            return true;
        }


        /// <summary>
        /// Sends a raw message to all players on the server.
        /// </summary>
        /// <param name="message">The message to send all players on the server.</param>
        public void BroadcastRawMessage(byte[] message, string worldName = "*")
        {
            for (int i = 0; i <= Players.Count - 1; i++)
            {
                Players[i].SendRawMessage(message);
            }
        }


        /// <summary>
        /// Sends a chat message to all players on the server.
        /// </summary>
        /// <param name="message">The chat message to send all players on the server.</param>
        public void BroadcastMessage(string message, string worldName = "*")
        {
            if (worldName == "*")
            {
                foreach (ServerPlayerHandler player in Players)
                {
                    player.SendMessage(message);
                }
            }
            else
            {
                World.World world = GetWorld(worldName);

                if (world != null)
                {
                    foreach (ServerPlayerHandler player in world.GetPlayers())
                    {
                        player.SendMessage(message);
                    }
                }
            }
        }


        /// <summary>
        /// Sends a formatted chat message to all players on the server.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sender"></param>
        /// <param name="worldName"></param>
        public void BroadcastChatMessage(string message, string sender, string worldName = "*")
        {
            if (worldName == "*")
            {
                foreach (ServerPlayerHandler player in Players)
                {
                    player.SendChatMessage(message, sender);
                }
            }
            else
            {
                World.World world = GetWorld(worldName);

                if (world != null)
                {
                    foreach (ServerPlayerHandler player in world.GetPlayers())
                    {
                        player.SendChatMessage(message, sender);
                    }
                }
            }
        }


        /// <summary>
        /// Gets the number of players connected to the server.
        /// </summary>
        /// <returns>The number of players connected to the server.</returns>
        public int GetPlayerCount()
        {
            return Players.Count;
        }


        /// <summary>
        /// Gets the names of all players connected to the server.
        /// </summary>
        /// <returns>The names of all players connected to the server.</returns>
        public string[] GetPlayerNames()
        {
            List<string> playerNames = new List<string>();

            foreach (ServerPlayerHandler player in Players)
            {
                playerNames.Add(player.Nickname);
            }

            return playerNames.ToArray();
        }


        /// <summary>
        /// Attempts to get the individual player with the specified nickname from the server.
        /// </summary>
        /// <param name="nickName">The nickname of the player to get.</param>
        /// <returns>The player instance if they exist, null otherwise.</returns>
        public ServerPlayerHandler GetPlayer(string nickName)
        {
            ServerPlayerHandler player = null;
            int i = 0;

            do
            {
                if (Players[i].Nickname == nickName)
                {
                    player = Players[i];
                }

                i++;
            } while (player == null && i <= Players.Count - 1);

            return player;
        }


        /// <summary>
        /// Checks whether the player with the specified nickname is connected to the server.
        /// </summary>
        /// <param name="nickName">The nickname of the player to find.</param>
        /// <returns>Whether the player is connected or not.</returns>
        public bool PlayerIsOnline(string nickName)
        {
            return GetPlayer(nickName) != null;
        }


        /// <summary>
        /// Gets all instances of the players connected to the server.
        /// </summary>
        /// <returns>The instances of all players connected to the server.</returns>
        public ServerPlayerHandler[] GetPlayers()
        {
            return Players.ToArray();
        }
    }
}
