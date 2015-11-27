/**
 * RozWorld.COMFY.Definition.ThingDefinition -- RozWorld COMFY Thing Definition
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using RozWorld.Item;

using System.Collections.Generic;


namespace RozWorld.COMFY.Definition
{
    public class ThingDefinition
    {
        public string LanguageName;
        public string Texture;
        public NatureType Nature;
        public ItemStack Drops;
        public int Health;
        private List<string> Flags = new List<string>();


        /// <summary>
        /// Sets a flag state from inside of this definition.
        /// </summary>
        /// <param name="flag">The flag to set.</param>
        public void SetFlag(string flag)
        {
            if (!Flags.Contains(flag.ToUpper()))
            {
                Flags.Add(flag.ToUpper());
            }
        }


        /// <summary>
        /// Removes a flag from inside of this definition.
        /// </summary>
        /// <param name="flag">The flag to remove.</param>
        public void RemoveFlag(string flag)
        {
            if (Flags.Contains(flag.ToUpper()))
            {
                Flags.Remove(flag.ToUpper());
            }
        }


        /// <summary>
        /// Gets all flags from inside of this definition.
        /// </summary>
        /// <returns>The array of flags </returns>
        public string[] GetFlags()
        {
            return Flags.ToArray();
        }
    }
}
