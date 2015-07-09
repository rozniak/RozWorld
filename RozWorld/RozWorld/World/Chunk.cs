//
// RozWorld.World.Chunk -- RozWorld World Chunk
//
// This source-code is part of the RozWorld project by rozza of Oddmatics:
// <<http://www.oddmatics.co.uk>>
// <<http://www.oddmatics.co.uk/projects/rozworld>>
//
// Sharing, editing and general licence term information can be found inside of the "sup.txt" file that should be located in the root of this project's directory structure.
//

using System;

namespace RozWorld.World
{
    public class Chunk
    {
        // data structure for a chunk

        private Tile[,] Tiles = new Tile[16, 16];
        public readonly int X;
        public readonly int Y;


        public Chunk(int x, int y)
        {
            // init the chunk here
        }
    }
}
