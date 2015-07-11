/**
 * RozWorld.World.Chunk -- RozWorld World Chunk
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

/**
 * [ NOTICE! ]
 * 
 * This code is preliminary, and will be worked on later...
 */

using System;

namespace RozWorld.World
{
    public class Chunk
    {
        private Tile[,] Tiles = new Tile[16, 16];
        public readonly int X;
        public readonly int Y;


        public Chunk(int x, int y)
        {
            // TODO: Chunk initialisation
        }
    }
}
