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
        private MainWindow mainWindow;
        public SettingsWindow()
        {
            InitializeComponent();
        }
        public SettingsWindow(MainWindow callingForm)
        {
            mainWindow = callingForm;
            Owner = (Window)Parent;
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
            CountriesList.SelectedIndex = controllers.Settings.SelectedConfigIndex;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Add a new country to the dictionary with the key value "New Country"
            controllers.Settings.CreateNewCountry();
            // Update the listbox
            CountriesList.Items.Refresh();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // Delete the selected country from the dictionary
            controllers.Settings.DeleteCountry(CountriesList.SelectedIndex);
            CountriesList.Items.Refresh();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            controllers.Settings.SelectedConfigIndex = CountriesList.SelectedIndex;
            controllers.FileIO.SaveSettings();
            Close();
        }

        private void CountriesList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // if index out of bounds, set index to 0
            if (CountriesList.SelectedIndex < 0 || CountriesList.SelectedIndex >= controllers.Settings.countriesList.Count)
            {
                CountriesList.SelectedIndex = 0;
                return;
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
            }
            catch (Exception)
            {
                MessageBox.Show($"Error parsing data {Environment.NewLine}Make sure all fields are filled in correctly {Environment.NewLine}and that the data is in the correct format");
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
                MaxStackWeightTextBox.Text = controllers.Settings.countriesList[CountriesList.SelectedIndex].MaxStackWeight.ToString();
                MaxStackHeightTextBox.Text = controllers.Settings.countriesList[CountriesList.SelectedIndex].MaxStackHeight.ToString();
                PalletHeightTextBox.Text = controllers.Settings.countriesList[CountriesList.SelectedIndex].PalletHeight.ToString();
                string palletWidth = "";
                foreach (int i in controllers.Settings.countriesList[CountriesList.SelectedIndex].PalletWidth)
                {
                    palletWidth += i.ToString() + ", ";
                }
                // remove trailing ", "
                palletWidth = palletWidth[0..^2];
                PalletWidthTextBox.Text = palletWidth;
                IsAloneTextBox.Text = controllers.Settings.countriesList[CountriesList.SelectedIndex].IsAlone.ToString();
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

        private void SaveConfigButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsDataValid())
            {
                controllers.Settings.countriesList[CountriesList.SelectedIndex].ConfigName = ConfigNameTextBox.Text;
                controllers.Settings.countriesList[CountriesList.SelectedIndex].MaxStackWeight = int.Parse(MaxStackWeightTextBox.Text);
                controllers.Settings.countriesList[CountriesList.SelectedIndex].MaxStackHeight = int.Parse(MaxStackHeightTextBox.Text);
                controllers.Settings.countriesList[CountriesList.SelectedIndex].PalletHeight = int.Parse(PalletHeightTextBox.Text);
                string[] palletWidth = PalletWidthTextBox.Text.Split(',');
                int[] palletWidthInt = new int[palletWidth.Length];
                for (int i = 0; i < palletWidth.Length; i++)
                {
                    palletWidthInt[i] = int.Parse(palletWidth[i]);
                }
                controllers.Settings.countriesList[CountriesList.SelectedIndex].PalletWidth = palletWidthInt;
                controllers.Settings.countriesList[CountriesList.SelectedIndex].IsAlone = bool.Parse(IsAloneTextBox.Text);
            }
            //refresh view
            CountriesList.Items.Refresh();
        }
    }
}