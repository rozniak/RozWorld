/**
 * RozWorld.COMFY.ComfyContent -- RozWorld COMFY Content Manager
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using RozWorld.COMFY.Definition;

using System.Collections.Generic;


namespace RozWorld.COMFY
{
    public class ComfyContent
    {
        public Dictionary<string, string> Textures = new Dictionary<string, string>();

        public PlayerPawnDefinition PlayerPawn = new PlayerPawnDefinition();

        public Dictionary<string, InformationDefinition> InformationDefinitions = new Dictionary<string, InformationDefinition>();
        public Dictionary<string, string> LanguageDefinitions = new Dictionary<string, string>();
        public Dictionary<string, ItemDefinition> ItemDefinitions = new Dictionary<string, ItemDefinition>();
        public Dictionary<string, PetDefinition> PetDefinitions = new Dictionary<string, PetDefinition>();
        public Dictionary<string, SoundDefinition> SoundDefinitions = new Dictionary<string, SoundDefinition>();
        public Dictionary<string, ThingDefinition> ThingDefinitions = new Dictionary<string, ThingDefinition>();
        public Dictionary<string, TileFloorDefinition> TileFloorDefinitions = new Dictionary<string, TileFloorDefinition>();
        public Dictionary<string, TileWallDefinition> TileWallDefinitions = new Dictionary<string, TileWallDefinition>();
    }
}
