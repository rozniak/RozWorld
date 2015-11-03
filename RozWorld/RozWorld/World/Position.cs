/**
 * RozWorld.World.Position -- RozWorld Position
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using RozWorld.IO;

using System;


namespace RozWorld.World
{
    public class Position
    {
        public World LocalWorld;
        public int ChunkX;
        public int ChunkY;
        public double LocalX;
        public double LocalY;


        public Position()
        {
            ChunkX = 0;
            ChunkY = 0;
            LocalX = 0;
            LocalY = 0;
        }


        public Position(double localX, double localY, int chunkX, int chunkY, World localWorld)
        {
            LocalWorld = localWorld;
            ChunkX = chunkX;
            ChunkY = chunkY;
            LocalX = localX;
            LocalY = localY;
        }


        /// <summary>
        /// Gets this position data into an array of byte arrays.
        /// </summary>
        /// <returns>This position data in an array of byte arrays.</returns>
        public byte[][] GetBytes()
        {
            return new byte[][]
            {
                RozEncoding.GetBytes(LocalWorld.Name),
                RozEncoding.GetBytes(LocalX.ToString()),
                RozEncoding.GetBytes(LocalY.ToString()),
                RozEncoding.GetBytes(ChunkX.ToString()),
                RozEncoding.GetBytes(ChunkY.ToString())
            };
        }
    }
}
