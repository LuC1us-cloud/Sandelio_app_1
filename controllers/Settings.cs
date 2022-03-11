using System.Collections.Generic;

namespace Sandelio_app_1.controllers
{
    public class CountryOptions
    {
        public string ConfigName { get; set; } = "New Config";
        public int MaxStackWeight { get; set; } = 60;
        public int MaxStackHeight { get; set; } = 2650;
        public int PalletHeight { get; set; } = 200;
        public int[] PalletWidth { get; set; } = { 600, 800, 1200 };
        public bool IsAlone { get; set; }

        public CountryOptions()
        { 
            ConfigName = "New Config";
            MaxStackWeight = 60;
            MaxStackHeight = 2650;
            PalletHeight = 200;
            PalletWidth = new int[] { 600, 800, 1200 };
            IsAlone = false;
        }
        public override string ToString()
        {
            return ConfigName;
        }
    }

    public static class Settings
    {
        public static List<CountryOptions> countriesList = new();
        public static int SelectedConfigIndex { get; set; }
        public static int BoxMargin { get; set; } = 0;
        public static int HorizontalMargin { get; set; } = 50;
        public static int MaxStackWeight => countriesList[SelectedConfigIndex].MaxStackWeight;
        public static int MaxStackHeight => countriesList[SelectedConfigIndex].MaxStackHeight;
        public static int PalletHeight => countriesList[SelectedConfigIndex].PalletHeight;
        public static bool IsAlone => countriesList[SelectedConfigIndex].IsAlone;
        public static int[] PalletWidth => countriesList[SelectedConfigIndex].PalletWidth;

        public static void CreateNewCountry()
        {
            countriesList.Add(new CountryOptions());
        }

        public static void EditCountry(int index, string name, int maxStackWeight, int maxStackHeight, int palletHeight, int[] palletWidth, bool isAlone)
        {
            countriesList[index].ConfigName = name;
            countriesList[index].MaxStackWeight = maxStackWeight;
            countriesList[index].MaxStackHeight = maxStackHeight;
            countriesList[index].PalletHeight = palletHeight;
            countriesList[index].PalletWidth = palletWidth;
            countriesList[index].IsAlone = isAlone;
        }

        public static void DeleteCountry(int index)
        {
            // if index out of bounds return
            if (index < 0 || index >= countriesList.Count)
            {
                return;
            }
            countriesList.RemoveAt(index);
        }
    }
}