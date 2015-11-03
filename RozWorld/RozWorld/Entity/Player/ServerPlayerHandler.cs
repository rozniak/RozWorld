/**
 * RozWorld.Entity.Player.ServerPlayerHandler -- RozWorld Server Player Handler
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using RozWorld.Item;
using RozWorld.IO;
using RozWorld.Network.Chat;
using RozWorld.World;
using RozWorld.Network;

using System;
using System.Net;
using System.Collections.Generic;
using System.IO;


namespace RozWorld.Entity.Player
{
    public class ServerPlayerHandler
    {
        public readonly string Nickname = "";

        public string DisplayName
        {
            get;
            private set;
        }

        public bool Muted;

        public readonly string AuthToken = "";
        public readonly IPAddress AuthIP;

        private string _Group;
        public string Group
        {
            get
            {
                return this._Group;
            }

            set 
            {
                if (this.ParentServer.ServerConfiguration.PermissionGroups.ContainsKey(value)) {
                    this._Group = value;
                } 
            }
        }

        public Inventory PlayerInventory
        {
            get;
            private set;
        }

        public uint Health
        {
            get;
            private set;
        }

        public uint MaxHealth
        {
            get;
            private set;
        }

        public bool IsAlive
        {
            get;
            private set;
        }

        public uint Level
        {
            get;
            private set;
        }

        public uint SkillPoints;

        public ulong Experience
        {
            get;
            private set;
        }

        public ulong RequiredExperience
        {
            get;
            private set;
        }

        public Position Position
        {
            get;
            private set;
        }

        public Single Speed;

        private Queue<byte[]> PacketQueue;

        private RozServer ParentServer;


        public ServerPlayerHandler(RozServer parentServer, string nick, string group, IPAddress authIP, string displayName = "")
        {
            ParentServer = parentServer;
            Nickname = nick;
            Group = group;

            if (displayName != "")
            {
                DisplayName = displayName;
            }
            else
            {
                DisplayName = nick;
            }

            PlayerInventory = new Inventory(20);
            AuthIP = authIP;
            PacketQueue = new Queue<byte[]>();
            Muted = false;

            // get username.dat file
            if (!Directory.Exists(this.ParentServer.SaveDirectory + "\\players"))
            {
                Directory.CreateDirectory(this.ParentServer.SaveDirectory + "\\players");
                CreateDefaultUserData();
            }

            LoadUserData();
        }


        /// <summary>
        /// Loads the user data for this player from the disk.
        /// </summary>
        private void LoadUserData()
        {
            IList<string> userData = Files.GetTextFile(ParentServer.SaveDirectory + "\\players\\" + Nickname + ".dat");

            // TODO: Finish this
        }


        /// <summary>
        /// Creates a default user data file for this player.
        /// </summary>
        private void CreateDefaultUserData()
        {
            // TODO: Create the default user data
        }


        /// <summary>
        /// Sends a formatted chat message to this player.
        /// </summary>
        /// <param name="message">The chat message to send.</param>
        public void SendChatMessage(string message, string sender)
        {
            string prefix = ParentServer.ServerConfiguration.ChatFormat.Prefix;
            string suffix = ParentServer.ServerConfiguration.ChatFormat.Suffix;
            string groupPrefix = ParentServer.ServerConfiguration.PermissionGroups[Group].Prefix;
            string messageToSend = message;
            string chatMessage = ParentServer.ServerConfiguration.ChatFormat.MessageFormat;

            chatMessage = chatMessage.Replace("%prefix%", prefix);
            chatMessage = chatMessage.Replace("%grpre%", groupPrefix);
            chatMessage = chatMessage.Replace("%name%", sender);
            chatMessage = chatMessage.Replace("%suffix%", suffix);

            if (!ParentServer.ServerConfiguration.ChatFormat.AllowColours)
            {
                foreach (string colourCode in ChatColour.FromArray)
                {
                    messageToSend = messageToSend.Replace(colourCode, "");
                }
            }

            chatMessage = chatMessage.Replace("%message%", messageToSend);

            SendMessage(chatMessage);
        }


        /// <summary>
        /// Sends a chat message directly to this player.
        /// </summary>
        /// <param name="message">The chat message to send.</param>
        public void SendMessage(string message)
        {
            byte[] chatEncoded = RozEncoding.GetBytes(message);

            SendRawMessage(Packets.ConstructMessage(new byte[][] { chatEncoded }, Packets.SERVER_CHAT_MESSAGE_CODE));
        }


        /// <summary>
        /// Sends a raw message to this player.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public void SendRawMessage(byte[] message)
        {
            if (message.Length > 0)
            {
                PacketQueue.Enqueue(message);
            }            
        }


        /// <summary>
        /// Attempts to heal this player.
        /// </summary>
        /// <param name="amount">The amount to heal the player.</param>
        /// <returns>The amount the player was healed.</returns>
        public uint Heal(uint amount = uint.MaxValue)
        {
            uint healedAmount;

            if (Health + amount >= MaxHealth)
            {
                healedAmount = MaxHealth - Health;
                Health = MaxHealth;
            }
            else
            {
                healedAmount = amount;
                Health += amount;
            }

            return healedAmount;
        }


        /// <summary>
        /// Adds experience to this player.
        /// </summary>
        /// <param name="amount">The amount of experience to add.</param>
        public void AddExperience(ulong amount)
        {
            if (Experience + amount > RequiredExperience)
            {
                SetLevel(Level + 1);
                AddExperience(amount - RequiredExperience);
            }
            else if (Experience + amount == RequiredExperience)
            {
                SetLevel(Level + 1);
            }
            else
            {
                Experience += amount;
            }
        }


        /// <summary>
        /// Sets the level of this player.
        /// </summary>
        /// <param name="newLevel">The level to set the player to.</param>
        public void SetLevel(uint newLevel)
        {
            Experience = 0;
            RequiredExperience = (ulong)(((newLevel ^ 2) / Math.Sqrt((double)(newLevel + 2))) * 3.5);
            Level = newLevel;
        }


        /// <summary>
        /// Instantly kills off this player.
        /// </summary>
        /// <param name="message">The death message to send the chat.</param>
        public void Kill(string message = "")
        {
            if (IsAlive)
            {
                Health = 0;
                IsAlive = false;

                if (message == "")
                {
                    ParentServer.BroadcastMessage(DisplayName + " died.");
                }
                else
                {
                    ParentServer.BroadcastMessage(message);
                }
            }
        }


        /// <summary>
        /// Deals damage to this player.
        /// </summary>
        /// <param name="amount">The amount of damage to deal.</param>
        /// <param name="senderName">The identity of the causation of the damage.</param>
        /// <param name="damageMethod">The method in which this damage is being dealt.</param>
        /// <param name="damageThing">The identity of any specific thing used or dealing the damage.</param>
        /// <returns>Whether this player was killed by the damage.</returns>
        public bool DealDamage(uint amount, string senderName, DamageMethod damageMethod, string damageThing = "")
        {
            if (IsAlive)
            {
                if (Health - amount <= 0)
                {
                    switch (damageMethod)
                    {
                        case DamageMethod.Generic: Kill(DisplayName + " was killed by " + senderName + "."); break;
                        case DamageMethod.Explosion: Kill(DisplayName + " was killed in an explosion."); break;
                        case DamageMethod.Weapon: Kill(DisplayName + " was killed by " + senderName + "'s " + damageThing + "."); break;
                        case DamageMethod.Potion: Kill(DisplayName + " was killed by " + senderName + " using " + damageThing + "."); break;
                        case DamageMethod.Pet: Kill(DisplayName + " was killed by " + senderName + "'s pet " + damageThing + "."); break;
                    }

                    return true;
                }
                else
                {
                    Health -= amount;
                }
            }

            byte[] nickBytes = RozEncoding.GetBytes(Nickname);
            byte[] healthBytes = RozEncoding.GetBytes(Health.ToString());

            ParentServer.BroadcastRawMessage(Packets.ConstructMessage(new byte[][] { nickBytes, new byte[] { Packets.STAT_HEALTH_CODE }, healthBytes }, Packets.SERVER_STAT_UPDATE_CODE));

            return false;
        }


        /// <summary>
        /// Attempts to kick this player off the server.
        /// </summary>
        /// <returns>Whether this player was successfully kicked or not.</returns>
        public bool Kick()
        {
            return ParentServer.KickPlayer(Nickname);
        }


        /// <summary>
        /// Saves the user data of this player to the disk.
        /// </summary>
        public void Save()
        {
            // TODO: save the user data here
        }


        /// <summary>
        /// Attempts to teleport this player to another world.
        /// </summary>
        /// <param name="newWorld">The world to teleport the player to.</param>
        /// <returns>Whether this player was successfully teleported or not.</returns>
        public bool ChangeWorld(string newWorld)
        {
            if (ParentServer.WorldExists(newWorld))
            {
                Position = ParentServer.GetWorld(newWorld).SpawnPoint;

                SendRawMessage(Packets.ConstructMessage(Position.GetBytes(), Packets.SERVER_TELEPORT_CODE));                
                return true;
            }

            return false;
        }


        /// <summary>
        /// Attempts to teleport this player to another world.
        /// </summary>
        /// <param name="newPosition">The new world and coordinates to teleport this player to.</param>
        /// <returns>Whether this player was successfully teleported or not.</returns>
        public bool ChangeWorld(Position newPosition)
        {
            if (ParentServer.WorldExists(newPosition.LocalWorld.Name))
            {
                Position = newPosition;

                SendRawMessage(Packets.ConstructMessage(Position.GetBytes(), Packets.SERVER_TELEPORT_CODE));
                return true;
            }

            return false;
        }


        /// <summary>
        /// Teleports this player to a new set of coordinates.
        /// </summary>
        /// <param name="localX">The local x coordinate.</param>
        /// <param name="localY">The local y coordinate.</param>
        /// <param name="chunkX">The chunk x coordinate.</param>
        /// <param name="chunkY">The chunk y coordinate.</param>
        /// <returns>Whether this player was successfully teleported or not.</returns>
        public bool Teleport(double localX, double localY, int chunkX, int chunkY)
        {
            Position.LocalX = localX;
            Position.LocalY = localY;
            Position.ChunkX = chunkX;
            Position.ChunkY = chunkY;

            SendRawMessage(Packets.ConstructMessage(Position.GetBytes(), Packets.SERVER_TELEPORT_CODE));
            return true;
        }
    }
}
