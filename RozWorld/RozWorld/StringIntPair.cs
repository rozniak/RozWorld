//
// RozWorld.StringIntPair -- RozWorld String - Integer Pair
//
// This source-code is part of the RozWorld project by rozza of Oddmatics:
// <<http://www.oddmatics.co.uk>>
// <<http://www.oddmatics.co.uk/projects/rozworld>>
//
// Sharing, editing and general licence term information can be found inside of the "sup.txt" file that should be located in the root of this project's directory structure.
//

namespace RozWorld
{
    public struct StringIntPair
    {
        public string StringValue;
        public int IntegerValue;


        public StringIntPair(string stringValue, int integerValue)
        {
            StringValue = stringValue;
            IntegerValue = integerValue;
        }
    }
}
