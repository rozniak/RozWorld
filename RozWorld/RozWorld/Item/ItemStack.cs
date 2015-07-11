/**
 * RozWorld.Item.ItemStack -- RozWorld Item Stack
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
    public class ItemStack
    {
        public Item Item;

        private int _Quantity;
        public int Quantity
        {
            get { return this._Quantity; }

            set
            {
                if (value > Item.MAX_QUANTITY)
                {
                    this._Quantity = Item.MAX_QUANTITY;
                }
                else if (value < Item.MIN_QUANTITY)
                {
                    this._Quantity = Item.MIN_QUANTITY;
                }
                else
                {
                    this._Quantity = value;
                }
            }
        }


        public ItemStack(Item item, int quantity = Item.MAX_QUANTITY)
        {
            Item = item;
            Quantity = quantity;
        }
    }
}
