using static Sandelio_app_1.controllers.Settings;

namespace Sandelio_app_1.classes
{
    internal class Item
    {
        public Item Parent { get; set; }
        public Item Child { get; set; }
        public int LayerHeight { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }

        private readonly string clientInfo;
        private readonly string name;

        private readonly string picture;
        private readonly int orderNumber;
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

        public Item(int width, int length, int height, int margin, int weight, string name)
        {
            this.width = width;
            this.length = length;
            this.height = height;
            this.weight = weight;
            this.name = name;
            Margin = margin;
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
        public int GetStackHeight()
        {
            int totalHeight = 0;
            totalHeight += height;
            if (LayerHeight is 0 or 1)
            {
                if (Child is not null)
                {
                    totalHeight += Child.GetStackHeight();
                }
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
            return bottom.width >= top.width && bottom.length >= top.length;
        }

        // Dar pridet kad atsizvelgtu i auksti
        /// <summary>
        /// Adds an item to the item stack if possible
        /// </summary>
        /// <param name="item">Item to be added</param>
        /// <returns>True if the stacking was successful</returns>
        public bool AddItemOnStack(Item item)
        {
            if (Child is null)
            {
                // Don't like this, a bit scuffed
                if (LayerHeight is 0 && item.weight > MaxStackWeight && !FitsOnTop(this, item))
                {
                    return false;
                }
                else if (LayerHeight is 1 && (weight + item.weight) > MaxStackWeight && !FitsOnTop(this, item))
                {
                    return false;
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
        /// Removes item from stack and all items that were on top of it
        /// </summary>
        public void RemoveFromStack()
        {
        }
        /// <summary>
        /// To string formatting
        /// </summary>
        /// <returns>String representing full stack info</returns>
        public override string ToString()
        {
            string temp = $"Layer {LayerHeight}: {name}, {weight} | StackWeight: {GetStackWeight()}";
            if (Child is not null)
            {
                temp += "\n" + Child.ToString();
            }
            return temp;
        }
    }
}