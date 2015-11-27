/**
 * RozWorld.COMFY.ComfyInterpreter -- RozWorld COMFY Definition Script Interpreter
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using RozWorld.IO;
using RozWorld.COMFY.Definition;
using RozWorld.Item;

using System;
using System.IO;
using System.Collections.Generic;


namespace RozWorld.COMFY
{
    public static class ComfyInterpreter
    {
        /// <summary>
        /// Load all COMFY script files inside of the local COMFY directory into a ComfyContent class.
        /// </summary>
        /// <returns>The resulting ComfyContent built from the COMFY script files.</returns>
        public static ComfyContent Load(string loadDirectory = "")
        {
            // If no load directory is specified or doesn't exist, assume loading the default local COMFY directory...
            string comfyDirectory = loadDirectory != "" && File.Exists(loadDirectory) ? loadDirectory : Files.ComfyDirectory;

            ComfyContent comfyContent = new ComfyContent();

            foreach (string file in Directory.GetFiles(comfyDirectory))
            {
                IList<string> fileContent = Files.GetTextFile(file);

                object currentDefinition = null;
                DefinitionType currentDefType = DefinitionType.Unknown;
                bool currentlyDefining = false;
                bool expectingOpeningBrace = false;
                string objectName = "";

                foreach (string line in fileContent)
                {
                    Tuple<ParsedObjectType, object>[] parsedContent = ParseLine(line);

                    if (parsedContent.Length == 0 || parsedContent[0].Item1 != ParsedObjectType.Word)
                    {
                        continue;
                    }

                    // Used to examine the first WORD of a line...
                    string firstWord = ((string)parsedContent[0].Item2).ToLower();

                    if (!currentlyDefining)
                    {
                        if (!expectingOpeningBrace)
                        {
                            if (parsedContent.Length == 1)
                            {
                                expectingOpeningBrace = true;

                                switch (firstWord)
                                {
                                    case "@info":
                                        currentDefType = DefinitionType.InformationDefinition;
                                        currentDefinition = new InformationDefinition();
                                        
                                        break;

                                    case "@languages":
                                        currentDefType = DefinitionType.LanguageDefinition;
                                        
                                        break;

                                    case "@textures":
                                        currentDefType = DefinitionType.TextureDefinition;
                                        
                                        break;

                                    case "@playerpawn":
                                        currentDefType = DefinitionType.PlayerPawnDefinition;
                                        currentDefinition = new PlayerPawnDefinition();
                                        
                                        break;

                                    default:
                                        // Interpreter error here
                                        expectingOpeningBrace = false;

                                        break;
                                }
                            }
                            else if (parsedContent.Length == 2 && parsedContent[1].Item1 == ParsedObjectType.Word)
                            {
                                switch (firstWord)
                                {
                                    case "sound":
                                        currentDefType = DefinitionType.SoundDefinition;
                                        currentDefinition = new SoundDefinition();
                                        break;

                                    case "tilefloor":
                                        currentDefType = DefinitionType.TileFloorDefinition;
                                        currentDefinition = new TileFloorDefinition();
                                        break;

                                    case "tilewall":
                                        currentDefType = DefinitionType.TileWallDefinition;
                                        currentDefinition = new TileWallDefinition();
                                        break;

                                    case "item":
                                        currentDefType = DefinitionType.ItemDefinition;
                                        currentDefinition = new ItemDefinition();
                                        break;

                                    case "thing":
                                        currentDefType = DefinitionType.ThingDefinition;
                                        currentDefinition = new ThingDefinition();
                                        break;

                                    case "pet":
                                        currentDefType = DefinitionType.PetDefinition;
                                        currentDefinition = new PetDefinition();
                                        break;

                                    default:
                                        // Interpreter error here

                                        break;
                                }
                                objectName = (string)parsedContent[1].Item2;
                                expectingOpeningBrace = true;
                            }
                        }
                        else
                        {
                            if (parsedContent.Length == 1 && firstWord == "{")   // OPENING DEFINITION '{'
                            {
                                expectingOpeningBrace = false;
                                currentlyDefining = true;
                            }
                            else
                            {
                                // Interpreter error here
                            }
                        }
                    }
                    else
                    {
                        switch (currentDefType)
                        {
                            /*
                             *
                             * INFORMATION DEFINITION
                             *
                             */
                            case DefinitionType.InformationDefinition:
                                switch (parsedContent.Length)
                                {
                                    case 2:
                                        if (parsedContent[1].Item1 == ParsedObjectType.String)
                                        {
                                            switch (firstWord)
                                            {
                                                // 'NAME' definition (usage: 'name: Name'):
                                                case "name:":
                                                    ((InformationDefinition)currentDefinition).Name = (string)parsedContent[1].Item2;

                                                    break;

                                                // 'AUTHOR' definition (usage: 'author: Author'):
                                                case "author:":
                                                    ((InformationDefinition)currentDefinition).Author = (string)parsedContent[1].Item2;

                                                    break;

                                                default:
                                                    // Interpreter error here

                                                    break;
                                            }
                                        }
                                        else if (parsedContent[1].Item1 == ParsedObjectType.Word)
                                        {
                                            switch (firstWord)
                                            {
                                                // 'VERSION' definition (usage: 'version: Version'):
                                                case "version:":
                                                    double parsedVersion;
                                                    bool successfulParse = double.TryParse((string)parsedContent[1].Item2, out parsedVersion);

                                                    if (successfulParse)
                                                    {
                                                        ((InformationDefinition)currentDefinition).Version = parsedVersion;
                                                    }

                                                    break;

                                                default:
                                                    // Interpreter error here

                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            // Interpreter error here
                                        }

                                        break;

                                    case 1:
                                        if ((string)parsedContent[0].Item2 == "}")   // END OF DEFINITION '}'
                                        {
                                            ((InformationDefinition)currentDefinition).MD5Hash = Files.GetMD5Hash(file);
                                            comfyContent.InformationDefinitions[file] = (InformationDefinition)currentDefinition;
                                            currentlyDefining = false;
                                        }

                                        break;

                                    default:
                                        // Interpreter error here

                                        break;
                                }

                                break;


                            /*
                             *
                             * ITEM DEFINITION
                             *
                             */
                            case DefinitionType.ItemDefinition:
                                switch (parsedContent.Length)
                                {
                                    case 3:
                                        // 'ENTRY' definition (usage: 'entry: EntryKey EntryData'):
                                        if (firstWord == "entry:" && parsedContent[1].Item1 == ParsedObjectType.Word && parsedContent[2].Item1 == ParsedObjectType.Word)
                                        {
                                            ((ItemDefinition)currentDefinition).Entry[(string)parsedContent[1].Item2] = (string)parsedContent[2].Item2;
                                        }
                                        else
                                        {
                                            // Interpreter error here
                                        }

                                        break;

                                    case 2:
                                        if (parsedContent[1].Item1 == ParsedObjectType.Word)
                                        {
                                            switch (firstWord)
                                            {
                                                // 'NAME' definition (usage: 'name: LanguageName'):
                                                case "name:":
                                                    ((ItemDefinition)currentDefinition).LanguageName = (string)parsedContent[1].Item2;

                                                    break;

                                                // 'TEXTURE' definition (usage: 'texture: TextureName'):
                                                case "texture:":
                                                    ((ItemDefinition)currentDefinition).Texture = (string)parsedContent[1].Item2;

                                                    break;

                                                // 'TYPE' definition (usage: 'type: ItemType'):
                                                case "type:":
                                                    switch (((string)parsedContent[1].Item2).ToLower())
                                                    {
                                                        case "weapon":
                                                            ((ItemDefinition)currentDefinition).Type = ItemType.Weapon;
                                                            break;

                                                        case "bucket":
                                                            ((ItemDefinition)currentDefinition).Type = ItemType.Bucket;
                                                            break;

                                                        default:
                                                            // Interpreter error here

                                                            break;
                                                    }

                                                    break;

                                                // 'PLACES' definition (usage: 'places: Places'):
                                                case "places:":
                                                    ((ItemDefinition)currentDefinition).Places = (string)parsedContent[1].Item2;

                                                    break;

                                                // 'ORIGINAL' definition (usage: 'original: Original'):
                                                case "original:":
                                                    ((ItemDefinition)currentDefinition).Original = (string)parsedContent[1].Item2;

                                                    break;

                                                default:
                                                    // Interpreter error here

                                                    break;
                                            }
                                        }
                                        else if (parsedContent[1].Item1 == ParsedObjectType.Integer)
                                        {
                                            switch (firstWord)
                                            {
                                                // 'POWER' definition (usage: 'power: Power'):
                                                case "power:":
                                                    ((ItemDefinition)currentDefinition).Power = (int)parsedContent[1].Item2;

                                                    break;

                                                default:
                                                    // Interpreter error here

                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            // Interpreter error here
                                        }

                                        break;

                                    case 1:
                                        if ((string)parsedContent[0].Item2 == "}")   // END OF DEFINITION '}'
                                        {
                                            comfyContent.ItemDefinitions[objectName] = (ItemDefinition)currentDefinition;
                                            currentlyDefining = false;
                                        }
                                        else if (((string)parsedContent[0].Item2)[0] == '+')   // SET FLAG '+FLAG'
                                        {
                                            ((ItemDefinition)currentDefinition).SetFlag(((string)parsedContent[0].Item2).Substring(1));
                                        }
                                        else if (((string)parsedContent[0].Item2)[0] == '-')   // REMOVE FLAG '-FLAG'
                                        {
                                            ((ItemDefinition)currentDefinition).RemoveFlag(((string)parsedContent[0].Item2).Substring(1));
                                        }
                                        else
                                        {
                                            // Interpreter error here
                                        }

                                        break;

                                    default:
                                        // Interpreter error here

                                        break;
                                }

                                break;


                            /*
                             *
                             * LANGUAGE DEFINITION
                             *
                             */
                            case DefinitionType.LanguageDefinition:
                                switch (parsedContent.Length)
                                {
                                    case 3:
                                        // 'LOAD' definition (usage: 'load: LanguageName Source'):
                                        if (firstWord == "load:" && parsedContent[1].Item1 == ParsedObjectType.String && parsedContent[2].Item1 == ParsedObjectType.String)
                                        {
                                            comfyContent.LanguageDefinitions[(string)parsedContent[1].Item2] = Files.ReplaceSpecialDirectories((string)parsedContent[2].Item2);
                                        }
                                        else
                                        {
                                            // Interpreter error here
                                        }

                                        break;

                                    case 1:
                                        if ((string)parsedContent[0].Item2 == "}")   // END OF DEFINITION '}'
                                        {
                                            currentlyDefining = false;
                                        }
                                        else
                                        {
                                            // Interpreter error here
                                        }

                                        break;

                                    default:
                                        // Interpreter error here

                                        break;
                                }

                                break;


                            /*
                             *
                             * PET DEFINITION
                             *
                             */
                            case DefinitionType.PetDefinition:
                                switch (parsedContent.Length)
                                {
                                    case 2:
                                        if (parsedContent[1].Item1 == ParsedObjectType.Word)
                                        {
                                            switch (firstWord)
                                            {
                                                // 'NAME' definition (usage: 'name: LanguageName'):
                                                case "name:":
                                                    ((PetDefinition)currentDefinition).LanguageName = (string)parsedContent[1].Item2;

                                                    break;

                                                // 'TEXTURE' definition (usage: 'texture: TextureName'):
                                                case "texture:":
                                                    ((PetDefinition)currentDefinition).Texture = (string)parsedContent[1].Item2;

                                                    break;

                                                default:
                                                    // Interpreter error here

                                                    break;
                                            }
                                        }
                                        else if (parsedContent[1].Item1 == ParsedObjectType.Integer)
                                        {
                                            if (firstWord == "health:")
                                            {
                                                ((PetDefinition)currentDefinition).Health = (int)parsedContent[1].Item2;
                                            }
                                        }
                                        else
                                        {
                                            // Interpreter error here
                                        }

                                        break;

                                    case 1:
                                        if ((string)parsedContent[0].Item2 == "}")   // END OF DEFINITION '}'
                                        {
                                            comfyContent.PetDefinitions[objectName] = (PetDefinition)currentDefinition;
                                            currentlyDefining = false;
                                        }
                                        else if (((string)parsedContent[0].Item2)[0] == '+')   // SET FLAG '+FLAG'
                                        {
                                            ((PetDefinition)currentDefinition).SetFlag(((string)parsedContent[0].Item2).Substring(1));
                                        }
                                        else if (((string)parsedContent[0].Item2)[0] == '-')   // REMOVE FLAG '-FLAG'
                                        {
                                            ((PetDefinition)currentDefinition).RemoveFlag(((string)parsedContent[0].Item2).Substring(1));
                                        }
                                        else
                                        {
                                            // Interpreter error here
                                        }

                                        break;

                                    default:
                                        // Interpreter error here

                                        break;
                                }

                                break;


                            /*
                             *
                             * PLAYER PAWN DEFINITION
                             *
                             */
                            case DefinitionType.PlayerPawnDefinition:
                                switch (parsedContent.Length)
                                {
                                    case 2:
                                        if (parsedContent[1].Item1 == ParsedObjectType.Word)
                                        {
                                            switch (firstWord)
                                            {
                                                // 'BASESPEED' definition (usage: 'basespeed: BaseSpeed'):
                                                case "basespeed:":
                                                    double parsedSpeed;
                                                    bool successfulParse = double.TryParse((string)parsedContent[1].Item2, out parsedSpeed);

                                                    if (successfulParse)
                                                    {
                                                        ((PlayerPawnDefinition)currentDefinition).BaseSpeed = parsedSpeed;
                                                    }

                                                    break;

                                                // 'MAXSPEED' definition (usage: 'maxspeed: MaxSpeed'):
                                                case "maxspeed:":
                                                    double parsedSpeed_;
                                                    bool successfulParse_ = double.TryParse((string)parsedContent[1].Item2, out parsedSpeed_);

                                                    if (successfulParse_)
                                                    {
                                                        ((PlayerPawnDefinition)currentDefinition).MaxSpeed = parsedSpeed_;
                                                    }

                                                    break;

                                                default:
                                                    // Interpreter error here

                                                    break;
                                            }
                                        }
                                        else if (parsedContent[1].Item1 == ParsedObjectType.Integer)
                                        {
                                            switch (firstWord)
                                            {
                                                // 'BASESTRENGTH' definition (usage: 'basestrength: BaseStrength'):
                                                case "basestrength:":
                                                    ((PlayerPawnDefinition)currentDefinition).BaseStrength = (int)parsedContent[1].Item2;

                                                    break;

                                                // 'BASEDEFENCE' definition (usage: 'basedefence: BaseDefence'):
                                                case "basedefence:":
                                                    ((PlayerPawnDefinition)currentDefinition).BaseDefence = (int)parsedContent[1].Item2;

                                                    break;

                                                // 'HEALTH' definition (usage: 'health: Health'):
                                                case "health:":
                                                    ((PlayerPawnDefinition)currentDefinition).Health = (int)parsedContent[1].Item2;

                                                    break;

                                                // 'MAXLEVEL' definition (usage: 'maxlevel: MaxLevel'):
                                                case "maxlevel:":
                                                    ((PlayerPawnDefinition)currentDefinition).MaxLevel = (int)parsedContent[1].Item2;

                                                    break;

                                                default:
                                                    // Interpreter error here

                                                    break;
                                            }
                                        }

                                        break;

                                    case 1:
                                        if ((string)parsedContent[0].Item2 == "}")   // END OF DEFINITION '}'
                                        {
                                            comfyContent.PlayerPawn = (PlayerPawnDefinition)currentDefinition;
                                            currentlyDefining = false;
                                        }
                                        else
                                        {
                                            // Interpreter error here
                                        }

                                        break;

                                    default:
                                        // Interpreter error here

                                        break;
                                }

                                break;


                            /*
                             *
                             * SOUND DEFINITION
                             *
                             */
                            case DefinitionType.SoundDefinition:
                                switch (parsedContent.Length)
                                {
                                    case 2:
                                        // 'SRC' definition (usage: 'src: Source')
                                        if (firstWord == "src:" && parsedContent[1].Item1 == ParsedObjectType.String)
                                        {
                                            ((SoundDefinition)currentDefinition).Source = Files.ReplaceSpecialDirectories((string)parsedContent[1].Item2);
                                        }
                                        else
                                        {
                                            // Interpreter error here
                                        }

                                        break;

                                    case 1:
                                        if ((string)parsedContent[0].Item2 == "}")   // END OF DEFINITION '}'
                                        {
                                            comfyContent.SoundDefinitions[objectName] = (SoundDefinition)currentDefinition;
                                            currentlyDefining = false;
                                        }
                                        else
                                        {
                                            // Interpreter error here
                                        }

                                        break;

                                    default:
                                        // Interpreter error here

                                        break;
                                }

                                break;


                            /*
                             *
                             * TEXTURE DEFINITION
                             *
                             */
                            case DefinitionType.TextureDefinition:
                                switch (parsedContent.Length)
                                {
                                    case 3:
                                        // 'LOAD' definition (usage: 'load: TextureName Source'):
                                        if (firstWord == "load:" && parsedContent[1].Item1 == ParsedObjectType.String && parsedContent[2].Item1 == ParsedObjectType.String)
                                        {
                                            comfyContent.Textures[(string)parsedContent[1].Item2] = Files.ReplaceSpecialDirectories((string)parsedContent[2].Item2);
                                        }
                                        else
                                        {
                                            // Interpreter error here
                                        }

                                        break;

                                    case 1:
                                        if ((string)parsedContent[0].Item2 == "}")   // END OF DEFINITION '}'
                                        {
                                            currentlyDefining = false;
                                        }
                                        else
                                        {
                                            // Interpreter error here
                                        }

                                        break;

                                    default:
                                        // Interpreter error here

                                        break;
                                }

                                break;


                            /*
                             *
                             * THING DEFINITION
                             *
                             */
                            case DefinitionType.ThingDefinition:
                                switch (parsedContent.Length)
                                {
                                    case 3:
                                        // 'DROPS' definition (usage 'drops: ItemName Quantity'):
                                        if (firstWord == "drops:" && parsedContent[1].Item1 == ParsedObjectType.Word && parsedContent[2].Item1 == ParsedObjectType.Integer)
                                        {
                                            ((ThingDefinition)currentDefinition).Drops = new ItemStack(new Item.Item((string)parsedContent[1].Item2), (int)parsedContent[2].Item2);
                                        }
                                        else
                                        {
                                            // Interpreter error here
                                        }

                                        break;

                                    case 2:
                                        if (parsedContent[1].Item1 == ParsedObjectType.Word)
                                        {
                                            switch (firstWord)
                                            {
                                                // 'NAME' definition (usage: 'name: LanguageName'):
                                                case "name:":
                                                    ((ThingDefinition)currentDefinition).LanguageName = (string)parsedContent[1].Item2;

                                                    break;

                                                // 'TEXTURE' definition (usage: 'texture: TextureName'):
                                                case "texture:":
                                                    ((ThingDefinition)currentDefinition).Texture = (string)parsedContent[1].Item2;

                                                    break;

                                                // 'NATURE' definition (usage: 'nature: Nature'):
                                                case "nature:":
                                                    if (firstWord == "passive")
                                                    {
                                                        ((ThingDefinition)currentDefinition).Nature = NatureType.Passive;
                                                    }
                                                    else if (firstWord == "hostile")
                                                    {
                                                        ((ThingDefinition)currentDefinition).Nature = NatureType.Hostile;
                                                    }

                                                    break;

                                                default:
                                                    // Interpreter error here

                                                    break;
                                            }
                                        }
                                        else if (parsedContent[1].Item1 == ParsedObjectType.Integer)
                                        {
                                            switch (firstWord)
                                            {
                                                // 'HEALTH' definition (usage: 'health: Health'):
                                                case "health:":
                                                    ((ThingDefinition)currentDefinition).Health = (int)parsedContent[1].Item2;

                                                    break;

                                                default:
                                                    // Intpreter error here

                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            // Interpreter error here
                                        }

                                        break;

                                    case 1:
                                        if ((string)parsedContent[0].Item2 == "}")   // END OF DEFINITION '}'
                                        {
                                            comfyContent.ThingDefinitions[objectName] = (ThingDefinition)currentDefinition;
                                            currentlyDefining = false;
                                        }
                                        else if (((string)parsedContent[0].Item2)[0] == '+')   // SET FLAG '+FLAG'
                                        {
                                            ((ThingDefinition)currentDefinition).SetFlag(((string)parsedContent[0].Item2).Substring(1));
                                        }
                                        else if (((string)parsedContent[0].Item2)[0] == '-')   // REMOVE FLAG '-FLAG'
                                        {
                                            ((ThingDefinition)currentDefinition).RemoveFlag(((string)parsedContent[0].Item2).Substring(1));
                                        }
                                        else
                                        {
                                            // Interpreter error here
                                        }

                                        break;

                                    default:
                                        // Interpreter error here

                                        break;
                                }

                                break;


                            /*
                             *
                             * TILE FLOOR DEFINITION
                             *
                             */
                            case DefinitionType.TileFloorDefinition:
                                switch (parsedContent.Length)
                                {
                                    case 2:
                                        switch (firstWord)
                                        {
                                            // 'TEXTURE' definition (usage: 'texture: TextureName'):
                                            case "texture:":
                                                ((TileFloorDefinition)currentDefinition).Texture = (string)parsedContent[1].Item2;

                                                break;
                                        }

                                        break;

                                    case 1:
                                        if ((string)parsedContent[0].Item2 == "}")   // END OF DEFINITION '}'
                                        {
                                            comfyContent.TileFloorDefinitions[objectName] = (TileFloorDefinition)currentDefinition;
                                            currentlyDefining = false;
                                        }
                                        else if (((string)parsedContent[0].Item2)[0] == '+')   // SET FLAG '+FLAG'
                                        {
                                            ((TileFloorDefinition)currentDefinition).SetFlag(((string)parsedContent[0].Item2).Substring(1));
                                        }
                                        else if (((string)parsedContent[0].Item2)[0] == '-')   // REMOVE FLAG '-FLAG'
                                        {
                                            ((TileFloorDefinition)currentDefinition).RemoveFlag(((string)parsedContent[0].Item2).Substring(1));
                                        }

                                        break;

                                    default:
                                        // Interpreter error here

                                        break;
                                }

                                break;


                            /*
                             *
                             * TILE WALL DEFINITION
                             *
                             */
                            case DefinitionType.TileWallDefinition:
                                switch (parsedContent.Length)
                                {
                                    case 2:
                                        switch (firstWord)
                                        {
                                            // 'TEXTURE' definition (usage: 'texture: TextureName'):
                                            case "texture:":
                                                ((TileWallDefinition)currentDefinition).Texture = (string)parsedContent[1].Item2;

                                                break;

                                            // 'BREAKS' definition (usage: 'breaks: Breaks'):
                                            case "breaks:":
                                                bool successfulParse = bool.TryParse((string)parsedContent[1].Item2, out ((TileWallDefinition)currentDefinition).Breaks);

                                                if (!successfulParse)
                                                {
                                                    ((TileWallDefinition)currentDefinition).Breaks = false;
                                                }

                                                break;

                                            default:
                                                // Interpreter error here

                                                break;
                                        }

                                        break;

                                    case 1:
                                        if (firstWord == "}")   // END OF DEFINITION '}'
                                        {
                                            comfyContent.TileWallDefinitions[objectName] = (TileWallDefinition)currentDefinition;
                                            currentlyDefining = false;
                                        }
                                        else if (firstWord[0] == '+')   // SET FLAG '+FLAG'
                                        {
                                            ((TileWallDefinition)currentDefinition).SetFlag(((string)parsedContent[0].Item2).Substring(1));
                                        }
                                        else if (firstWord[0] == '-')   // REMOVE FLAG '-FLAG'
                                        {
                                            ((TileWallDefinition)currentDefinition).RemoveFlag(((string)parsedContent[0].Item2).Substring(1));
                                        }

                                        break;

                                    default:
                                        // Interpreter error here

                                        break;
                                }

                                break;

                            default:
                                // Interpreter error here

                                break;
                        }
                    }
                }
            }

            return comfyContent;
        }


        /// <summary>
        /// Parses a string into separate data objects.
        /// </summary>
        /// <param name="line">The string data to parse.</param>
        /// <returns>The array of data objects retrieved from the string data.</returns>
        public static Tuple<ParsedObjectType, object>[] ParseLine(string line)
        {
            var parsedLineData = new List<Tuple<ParsedObjectType, object>>();

            bool insideQuotes = false;
            bool finished = false;
            bool escaped = false;
            bool individualFinished = false;
            bool startedDefining = false;
            bool objectWhitespaceSeparated = true;
            bool errors = false;
            int index = 0;

            string definingData = "";

            // Strip all tabs, because they are evil.
            line = line.Replace("\t", "    ");

            if (line.Length > 0)
            {
                do
                {
                    switch (line[index])
                    {
                        //                  //
                        // Handling spaces. //
                        //                  //
                        case ' ':
                            if (startedDefining)
                            {
                                if (insideQuotes)
                                {
                                    definingData += " ";
                                }
                                else
                                {
                                    individualFinished = true;
                                    objectWhitespaceSeparated = true;
                                }
                            }
                            else if (!objectWhitespaceSeparated)
                            {
                                objectWhitespaceSeparated = true;
                            }

                            break;

                        //                         //
                        // Handling hashes/pounds. //
                        //                         //
                        case '#':
                            if (startedDefining)
                            {
                                if (insideQuotes)
                                {
                                    definingData += "#";
                                }
                                else
                                {
                                    individualFinished = true;
                                    finished = true;
                                }
                            }
                            else
                            {
                                finished = true;
                            }

                            break;

                        //                  //
                        // Handling quotes. //
                        //                  //
                        case '"':
                            if (startedDefining)
                            {
                                if (insideQuotes)
                                {
                                    if (escaped)
                                    {
                                        definingData += "\"";
                                        escaped = false;
                                    }
                                    else
                                    {
                                        definingData += "\"";
                                        insideQuotes = false;
                                        individualFinished = true;
                                        objectWhitespaceSeparated = false;
                                    }
                                }
                                else
                                {
                                    errors = true;
                                    individualFinished = true;
                                    objectWhitespaceSeparated = false;
                                }
                            }
                            else
                            {
                                if (objectWhitespaceSeparated)
                                {
                                    startedDefining = true;
                                    definingData += "\"";
                                    insideQuotes = true;
                                }
                            }

                            break;

                        case '\\':
                            //                       //
                            // Handling backslashes. //
                            //                       //
                            if (insideQuotes)
                            {
                                if (escaped)
                                {
                                    definingData += "\\";
                                    escaped = false;
                                }
                                else
                                {
                                    escaped = true;
                                }
                            }
                            else
                            {
                                errors = true;
                                individualFinished = true;
                                objectWhitespaceSeparated = false;
                            }

                            break;

                        default:
                            //                           //
                            // Handling everything else. //
                            //                           //
                            if (startedDefining)
                            {
                                definingData += line[index];
                            }
                            else
                            {
                                if (objectWhitespaceSeparated)
                                {
                                    startedDefining = true;
                                    definingData += line[index];
                                }
                            }

                            break;
                    }

                    if (index == line.Length - 1 && startedDefining && !individualFinished)
                    {
                        individualFinished = true;
                    }

                    if (individualFinished)
                    {
                        startedDefining = false;

                        object parsedObject = definingData;
                        ParsedObjectType parsedType;

                        if (!errors)
                        {
                            if (definingData.Length >= 2 && definingData[0] == '"' && definingData[definingData.Length - 1] == '"')
                            {
                                parsedObject = definingData.Substring(1, definingData.Length - 2);
                                parsedType = ParsedObjectType.String;
                            }
                            else
                            {
                                int resultingParsedInt = 0;
                                bool successfulParse = int.TryParse(definingData, out resultingParsedInt);

                                if (successfulParse)
                                {
                                    parsedType = ParsedObjectType.Integer;
                                    parsedObject = resultingParsedInt;
                                }
                                else
                                {
                                    parsedType = ParsedObjectType.Word;
                                }
                            }
                        }
                        else
                        {
                            parsedType = ParsedObjectType.Invalid;
                            errors = false;
                        }

                        parsedLineData.Add(new Tuple<ParsedObjectType, object>(parsedType, parsedObject));

                        definingData = "";
                        individualFinished = false;
                    }

                    index++;
                } while (!finished && index <= line.Length - 1);
            }

            return parsedLineData.ToArray();
        }
    }
}
