/**
 * RozWorld.World.World -- RozWorld World
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System;
using System.Collections.Generic;
using System.IO;

using RozWorld.Entity;
using RozWorld.Entity.Player;
using RozWorld.IO;
using RozWorld.Network;

namespace RozWorld.World
{
    public class World
    {
        public readonly string Name;
        private List<Chunk> Chunks = new List<Chunk>();

        private Position _SpawnPoint;
        public Position SpawnPoint
        {
            get
            {
                return this._SpawnPoint;
            }

            set
            {
                this._SpawnPoint = new Position(value.LocalX, value.LocalY, value.ChunkX, value.ChunkY);
            }
        }

        private RozServer ParentServer;


        public World(RozServer parentServer, string name)
        {
            ParentServer = parentServer;
            Name = RozEncoding.StripSpecialCharacters(name, StripType.Both);

            // Check if the world exists on the server or not.

            if (File.Exists(ParentServer.SaveDirectory + "\\" + Name + "\\world.dat"))
            {
                // Get the file stuff here
            }
            else
            {
                // File doesn't exist, make a new world
            }
        }


        /// <summary>
        /// Gets all player instances of players located inside this world.
        /// </summary>
        /// <returns>The instances of all players located inside this world.</returns>
        public ServerPlayerHandler[] GetPlayers()
        {
            List<ServerPlayerHandler> players = new List<ServerPlayerHandler>();

            foreach (ServerPlayerHandler player in ParentServer.GetPlayers())
            {
                if (player.GetWorldName() == Name)
                {
                    players.Add(player);
                }
            }

            return players.ToArray();
        }


        /// <summary>
        /// Gets the number of players located inside this world.
        /// </summary>
        /// <returns>The number of players located inside this world.</returns>
        public int GetPlayerCount()
        {
            return GetPlayers().Length;
        }


        /// <summary>
        /// Gets the names of all players located inside this world.
        /// </summary>
        /// <returns>The names of all players located inside this world.</returns>
        public string[] GetPlayerNames()
        {
            List<string> playerNames = new List<string>();

            foreach (ServerPlayerHandler player in GetPlayers())
            {
                playerNames.Add(player.Nickname);
            }

            return playerNames.ToArray();
        }


        /// <summary>
        /// Saves the current chunk data inside this world to the disk.
        /// </summary>
        public void Save()
        {
            if (!Directory.Exists(ParentServer.SaveDirectory + "\\" + Name))
            {
                Directory.CreateDirectory(ParentServer.SaveDirectory + "\\" + Name);
            }

            // TODO: add chunk saving here
        }


        /// <summary>
        /// Gets the chunk instance at the specified coordinates.
        /// </summary>
        /// <param name="x">The x coordinate of the chunk.</param>
        /// <param name="y">The y coordinate of the chunk.</param>
        /// <returns>The chunk instance if it exists, null otherwise.</returns>
        public Chunk GetChunkAt(int x, int y)
        {
            Chunk chunk = null;
            int i = 0;

            do
            {
                if (Chunks[i].X == x && Chunks[i].Y == y)
                {
                    chunk = Chunks[i];
                }

                i++;
            } while (chunk == null && i <= Chunks.Count - 1);

            return chunk;
        }


        /// <summary>
        /// Attempts to load the chunk at the specified coordinates into the world.
        /// </summary>
        /// <param name="x">The x coordinate of the chunk.</param>
        /// <param name="y">The y coordinate of the chunk.</param>
        /// <returns>Whether the chunk was successfully loaded or not.</returns>
        public bool LoadChunk(int x, int y)
        {
            // TODO: load the chunk
            return true;
        }


        /// <summary>
        /// Creates a new chunk at the specified coordinates and loads it.
        /// </summary>
        /// <param name="x">The x coordinate of the chunk.</param>
        /// <param name="y">The y coordinate of the chunk.</param>
        /// <param name="loadChunk">Whether the chunk should be loaded once it is created.</param>
        /// <returns>Whether the chunk was successfully created or not.</returns>
        public bool CreateChunk(int x, int y, bool loadChunk = true)
        {
            // TODO: create the chunk
            return true;
        }
    }
}
