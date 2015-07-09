﻿//
// RozWorld.Enums -- RozWorld Enumerations File
//
// This source-code is part of the RozWorld project by rozza of Oddmatics:
// <<http://www.oddmatics.co.uk>>
// <<http://www.oddmatics.co.uk/projects/rozworld>>
//
// Sharing, editing and general licence term information can be found inside of the "sup.txt" file that should be located in the root of this project's directory structure.
//


/* [TO BE UPDATED]
 * The type of item something is, useful for handling certain events.
 */
public enum ItemType
{
    Weapon,
    Bucket
}


/* [TO BE UPDATED (?)]
 * The type of sound a file is, effect or BGM.
 */
public enum SoundType
{
    Music,
    Sound
}


/* 
 * The status of wild entities that may be hostile or passive.
 */
public enum NatureType
{
    Passive,
    Hostile
}


/* [TO BE UPDATED]
 * The results of a connection attempt, to be reported to the connecting client.
 */
public enum ConnectResult
{
    Success,
    ServerFull,
    NotWhitelisted,
    InvalidMPKey,
    Banned,
    DuplicateNick,
    IsLiveFalse,
    PingFailure,
    MustBeAuthorised,
    Unknown
}


/*
 * The methods of damage performed on entities, useful for death messages/handling.
 */
public enum DamageMethod
{
    Generic,
    Explosion,
    Potion,
    Weapon,
    Pet
}


/* [TO BE CHANGED (?)]
 * The types of strips to perform on strings to make them safe for saving or other situations.
 */
public enum StripType
{
    None,
    SemiColons,
    WindowsSafe,
    Both
}


/*
 * The parsed object types, this mainly for the interpreter.
 */
public enum ParsedObjectType
{
    Integer,
    Invalid,
    String,
    Word
}


/*
 * The types of definitions used in COMFY files, this mainly for the interpreter. 
 */
public enum DefinitionType
{
    InformationDefinition,
    ItemDefinition,
    LanguageDefinition,
    PetDefinition,
    PlayerPawnDefinition,
    SoundDefinition,
    ThingDefinition,
    TextureDefinition,
    TileFloorDefinition,
    TileWallDefinition,
    Unknown
}


/* [TO BE CHANGED (?)]
 * The status of the game, may be useful for mods.
 */
public enum Status
{
    FatalError,
    StartingUp,
    Splash,
    MainMenu,
    InGame,
    PausedInGame
}


/* [TO BE CHANGED (?)]
 * Representation of the available fonts to the game.
 */
public enum FontType
{
    SmallText
}


/*
 * The different types of anchoring for controls, top and left are defaults as the controls will
 * stick to those sides normally.
 */
public enum AnchorType
{
    None,
    Right,
    Bottom,
    BottomRight,
    Centre
}