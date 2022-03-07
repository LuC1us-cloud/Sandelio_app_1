using System;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace Sandelio_app_1
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private int lastIndex = 0;
        private bool listInitialized = false;
        private bool modifiedList = false;
        private MainWindow mainWindow;
        public SettingsWindow()
        {
            InitializeComponent();
        }
        public SettingsWindow(MainWindow callingForm)
        {
            mainWindow = callingForm;
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            InitializeLocalData();
            base.OnInitialized(e);
        }

        private void InitializeLocalData()
        {
            CountriesList.ItemsSource = controllers.Settings.countriesList;
            // initialize text boxes with values from countrieslist[0]
            ConfigNameTextBox.Text = controllers.Settings.countriesList[0].ConfigName;
            MaxStackWeightTextBox.Text = controllers.Settings.countriesList[0].MaxStackWeight.ToString();
            MaxStackHeightTextBox.Text = controllers.Settings.countriesList[0].MaxStackHeight.ToString();
            PalletHeightTextBox.Text = controllers.Settings.countriesList[0].PalletHeight.ToString();
            string palletWidth = "";
            foreach (int i in controllers.Settings.countriesList[0].PalletWidth)
            {
                palletWidth += i.ToString() + ", ";
            }
            //remove last ", "
            palletWidth = palletWidth.Remove(palletWidth.Length - 2);
            PalletWidthTextBox.Text = palletWidth;
            IsAloneTextBox.Text = controllers.Settings.countriesList[0].IsAlone.ToString();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Add a new country to the dictionary with the key value "New Country"
            controllers.Settings.CreateNewCountry();
            modifiedList = true;
            // Update the listbox
            CountriesList.Items.Refresh();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // Delete the selected country from the dictionary
            modifiedList = true;
            controllers.Settings.DeleteCountry(CountriesList.SelectedIndex);
            CountriesList.Items.Refresh();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!IsDataValid())
            {
                return;
            }
            CountriesList.SelectedIndex = CountriesList.SelectedIndex;
            controllers.FileIO.SaveSettings();
            Close();
        }

        private void CountriesList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (!listInitialized)
            {
                listInitialized = true;
                return;
            }
            if (modifiedList)
            {
                modifiedList = false;
                return;
            }

            if(CountriesList.SelectedIndex != -1)
            {
                if (!IsDataValid())
                {
                    CountriesList.SelectedIndex = lastIndex;
                    return;
                }
            }
            // if index out of bounds, set index to 0
            if (CountriesList.SelectedIndex < 0 || CountriesList.SelectedIndex >= controllers.Settings.countriesList.Count)
            {
                CountriesList.SelectedIndex = 0;
            }
            ConfigNameTextBox.Text = controllers.Settings.countriesList[CountriesList.SelectedIndex].ConfigName;
            MaxStackWeightTextBox.Text = controllers.Settings.countriesList[CountriesList.SelectedIndex].MaxStackWeight.ToString();
            MaxStackHeightTextBox.Text = controllers.Settings.countriesList[CountriesList.SelectedIndex].MaxStackHeight.ToString();
            PalletHeightTextBox.Text = controllers.Settings.countriesList[CountriesList.SelectedIndex].PalletHeight.ToString();
            string palletWidth = "";
            foreach (int i in controllers.Settings.countriesList[CountriesList.SelectedIndex].PalletWidth)
            {
                palletWidth += i.ToString() + ", ";
            }
            // remove trailing ", "
            palletWidth = palletWidth.Substring(0, palletWidth.Length - 2);

            PalletWidthTextBox.Text = palletWidth;
            IsAloneTextBox.Text = controllers.Settings.countriesList[CountriesList.SelectedIndex].IsAlone.ToString();
            
            lastIndex = CountriesList.SelectedIndex;
        }

        private bool IsDataValid()
        {
            if(controllers.Settings.countriesList.Count == 0)
            {
                ConfigNameTextBox.Text = "";
                MaxStackWeightTextBox.Text = "";
                MaxStackHeightTextBox.Text = "";
                PalletHeightTextBox.Text = "";
                PalletWidthTextBox.Text = "";
                IsAloneTextBox.Text = "";
                MessageBox.Show("You need to add at least one country to the list before saving.");
                return false;
            }
            if (ConfigNameTextBox.Text == "")
            {
                MessageBox.Show("Config name cannot be empty");
                ConfigNameTextBox.Focus();
                ConfigNameTextBox.Text = controllers.Settings.countriesList[CountriesList.SelectedIndex].ConfigName;
                return false;
            }
            if (MaxStackWeightTextBox.Text == "")
            {
                MessageBox.Show("Max stack weight cannot be empty");
                MaxStackWeightTextBox.Focus();
                MaxStackWeightTextBox.Text = controllers.Settings.countriesList[CountriesList.SelectedIndex].MaxStackWeight.ToString();
                return false;
            }
            if (MaxStackHeightTextBox.Text == "")
            {
                MessageBox.Show("Max stack height cannot be empty");
                MaxStackHeightTextBox.Focus();
                MaxStackHeightTextBox.Text = controllers.Settings.countriesList[CountriesList.SelectedIndex].MaxStackHeight.ToString();
                return false;
            }
            if (PalletHeightTextBox.Text == "")
            {
                MessageBox.Show("Pallet height cannot be empty");
                PalletHeightTextBox.Focus();
                PalletHeightTextBox.Text = controllers.Settings.countriesList[CountriesList.SelectedIndex].PalletHeight.ToString();
                return false;
            }
            if (PalletWidthTextBox.Text == "")
            {
                MessageBox.Show("Pallet width cannot be empty");
                PalletWidthTextBox.Focus();
                PalletWidthTextBox.Text = controllers.Settings.countriesList[CountriesList.SelectedIndex].PalletWidth.ToString();
                return false;
            }
            if (IsAloneTextBox.Text == "")
            {
                MessageBox.Show("Is alone cannot be empty");
                IsAloneTextBox.Focus();
                IsAloneTextBox.Text = controllers.Settings.countriesList[CountriesList.SelectedIndex].IsAlone.ToString();
                return false;
            }
            // try parsing stack weight, height, pallet height to int, if error show error message
            try
            {
                int maxStackWeight = int.Parse(MaxStackWeightTextBox.Text);
                int maxStackHeight = int.Parse(MaxStackHeightTextBox.Text);
                int palletHeight = int.Parse(PalletHeightTextBox.Text);
                string[] palletWidth = PalletWidthTextBox.Text.Split(',');
                if(palletWidth.Length != 3)
                {
                    throw new Exception();
                }
                int[] palletWidthInt = new int[palletWidth.Length];
                for (int i = 0; i < palletWidth.Length; i++)
                {
                    palletWidthInt[i] = int.Parse(palletWidth[i]);
                }
                bool isAlone = bool.Parse(IsAloneTextBox.Text);
                controllers.Settings.countriesList[lastIndex].ConfigName = ConfigNameTextBox.Text;
                controllers.Settings.countriesList[lastIndex].MaxStackWeight = maxStackWeight;
                controllers.Settings.countriesList[lastIndex].MaxStackHeight = maxStackHeight;
                controllers.Settings.countriesList[lastIndex].PalletHeight = palletHeight;
                controllers.Settings.countriesList[lastIndex].PalletWidth = palletWidthInt;
                controllers.Settings.countriesList[lastIndex].IsAlone = isAlone;
            }
            catch (Exception)
            {
                MessageBox.Show("Error parsing data");
                // reset textboxes
                if(controllers.Settings.countriesList.Count == 0)
                {
                    ConfigNameTextBox.Text = "";
                    MaxStackWeightTextBox.Text = "";
                    MaxStackHeightTextBox.Text = "";
                    PalletHeightTextBox.Text = "";
                    PalletWidthTextBox.Text = "";
                    IsAloneTextBox.Text = "";
                    return false;
                }
                MaxStackWeightTextBox.Text = controllers.Settings.countriesList[lastIndex].MaxStackWeight.ToString();
                MaxStackHeightTextBox.Text = controllers.Settings.countriesList[lastIndex].MaxStackHeight.ToString();
                PalletHeightTextBox.Text = controllers.Settings.countriesList[lastIndex].PalletHeight.ToString();
                string palletWidth = "";
                foreach (int i in controllers.Settings.countriesList[lastIndex].PalletWidth)
                {
                    palletWidth += i.ToString() + ", ";
                }
                // remove trailing ", "
                palletWidth = palletWidth[0..^2];
                PalletWidthTextBox.Text = palletWidth;
                IsAloneTextBox.Text = controllers.Settings.countriesList[lastIndex].IsAlone.ToString();
                return false;
            }
            return true;
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}