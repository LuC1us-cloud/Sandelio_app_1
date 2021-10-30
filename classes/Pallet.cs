using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sandelio_app_1.classes
{
    internal class Pallet
    {
        public int PalletNumber { get; set; }
        public float PalletWeight { get; set; }
        public int PalletMaxHeight { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public List<Item> itemsList { get; set; } = new();

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
            // cursor refers to current position of item placement on pallet
            int cursorPositionX = 0;
            int cursorPositionY = 0;
            // First largest item gets placed manually, because it's
            // Gonna be alone in it's row anyway
            int biggestItemIndex = items.FindIndex(x => x.Length == Length);
            Item largestItem = items[biggestItemIndex];
            items.RemoveAt(biggestItemIndex);
            itemsList.Add(largestItem);
            cursorPositionY += largestItem.Width;
            // Jeigu bus daugiau items, will create infinite loop
            for (int layer = 0; layer < 3; layer++)
            {
                while (true)
                {
                    while (true)
                    {
                        int spaceLeftX = Length - cursorPositionX;
                        IEnumerable<Item> tempList = items.Where(x => x.Length <= spaceLeftX);
                        if (!tempList.Any() || cursorPositionX >= Length)
                        {
                            cursorPositionX = 0;
                            cursorPositionY += itemsList[^1].Width;
                            break;
                        }
                        else
                        {
                            largestItem = tempList.OrderByDescending(x => x.Length).First();
                            biggestItemIndex = items.FindIndex(x => x.Length == largestItem.Length);
                            items.RemoveAt(biggestItemIndex);
                            largestItem.X = cursorPositionX;
                            largestItem.Y = cursorPositionY;
                            itemsList.Add(largestItem);
                            cursorPositionX += largestItem.Length;
                        }
                    }
                    if (items.Count == 0)
                    {
                        break;
                    }
                }
            }
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

        public override string ToString()
        {
            StringBuilder stringBuilder = new($"{PalletNumber} Pallet {Length}x{Width}\n");
            foreach (var item in itemsList)
            {
                stringBuilder.AppendLine($"{item}");
            }
            return stringBuilder.ToString();
        }
    }
}