/**
 * RozWorld.StringIntPair -- RozWorld String - Integer Pair
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

/**
 * [ NOTICE! ]
 * 
 * THIS CODE IS TEMPORARY, IT WILL BE REMOVED IN FAVOUR OF TUPLE<T1, T2>!!
 */

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
