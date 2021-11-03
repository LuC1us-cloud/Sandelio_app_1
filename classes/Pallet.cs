using System.Collections.Generic;
using System.Diagnostics;
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
            Debug.WriteLine("Initialized");
            if (Length == 0 || Width == 0)
            {
                AutoSize(items);
            }
            // cursor refers to current position of item placement on pallet
            int cursorPositionX = 0;
            int cursorPositionY = 0;
            // First largest item gets placed manually, because it's
            // Gonna be alone in it's row anyway
            int biggestItemIndex = items.FindIndex(x => x.Length == Length);
            Item largestPossibleItem = items[biggestItemIndex];
            items.RemoveAt(biggestItemIndex);
            itemsList.Add(largestPossibleItem);
            cursorPositionY += largestPossibleItem.Width;
            while (true)
            {
                // Creates initial layer
                // Add that when creating a line it remembers max width of item in that line not last item placed
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
                        largestPossibleItem = tempList.OrderByDescending(x => x.Length).First();
                        biggestItemIndex = items.FindIndex(x => x.Length == largestPossibleItem.Length);
                        items.RemoveAt(biggestItemIndex);
                        largestPossibleItem.X = cursorPositionX;
                        largestPossibleItem.Y = cursorPositionY;
                        itemsList.Add(largestPossibleItem);
                        cursorPositionX += largestPossibleItem.Length;
                    }
                }
                // temp used to calculate if next row if items will be out of boundaries
                if(items.Count == 0)
                {
                    break;
                }
                int temp = cursorPositionY + items.Min(x => x.Width);
                if (temp >= Width)
                {
                    break;
                }
            }
            // Stacking items phase
            while (true)
            {
                bool inserted = false;
                foreach (Item item in itemsList)
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        inserted = item.AddItemOnStack(items[i]);
                        if (inserted)
                        {
                            items.RemoveAt(i);
                            break;
                        }
                    }
                }
                if (!inserted)
                {
                    break;
                }
            }
            // after stacking check if possible to expand pallet to accomodate more items
            if (items.Count > 0)
            {
                if (Width == 600)
                {
                    Width = 800;
                    items.AddRange(TakeAllItems());
                    return Initialize(items);
                }
                else if (Width == 800)
                {
                    Width = 1200;
                    items.AddRange(TakeAllItems());
                    return Initialize(items);
                }
                else if (Width == 1200)
                {
                    return items;
                }
            }
            return items;
        }

        /// <summary>
        /// Clears the pallet
        /// </summary>
        /// <returns>All cleared items</returns>
        public List<Item> TakeAllItems()
        {
            List<Item> temp = new();
            foreach (Item item in itemsList)
            {
                // Pops all children from the stack
                while (item.Child != null)
                {
                    temp.Add(item.PopStack());
                }
                // Adds the remaining item itself
                temp.Add(item);
            }
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
            foreach (Item item in itemsList)
            {
                _ = stringBuilder.AppendLine($"{item}");
            }
            return stringBuilder.ToString();
        }
    }
}