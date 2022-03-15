using Sandelio_app_1.controllers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Sandelio_app_1.classes
{
    internal class Pallet
    {
        public int PalletNumber { get; set; }
        public string LoadingDate { get; set; }
        public string ClientName { get; set; }
        public string Adress { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string OrderNumber { get => GetOrderNumber(); }
        public float PalletWeight { get; set; }
        public int PalletMaxHeight { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<Item> ItemsList { get; set; } = new();

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
            // If the pallet is empty, initialize it
            if (Length == 0 || Width == 0) AutoSize(items);

            // cursor refers to current position of item placement on pallet
            int cursorPositionX = 0;
            int cursorPositionY = 0;
            // First largest item gets placed manually, because it's
            // Gonna be alone in it's row anyway
            int biggestItemIndex = items.FindIndex(x => x.Length == Length);
            // Gets the biggest item
            Item largestPossibleItem = items[biggestItemIndex];
            largestPossibleItem.X = cursorPositionX;
            largestPossibleItem.Y = cursorPositionY;
            // Removes the item from the list
            items.RemoveAt(biggestItemIndex);
            // Adds the item to the pallet
            ItemsList.Add(largestPossibleItem);
            // Sets the cursor to the next line position
            cursorPositionY += largestPossibleItem.Width;
            while (true)
            {
                // Creates initial layer
                // Add that when creating a line it remembers max width of item in that line not last item placed
                int largestWidthInLine = 0;
                while (true)
                {
                    // Calculates horizontal space left in that line of the pallet
                    int spaceLeftX = Length - cursorPositionX;
                    // Filters out items that are too big to fit in the space left
                    IEnumerable<Item> tempList = items.Where(x => x.Length <= spaceLeftX);
                    // if no items left in the list or cursor is beyond the bounds of pallet, break out of the loop
                    if (!tempList.Any() || cursorPositionX >= Length)
                    {
                        // resets X position
                        cursorPositionX = 0;
                        // moves Y down as much as largest Width of item in that line
                        cursorPositionY += largestWidthInLine;
                        break;
                    }
                    else
                    {
                        // Orders items by length and then takes the longest one
                        largestPossibleItem = tempList.OrderByDescending(x => x.Length).First();
                        // Finds that item's index in the list, needed for operations
                        biggestItemIndex = items.FindIndex(x => x.Length == largestPossibleItem.Length);
                        // Removes that item from the list
                        items.RemoveAt(biggestItemIndex);
                        // Sets that item's X and Y positions to the cursor
                        largestPossibleItem.X = cursorPositionX;
                        largestPossibleItem.Y = cursorPositionY;
                        // Adds that item to the list of items on the pallet
                        ItemsList.Add(largestPossibleItem);
                        // Moves cursor to the right as much as the item's Length
                        cursorPositionX += largestPossibleItem.Length;
                        // Adds horizontal margin between items
                        cursorPositionX += Settings.HorizontalMargin;
                        // Updates largest Y as much as the item's Width if the width is bigger than the current largest
                        if (largestPossibleItem.Width > largestWidthInLine) largestWidthInLine = largestPossibleItem.Width;
                    }
                }
                // If there are no items left in the list, break out of the loop
                if (items.Count == 0) break;

                // Check if there is enough space for the next item
                int nextRowCursorPositionY = cursorPositionY + items.Min(x => x.Width);
                if (nextRowCursorPositionY >= Width) break;
            }
            // Stacking items phase
            while (true)
            {
                bool inserted = false;
                // Foreach already placed item on the pallet
                foreach (Item item in ItemsList)
                {
                    // Check if any of the items can be stacked on top
                    for (int i = 0; i < items.Count; i++)
                    {
                        // True if the item can be stacked on top
                        inserted = item.AddItemOnStack(items[i]);

                        // If the item was stacked, remove it from the list
                        if (inserted)
                        {
                            items.RemoveAt(i);
                            break;
                        }
                    }
                }
                // If no items were stacked, break out of the loop
                if (!inserted) break;
            }

            // after stacking check if possible to expand pallet to accomodate more items
            // Not really efficient but calculations are still in the hundreds of cycles, so it's fine
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
        /// Calculates the total weight of all items on the pallet
        /// </summary>
        /// <returns>the weight of the pallet in kg</returns>
        public int GetPalletWeight()
        {
            int weight = 0;
            foreach (Item item in ItemsList)
            {
                weight += item.GetStackWeight();
            }
            return weight;
        }

        /// <summary>
        /// Clears the pallet
        /// </summary>
        /// <returns>All cleared items</returns>
        public List<Item> TakeAllItems()
        {
            List<Item> temp = new();
            foreach (Item item in ItemsList)
            {
                // Pops all children from the stack
                while (item.Child != null)
                {
                    temp.Add(item.PopStack());
                }
                // Adds the remaining item itself
                temp.Add(item);
            }
            ItemsList = new();
            return temp;
        }
        /// <summary>
        /// Iterates through all items on the pallet and builds their order number string
        /// </summary>
        /// <returns>The order number string</returns>
        private string GetOrderNumber()
        {
            // iterate through all items and their children on the pallet
            // add their order number to a list, then remove duplicates and join the list to a string
            List<string> orderNumberList = new();
            foreach (Item item in ItemsList)
            {
                var currentItem = item;
                // if item.ClientName is empty or null, string clientName should be empty, else it should be $"({item.ClientName})"
                string clientName = string.IsNullOrEmpty(item.ClientName) ? "" : $"({item.ClientName})";
                orderNumberList.Add($"{currentItem.OrderNumber}{clientName}");
                while (currentItem.Child != null)
                {
                    clientName = string.IsNullOrEmpty(currentItem.Child.ClientName) ? "" : $"({currentItem.Child.ClientName})";
                    orderNumberList.Add($"{currentItem.Child.OrderNumber}{clientName}");
                    currentItem = currentItem.Child;
                }
            }
            orderNumberList = orderNumberList.Distinct().ToList();
            string temp = string.Join(", ", orderNumberList);
            
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
                    Width = Settings.PalletWidth[0];
                    break;

                case < 3000:
                    Width = Settings.PalletWidth[1];
                    break;

                case >= 3000:
                    Width = Settings.PalletWidth[2];
                    break;

                default:
            }
        }

        /// <summary>
        /// Get total height of the pallet
        /// </summary>
        /// <returns>Height of pallet in cm</returns>
        public int GetTotalHeight()
        {
            // find largest item.GetStackHeight() value and add Settings.PalletHeight to it, then return it
            // Debug each item.GetStackHeight() from ItemsList and print to Debug console
            foreach (Item item in ItemsList)
            {
                Debug.WriteLine($"{item.GetStackHeight()} of {item.ToString()}");
            }
            Height = ItemsList.Max(x => x.GetStackHeight()) + Settings.PalletHeight;
            //foreach (Item item in ItemsList)
            //{
            //    Debug.WriteLine(item.GetStackHeight());
            //}
            return Height;
        }

        /// <summary>
        /// Returns a string array with all the strings reprensenting the pallet and it's contents
        /// </summary>
        public string[] ToStringArray()
        {
            // sort ItemList by Y.
            string[] tempStringArray = new string[ItemsList.Count];
            // temp item list is used to track the Y index of items, so you can tell when the pallet changes rows
            List<Item> tempItemList = ItemsList.OrderBy(x => x.Y).ToList();
            int tempHeight = tempItemList.First().Y;
            int tempIndex = 0;
            for (int i = 0; i < tempItemList.Count; i++)
            {
                if (tempHeight != tempItemList[i].Y)
                {
                    tempHeight = tempItemList[i].Y;
                    tempIndex++;
                }
                tempStringArray[tempIndex] += tempItemList[i].ToString() + " + ";
            }
            // remove null strings
            tempStringArray = tempStringArray.Where(x => x != null).ToArray();
            // remove trailing " + " from each string
            for (int i = 0; i < tempStringArray.Length; i++)
            {
                tempStringArray[i] = tempStringArray[i].Remove(tempStringArray[i].Length - 3);
            }
            return tempStringArray;
        }

        /// <summary>
        /// Gets all pictures of items on the pallet
        /// </summary>
        /// <returns>An array with all pictures</returns>
        public Picture[] GetPictures()
        {
            // iterate through ItemsList and add each item's drawing to the array, then remove duplicate entries
            Picture[] pictureArray = new Picture[ItemsList.Count];
            int tempIndex = 0;
            foreach (Item item in ItemsList)
            {
                pictureArray[tempIndex] = item.Picture;
                tempIndex++;
            }
            // remove duplicate entries
            List<string> uniqueEntries = new();
            foreach (Picture picture in pictureArray)
            {
                if (!uniqueEntries.Contains(picture.Path))
                {
                    uniqueEntries.Add(picture.Path);
                }
            }
            Picture[] temp = new Picture[uniqueEntries.Count];
            for (int i = 0; i < uniqueEntries.Count; i++)
            {
                for (int j = 0; j < pictureArray.Length; j++)
                {
                    if (uniqueEntries[i] == pictureArray[j].Path)
                    {
                        temp[i] = pictureArray[j];
                    }
                }
            }
            return temp;
        }

        /// <summary>
        /// Gets LDM of the pallet
        /// </summary>
        /// <returns>LDM of the pallet</returns>
        public float GetLDM()
        {
            // convert width and length to meters
            // then multiply width and length and divide by 2.4
            // plus 50 on Length is added because of the extra borders of the pallet when shipping
            float ldm = Width / 1000f * (Length + 50f) / 1000f / 2.4f;
            // format number to 0.00
            ldm = (float)Math.Round(ldm, 2);
            return ldm;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new($"{PalletNumber} Pallet {Length}x{Width}\n");
            foreach (Item item in ItemsList)
            {
                _ = stringBuilder.AppendLine($"{item}");
            }
            return stringBuilder.ToString();
        }
    }
}