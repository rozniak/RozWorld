/**
 * RozWorld.WorldPoint -- RozWorld World Point
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using OpenGL;


namespace RozWorld
{
    public struct WorldPoint
    {
        public Vector3 Position;
        public string World;


        public WorldPoint(string world, Vector3 position)
        {
            World = world;
            Position = position;
        }
    }
}
