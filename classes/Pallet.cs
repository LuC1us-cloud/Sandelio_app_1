using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandelio_app_1.classes
{
    internal class Pallet
    {
        public int PalletNumber { get; set; }
        public float PalletWeight { get; set; }
        public float PalletMaxHeight { get; set; }
        public float Length { get; set; }
        public float Width { get; set; }
        private List<Item> itemsList { get; set; } = new();
        public Pallet(int number)
        {
            PalletNumber = number;
        }
        /// <summary>
        /// Initializes the pallet for the first time
        /// </summary>
        /// <param name="items">Items to try ant stack on the pallet</param>
        /// <returns>List with unused items in the stacking process</returns>
        public List<Item> Initialize(List<Item> items)
        {
            AutoSize(items);

            return null;
        }
        /// <summary>
        /// Clears the pallet
        /// </summary>
        /// <returns>All cleared items</returns>
        public List<Item> TakeAllItems()
        {
            List<Item> temp = itemsList;
            itemsList = new();
            return temp;
        }
        /// <summary>
        /// Determines the size of the pallet according to items being added to it
        /// </summary>
        /// <param name="items">Items to add on the pallet</param>
        private void AutoSize(List<Item> items)
        {
            int maxLength = items.Max(x => x.Length);
            Length = maxLength;
            switch (maxLength)
            {
                case < 1800:
                    Width = 600;
                    break;
                case < 3000:
                    Width = 800;
                    break;
                case >= 3000:
                    Width = 1200;
                    break;
                default:
            }
        }
    }
}
