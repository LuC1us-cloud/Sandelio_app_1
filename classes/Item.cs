using System.Collections.Generic;
using System.Drawing;
using static Sandelio_app_1.controllers.Settings;

namespace Sandelio_app_1.classes
{
    internal class Picture
    {
        private readonly string path;
        private int height;
        private int width;

        public Picture(string path)
        {
            this.path = path;
        }

        public string Path => path;

        public int Height
        {
            get
            {
                GetDimensionsFromFile();
                return height;
            }
        }

        public int Width
        {
            get
            {
                GetDimensionsFromFile();
                return width;
            }
        }

        // if height or width is empty then load image dimensions
        private void GetDimensionsFromFile()
        {
            if (height == 0 || width == 0)
            {
                height = new Bitmap(path).Height;
                width = new Bitmap(path).Width;
            }
        }

        public override bool Equals(object obj)
        {
            // compare if the path is the same
            return obj is Picture picture &&
                   path == picture.path;
        }

        public override string ToString()
        {
            return Path;
        }
    }

    internal class Item
    {
        public Item Parent { get; set; }
        public Item Child { get; set; }
        public int LayerHeight { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        private readonly string clientInfo;
        private readonly string name;

        public Picture Picture
        { get { return picture; } }
        private readonly Picture picture;
        private readonly string orderNumber;
        public string OrderNumber => orderNumber;
        private readonly int positionNumber;
        private readonly int width; // x

        private readonly int length; // y
        private readonly int height; // z
        private readonly int weight; // kg

        //private readonly int amount;
        //private readonly bool isAlone;
        public int Width => width + (Margin * 2);

        public int Length => length + (Margin * 2);
        public int Height => height + (Margin * 2);
        /// <summary>
        /// Single item to be placed on a pallet
        /// </summary>
        /// <param name="width">Width in mm</param>
        /// <param name="length">Length in mm</param>
        /// <param name="height">Height in mm</param>
        /// <param name="margin">Margin around items in mm</param>
        /// <param name="weight">Weight in kg</param>
        /// <param name="name">Name of the item</param>

        public Item(string name, int width, int length, int height, int weight, string picture, string orderNumber)
        {
            this.width = width;
            this.length = length;
            this.height = height;
            this.weight = weight;
            this.name = name;
            this.picture = new(picture);
            this.orderNumber = orderNumber;
        }

        /// <summary>
        /// Gets stack weight
        /// </summary>
        /// <returns>Weight in kg of this item plus all on top</returns>
        public int GetStackWeight()
        {
            int totalWeight = 0;
            totalWeight += weight;
            if (LayerHeight is 0 or 1)
            {
                if (Child is not null)
                {
                    totalWeight += Child.GetStackWeight();
                }
            }
            return totalWeight;
        }

        /// <summary>
        /// Gets stack Height
        /// </summary>
        /// <returns>This plus all items on top height</returns>
        public int GetStackHeight(List<int> visitedLayers = null)
        {
            int totalHeight = 0;
            if (visitedLayers == null)
            {
                visitedLayers = new List<int>();
            }

            if (visitedLayers.Contains(LayerHeight))
            {
                return totalHeight;
            }
            else
            {
                visitedLayers.Add(LayerHeight);
            }

            totalHeight += height;
            if (Child is not null)
            {
                totalHeight += Child.GetStackHeight(visitedLayers);
            }
            if (Parent is not null)
            {
                totalHeight += Parent.GetStackHeight(visitedLayers);
            }
            return totalHeight;
        }

        /// <summary>
        /// Checks if bottom is larger than top
        /// </summary>
        /// <param name="bottom">Item that will be the bottom</param>
        /// <param name="top">Item that will be the top</param>
        /// <returns>True if the top item fits on the bottom one</returns>
        private static bool FitsOnTop(Item bottom, Item top)
        {
            return bottom.GetStackHeight() + top.GetStackHeight() <= MaxStackHeight
                && bottom.width >= top.width && bottom.length >= top.length;
        }

        /// <summary>
        /// Adds an item to the item stack if possible
        /// </summary>
        /// <param name="item">Item to be added</param>
        /// <returns>True if the stacking was successful</returns>
        public bool AddItemOnStack(Item item)
        {
            if (Child is null)
            {
                if (LayerHeight is 0)
                {
                    // MaxStackWeight is the maximum possible weight stacked on an item, that means not including the base item
                    if (item.weight > MaxStackWeight || !FitsOnTop(this, item))
                    {
                        return false;
                    }
                }
                else if (LayerHeight is 1)
                {
                    if ((weight + item.weight) > MaxStackWeight || !FitsOnTop(this, item))
                    {
                        return false;
                    }
                }
                else if (LayerHeight > 1)
                {
                    return false;
                }
                item.LayerHeight = LayerHeight + 1;
                item.Parent = this;
                Child = item;
                return true;
            }
            else
            {
                return LayerHeight is 0 or 1 && Child.AddItemOnStack(item);
            }
        }

        /// <summary>
        /// Pops top most item from stack
        /// </summary>
        public Item PopStack()
        {
            if (Child != null)
            {
                return Child.PopStack();
            }
            else
            {
                if (Parent != null)
                {
                    Parent.Child = null;
                    Parent = null;
                }
                return this;
            }
        }

        /// <summary>
        /// To string formatting
        /// </summary>
        /// <returns>String representing full stack info</returns>
        public override string ToString()
        {
            string temp = $"{name}({orderNumber}) ";
            if (Child is not null)
            {
                temp += "/ " + Child.ToString();
            }
            return temp;
        }
    }
}