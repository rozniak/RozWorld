/**
 * RozWorld.COMFY.ComfyInterpreter -- RozWorld COMFY Definition Script Interpreter
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System;
using System.IO;
using System.Collections.Generic;

using RozWorld.IO;
using RozWorld.COMFY.Definition;
using RozWorld.Item;

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
                string[] fileContent = Files.GetTextFile(file);

                object currentDefinition = null;
                DefinitionType currentDefType = DefinitionType.Unknown;
                bool currentlyDefining = false;
                bool expectingOpeningBrace = false;
                string objectName = "";

                foreach (string line in fileContent)
                {
                    ParsedLineData[] parsedContent = Files.ParseLine(line);

                    if (parsedContent.Length == 0 || parsedContent[0].Type != ParsedObjectType.Word)
                    {
                        continue;
                    }

                    // Used to examine the first WORD of a line...
                    string firstWord = ((string)parsedContent[0].Data).ToLower();

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
                            else if (parsedContent.Length == 2 && parsedContent[1].Type == ParsedObjectType.Word)
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
                                objectName = (string)parsedContent[1].Data;
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
                                        if (parsedContent[1].Type == ParsedObjectType.String)
                                        {
                                            switch (firstWord)
                                            {
                                                // 'NAME' definition (usage: 'name: Name'):
                                                case "name:":
                                                    ((InformationDefinition)currentDefinition).Name = (string)parsedContent[1].Data;

                                                    break;

                                                // 'AUTHOR' definition (usage: 'author: Author'):
                                                case "author:":
                                                    ((InformationDefinition)currentDefinition).Author = (string)parsedContent[1].Data;

                                                    break;

                                                default:
                                                    // Interpreter error here

                                                    break;
                                            }
                                        }
                                        else if (parsedContent[1].Type == ParsedObjectType.Word)
                                        {
                                            switch (firstWord)
                                            {
                                                // 'VERSION' definition (usage: 'version: Version'):
                                                case "version:":
                                                    double parsedVersion;
                                                    bool successfulParse = double.TryParse((string)parsedContent[1].Data, out parsedVersion);

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
                                        if ((string)parsedContent[0].Data == "}")   // END OF DEFINITION '}'
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
                                        if (firstWord == "entry:" && parsedContent[1].Type == ParsedObjectType.Word && parsedContent[2].Type == ParsedObjectType.Word)
                                        {
                                            ((ItemDefinition)currentDefinition).Entry[(string)parsedContent[1].Data] = (string)parsedContent[2].Data;
                                        }
                                        else
                                        {
                                            // Interpreter error here
                                        }

                                        break;

                                    case 2:
                                        if (parsedContent[1].Type == ParsedObjectType.Word)
                                        {
                                            switch (firstWord)
                                            {
                                                // 'NAME' definition (usage: 'name: LanguageName'):
                                                case "name:":
                                                    ((ItemDefinition)currentDefinition).LanguageName = (string)parsedContent[1].Data;

                                                    break;

                                                // 'TEXTURE' definition (usage: 'texture: TextureName'):
                                                case "texture:":
                                                    ((ItemDefinition)currentDefinition).Texture = (string)parsedContent[1].Data;

                                                    break;

                                                // 'TYPE' definition (usage: 'type: ItemType'):
                                                case "type:":
                                                    switch (((string)parsedContent[1].Data).ToLower())
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
                                                    ((ItemDefinition)currentDefinition).Places = (string)parsedContent[1].Data;

                                                    break;

                                                // 'ORIGINAL' definition (usage: 'original: Original'):
                                                case "original:":
                                                    ((ItemDefinition)currentDefinition).Original = (string)parsedContent[1].Data;

                                                    break;

                                                default:
                                                    // Interpreter error here

                                                    break;
                                            }
                                        }
                                        else if (parsedContent[1].Type == ParsedObjectType.Integer)
                                        {
                                            switch (firstWord)
                                            {
                                                // 'POWER' definition (usage: 'power: Power'):
                                                case "power:":
                                                    ((ItemDefinition)currentDefinition).Power = (int)parsedContent[1].Data;

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
                                        if ((string)parsedContent[0].Data == "}")   // END OF DEFINITION '}'
                                        {
                                            comfyContent.ItemDefinitions[objectName] = (ItemDefinition)currentDefinition;
                                            currentlyDefining = false;
                                        }
                                        else if (((string)parsedContent[0].Data)[0] == '+')   // SET FLAG '+FLAG'
                                        {
                                            ((ItemDefinition)currentDefinition).SetFlag(((string)parsedContent[0].Data).Substring(1));
                                        }
                                        else if (((string)parsedContent[0].Data)[0] == '-')   // REMOVE FLAG '-FLAG'
                                        {
                                            ((ItemDefinition)currentDefinition).RemoveFlag(((string)parsedContent[0].Data).Substring(1));
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
                                        if (firstWord == "load:" && parsedContent[1].Type == ParsedObjectType.String && parsedContent[2].Type == ParsedObjectType.String)
                                        {
                                            comfyContent.LanguageDefinitions[(string)parsedContent[1].Data] = Files.ReplaceSpecialDirectories((string)parsedContent[2].Data);
                                        }
                                        else
                                        {
                                            // Interpreter error here
                                        }

                                        break;

                                    case 1:
                                        if ((string)parsedContent[0].Data == "}")   // END OF DEFINITION '}'
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
                                        if (parsedContent[1].Type == ParsedObjectType.Word)
                                        {
                                            switch (firstWord)
                                            {
                                                // 'NAME' definition (usage: 'name: LanguageName'):
                                                case "name:":
                                                    ((PetDefinition)currentDefinition).LanguageName = (string)parsedContent[1].Data;

                                                    break;

                                                // 'TEXTURE' definition (usage: 'texture: TextureName'):
                                                case "texture:":
                                                    ((PetDefinition)currentDefinition).Texture = (string)parsedContent[1].Data;

                                                    break;

                                                default:
                                                    // Interpreter error here

                                                    break;
                                            }
                                        }
                                        else if (parsedContent[1].Type == ParsedObjectType.Integer)
                                        {
                                            if (firstWord == "health:")
                                            {
                                                ((PetDefinition)currentDefinition).Health = (int)parsedContent[1].Data;
                                            }
                                        }
                                        else
                                        {
                                            // Interpreter error here
                                        }

                                        break;

                                    case 1:
                                        if ((string)parsedContent[0].Data == "}")   // END OF DEFINITION '}'
                                        {
                                            comfyContent.PetDefinitions[objectName] = (PetDefinition)currentDefinition;
                                            currentlyDefining = false;
                                        }
                                        else if (((string)parsedContent[0].Data)[0] == '+')   // SET FLAG '+FLAG'
                                        {
                                            ((PetDefinition)currentDefinition).SetFlag(((string)parsedContent[0].Data).Substring(1));
                                        }
                                        else if (((string)parsedContent[0].Data)[0] == '-')   // REMOVE FLAG '-FLAG'
                                        {
                                            ((PetDefinition)currentDefinition).RemoveFlag(((string)parsedContent[0].Data).Substring(1));
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
                                        if (parsedContent[1].Type == ParsedObjectType.Word)
                                        {
                                            switch (firstWord)
                                            {
                                                // 'BASESPEED' definition (usage: 'basespeed: BaseSpeed'):
                                                case "basespeed:":
                                                    double parsedSpeed;
                                                    bool successfulParse = double.TryParse((string)parsedContent[1].Data, out parsedSpeed);

                                                    if (successfulParse)
                                                    {
                                                        ((PlayerPawnDefinition)currentDefinition).BaseSpeed = parsedSpeed;
                                                    }

                                                    break;

                                                // 'MAXSPEED' definition (usage: 'maxspeed: MaxSpeed'):
                                                case "maxspeed:":
                                                    double parsedSpeed_;
                                                    bool successfulParse_ = double.TryParse((string)parsedContent[1].Data, out parsedSpeed_);

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
                                        else if (parsedContent[1].Type == ParsedObjectType.Integer)
                                        {
                                            switch (firstWord)
                                            {
                                                // 'BASESTRENGTH' definition (usage: 'basestrength: BaseStrength'):
                                                case "basestrength:":
                                                    ((PlayerPawnDefinition)currentDefinition).BaseStrength = (int)parsedContent[1].Data;

                                                    break;

                                                // 'BASEDEFENCE' definition (usage: 'basedefence: BaseDefence'):
                                                case "basedefence:":
                                                    ((PlayerPawnDefinition)currentDefinition).BaseDefence = (int)parsedContent[1].Data;

                                                    break;

                                                // 'HEALTH' definition (usage: 'health: Health'):
                                                case "health:":
                                                    ((PlayerPawnDefinition)currentDefinition).Health = (int)parsedContent[1].Data;

                                                    break;

                                                // 'MAXLEVEL' definition (usage: 'maxlevel: MaxLevel'):
                                                case "maxlevel:":
                                                    ((PlayerPawnDefinition)currentDefinition).MaxLevel = (int)parsedContent[1].Data;

                                                    break;

                                                default:
                                                    // Interpreter error here

                                                    break;
                                            }
                                        }

                                        break;

                                    case 1:
                                        if ((string)parsedContent[0].Data == "}")   // END OF DEFINITION '}'
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
                                        if (firstWord == "src:" && parsedContent[1].Type == ParsedObjectType.String)
                                        {
                                            ((SoundDefinition)currentDefinition).Source = Files.ReplaceSpecialDirectories((string)parsedContent[1].Data);
                                        }
                                        else
                                        {
                                            // Interpreter error here
                                        }

                                        break;

                                    case 1:
                                        if ((string)parsedContent[0].Data == "}")   // END OF DEFINITION '}'
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
                                        if (firstWord == "load:" && parsedContent[1].Type == ParsedObjectType.String && parsedContent[2].Type == ParsedObjectType.String)
                                        {
                                            comfyContent.Textures[(string)parsedContent[1].Data] = Files.ReplaceSpecialDirectories((string)parsedContent[2].Data);
                                        }
                                        else
                                        {
                                            // Interpreter error here
                                        }

                                        break;

                                    case 1:
                                        if ((string)parsedContent[0].Data == "}")   // END OF DEFINITION '}'
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
                                        if (firstWord == "drops:" && parsedContent[1].Type == ParsedObjectType.Word && parsedContent[2].Type == ParsedObjectType.Integer)
                                        {
                                            ((ThingDefinition)currentDefinition).Drops = new ItemStack(new Item.Item((string)parsedContent[1].Data), (int)parsedContent[2].Data);
                                        }
                                        else
                                        {
                                            // Interpreter error here
                                        }

                                        break;

                                    case 2:
                                        if (parsedContent[1].Type == ParsedObjectType.Word)
                                        {
                                            switch (firstWord)
                                            {
                                                // 'NAME' definition (usage: 'name: LanguageName'):
                                                case "name:":
                                                    ((ThingDefinition)currentDefinition).LanguageName = (string)parsedContent[1].Data;

                                                    break;

                                                // 'TEXTURE' definition (usage: 'texture: TextureName'):
                                                case "texture:":
                                                    ((ThingDefinition)currentDefinition).Texture = (string)parsedContent[1].Data;

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
                                        else if (parsedContent[1].Type == ParsedObjectType.Integer)
                                        {
                                            switch (firstWord)
                                            {
                                                // 'HEALTH' definition (usage: 'health: Health'):
                                                case "health:":
                                                    ((ThingDefinition)currentDefinition).Health = (int)parsedContent[1].Data;

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
                                        if ((string)parsedContent[0].Data == "}")   // END OF DEFINITION '}'
                                        {
                                            comfyContent.ThingDefinitions[objectName] = (ThingDefinition)currentDefinition;
                                            currentlyDefining = false;
                                        }
                                        else if (((string)parsedContent[0].Data)[0] == '+')   // SET FLAG '+FLAG'
                                        {
                                            ((ThingDefinition)currentDefinition).SetFlag(((string)parsedContent[0].Data).Substring(1));
                                        }
                                        else if (((string)parsedContent[0].Data)[0] == '-')   // REMOVE FLAG '-FLAG'
                                        {
                                            ((ThingDefinition)currentDefinition).RemoveFlag(((string)parsedContent[0].Data).Substring(1));
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
                                                ((TileFloorDefinition)currentDefinition).Texture = (string)parsedContent[1].Data;

                                                break;
                                        }

                                        break;

                                    case 1:
                                        if ((string)parsedContent[0].Data == "}")   // END OF DEFINITION '}'
                                        {
                                            comfyContent.TileFloorDefinitions[objectName] = (TileFloorDefinition)currentDefinition;
                                            currentlyDefining = false;
                                        }
                                        else if (((string)parsedContent[0].Data)[0] == '+')   // SET FLAG '+FLAG'
                                        {
                                            ((TileFloorDefinition)currentDefinition).SetFlag(((string)parsedContent[0].Data).Substring(1));
                                        }
                                        else if (((string)parsedContent[0].Data)[0] == '-')   // REMOVE FLAG '-FLAG'
                                        {
                                            ((TileFloorDefinition)currentDefinition).RemoveFlag(((string)parsedContent[0].Data).Substring(1));
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
                                                ((TileWallDefinition)currentDefinition).Texture = (string)parsedContent[1].Data;

                                                break;

                                            // 'BREAKS' definition (usage: 'breaks: Breaks'):
                                            case "breaks:":
                                                bool successfulParse = bool.TryParse((string)parsedContent[1].Data, out ((TileWallDefinition)currentDefinition).Breaks);

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
                                            ((TileWallDefinition)currentDefinition).SetFlag(((string)parsedContent[0].Data).Substring(1));
                                        }
                                        else if (firstWord[0] == '-')   // REMOVE FLAG '-FLAG'
                                        {
                                            ((TileWallDefinition)currentDefinition).RemoveFlag(((string)parsedContent[0].Data).Substring(1));
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
    }
}
