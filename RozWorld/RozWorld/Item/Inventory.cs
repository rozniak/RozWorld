/**
 * RozWorld.Item.Inventory -- RozWorld Inventory
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

namespace RozWorld.Item
{
    public class Inventory
    {
        private Item[] Items;


        public Inventory(int slots)
        {
            Items = new Item[slots];
        }

        public Inventory(Item[] items)
        {
            Items = items;
        }
    }
}
