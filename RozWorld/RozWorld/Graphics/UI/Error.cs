﻿/**
 * RozWorld.Graphics.UI.Error -- RozWorld Error Code Constants
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

namespace RozWorld.Graphics.UI
{
    public static class Error
    {
        public const short UNKNOWN_ERROR = 0;
        public const short MISSING_CRITICAL_FILES = 1;
        public const short BROKEN_GUIOMETRY_FILE = 2;
        public const short BROKEN_FONT_LINK_FILE = 3;
        public const short INVALID_GUI_DICTIONARY_KEY = 10;
        public const short MISSING_INI_DICTIONARY_KEY = 11;
        public const short MISSING_FONT_DICTIONARY_KEY = 12;
    }
}
