namespace Sandelio_app_1.controllers
{
    public static class Settings
    {
        public static int Margin { get; set; } = 5;
        public static int MaxStackWeight { get; set; } = 60;
        public static int MaxStackHeight { get; set; } = 2650;
        public static int PalletHeight { get; set; } = 100;
        public static int[] PalletWidth { get; set; } = { 600, 800, 1200 };
    }
}