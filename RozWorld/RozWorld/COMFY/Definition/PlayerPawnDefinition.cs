/**
 * RozWorld.COMFY.Definition.PlayerPawnDefinition -- RozWorld COMFY Player Pawn Definition
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

namespace RozWorld.COMFY.Definition
{
    public class PlayerPawnDefinition
    {
        public double BaseSpeed;
        public int BaseStrength;
        public int BaseDefence;
        public double MaxSpeed;
        public int Health;
        public int MaxLevel;

        public PlayerPawnDefinition()
        {
            BaseSpeed = 1.1;
            BaseStrength = 10;
            BaseDefence = 10;
            MaxSpeed = 2.5;
            Health = 100;
            MaxLevel = 300;
        }
    }
}
