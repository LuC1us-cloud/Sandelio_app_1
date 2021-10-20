using static Sandelio_app_1.controllers.Global_data;

namespace Sandelio_app_1.classes
{
    internal class Item
    {
        public Item Parent { get; set; }
        public Item Child { get; set; }
        public int LayerHeight { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }

        //private readonly string clientInfo;
        //private readonly string name;
        //private readonly string picture;
        //private readonly int orderNumber;
        //private readonly int positionNumber;
        private readonly int width; // x
        private readonly int length; // y
        private readonly int height; // z
        private readonly int weight; // grams
        //private readonly int amount;
        //private readonly bool isAlone;
        public int Width => width + (Margin * 2);
        public int Length => length + (Margin * 2);
        public int Height => height + (Margin * 2);

        public Item(int width, int length, int height, int margin)
        {
            this.width = width;
            this.length = length;
            this.height = height;
            Margin = margin;
        }

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
        private static bool FitsOnTop(Item bottom, Item top)
        {
            return bottom.width >= top.width && bottom.length >= top.length;
        }
        // Dar pridet kad atsizvelgtu i auksti
        public bool AddItemOnStack(Item item)
        {
            if (Child is null)
            {
                if (LayerHeight is 0 && item.weight > MaxStackWeight && !FitsOnTop(this, item))
                {
                    return false;
                }
                else if (LayerHeight is 1 && (weight + item.weight) > MaxStackWeight && !FitsOnTop(this, item))
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
        public void RemoveFromStack()
        {

        }
    }
}