/**
 * RozWorld.Network.Chat.ChatFormatting -- RozWorld Chat Formatting Options
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

namespace RozWorld.Network.Chat
{
    public class ChatFormatting
    {
        public string MessageFormat;
        public bool AllowColours;
        public string Prefix;
        public string Suffix;


        public ChatFormatting()
        {
            MessageFormat = "%prefix%%grpre%%name%%suffix%%message%";
            AllowColours = true;
            Prefix = "<";
            Suffix = ">";
        }

        public ChatFormatting(string messageFormat, bool allowColours, string prefix, string suffix)
        {
            MessageFormat = messageFormat;
            AllowColours = allowColours;
            Prefix = prefix;
            Suffix = suffix;
        }
    }
}
