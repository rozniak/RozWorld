//
// RozWorld.Enums -- RozWorld Enumerations File
//
// This source-code is part of the RozWorld project by rozza of Oddmatics:
// <<http://www.oddmatics.co.uk>>
// <<http://www.oddmatics.co.uk/projects/rozworld>>
//
// Sharing, editing and general licence term information can be found inside of the "sup.txt" file that should be located in the root of this project's directory structure.
//

public enum ItemType
{
    Weapon,
    Bucket
}

public enum SoundType
{
    Music,
    Sound
}

public enum NatureType
{
    Passive,
    Hostile
}

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

public enum DamageMethod
{
    Generic,
    Explosion,
    Potion,
    Weapon,
    Pet
}

public enum StripType
{
    None,
    SemiColons,
    WindowsSafe,
    Both
}

public enum ParsedObjectType
{
    Integer,
    Invalid,
    String,
    Word
}

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

public enum Status
{
    FatalError,
    StartingUp,
    Splash,
    MainMenu,
    InGame,
    PausedInGame
}

public enum FontType
{
    SmallText
}