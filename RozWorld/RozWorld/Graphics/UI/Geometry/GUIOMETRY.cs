/**
 * RozWorld.Graphics.UI.Geometry.GUIOMETRY -- RozWorld UI Geometry
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RozWorld.Graphics.UI.Geometry
{
    public class GUIOMETRY
    {
        private Dictionary<string, FontInfo> Fonts = new Dictionary<string, FontInfo>();
        private Dictionary<string, ElementInfo> Elements = new Dictionary<string, ElementInfo>();

        public bool CentredTextButton = false;
        public sbyte OffsetButtonTop = 0;
        public sbyte OffsetButtonLeft = 0;

        public bool CentredTextText = false;
        public sbyte OffsetTextTop = 0;
        public sbyte OffsetTextLeft = 0;


        public GUIOMETRY()
        {
            if (Fonts.Count == 0 || Elements.Count == 0)
                BuildKeys();
        }


        /// <summary>
        /// Initialises all font and element keys in their respective Dictionary collections.
        /// </summary>
        private void BuildKeys()
        {
            // Add font keys
            Fonts.Add("ChatFont", new FontInfo());
            Fonts.Add("SmallFont", new FontInfo());
            Fonts.Add("MediumFont", new FontInfo());
            Fonts.Add("HugeFont", new FontInfo());

            // Add element keys

            // Button ElementInfos
            Elements.Add("ButtonBody", new ElementInfo());
            Elements.Add("ButtonTop", new ElementInfo());
            Elements.Add("ButtonSide", new ElementInfo());
            Elements.Add("ButtonEdgeSE", new ElementInfo());
            Elements.Add("ButtonEdgeSW", new ElementInfo());

            // TextBox ElementInfos
            Elements.Add("TextBody", new ElementInfo());
            Elements.Add("TextTop", new ElementInfo());
            Elements.Add("TextSide", new ElementInfo());
            Elements.Add("TextEdgeSE", new ElementInfo());
            Elements.Add("TextEdgeSW", new ElementInfo());

            // CheckBox ElementInfos
            Elements.Add("CheckBody", new ElementInfo());
            Elements.Add("CheckTop", new ElementInfo());
            Elements.Add("CheckSide", new ElementInfo());
            Elements.Add("CheckEdgeSE", new ElementInfo());
            Elements.Add("CheckEdgeSW", new ElementInfo());
            Elements.Add("CheckTick", new ElementInfo());
        }
    }
}
