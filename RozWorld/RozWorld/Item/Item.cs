//
// RozWorld.Item.Item -- RozWorld Item
//
// This source-code is part of the RozWorld project by rozza of Oddmatics:
// <<http://www.oddmatics.co.uk>>
// <<http://www.oddmatics.co.uk/projects/rozworld>>
//
// Sharing, editing and general licence term information can be found inside of the "sup.txt" file that should be located in the root of this project's directory structure.
//

namespace RozWorld.Item
{
    public class Item
    {
        public const int MAX_QUANTITY = 99;
        public const int MIN_QUANTITY = 0;

        public string DisplayName;
        public string ItemName;


        public Item(string itemName)
        {
            ItemName = itemName;
        }

        public Item(string itemName, string displayName)
        {
            ItemName = itemName;
            DisplayName = displayName;
        }
    }
}
