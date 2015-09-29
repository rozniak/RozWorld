/**
 * RozWorld.BasicObject.WorldGenerator -- RozWorld World Generator Base Class
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using RozWorld.World;

namespace RozWorld.BasicObject
{
    public abstract class WorldGenerator
    {
        /// <summary>
        /// Gets the internal name of this generator, to be used when getting this generator.
        /// </summary>
        public abstract string InternalName { get; }


        /// <summary>
        /// Generates a Chunk in the world, using the instructions of the surrounding area to support it.
        /// </summary>
        /// <param name="instructions">The instructions from surrounding Chunks.</param>
        /// <returns>A new Chunk, with its contents generated alongside the given instructions.</returns>
        public abstract Chunk MakeChunk(string instructions);
    }
}
