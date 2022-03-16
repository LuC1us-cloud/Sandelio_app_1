using Microsoft.WindowsAPICodePack.Dialogs;
using Sandelio_app_1.classes;
using Sandelio_app_1.controllers;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Sandelio_app_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    // Still need to fix pallet item width in one line problem
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            FileIO.LoadSettings();
            InitializeComponent();
        }

        private const int drawingScale = 4;
        private static Pallet pallet = new(1);

        // Not used, left as reference for visualizing the pallet
        private void ExperimentalMethod()
        {
            Border palletBorder = new()
            {
                Name = "Pallet",
                Width = pallet.Length / drawingScale,
                Height = pallet.Width / drawingScale,
                Background = Brushes.SandyBrown,
                CornerRadius = new(10)
            };
            Canvas.SetLeft(palletBorder, 47);
            Canvas.SetTop(palletBorder, 47);
            _ = canvas.Children.Add(palletBorder);

            for (int i = 0; i < pallet.ItemsList.Count; i++)
            {
                Border borderBox = new()
                {
                    Name = $"Box_{i}",
                    Width = (pallet.ItemsList[i].Length / drawingScale) - 5,
                    Height = (pallet.ItemsList[i].Width / drawingScale) - 5,
                    Background = new SolidColorBrush(Color.FromRgb((byte)(0 + (255 / 10 * i)), (byte)(0 + (255 / 10 * i)), 255)),
                    CornerRadius = new(5),
                    Child = new Label()
                    {
                        Content = pallet.ItemsList[i],
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontSize = pallet.ItemsList[i].Width / 12,
                        FontWeight = FontWeights.DemiBold,
                        Background = Brushes.Transparent
                    }
                };
                borderBox.MouseMove += BorderBox_MouseMove;
                borderBox.Child.MouseMove += BorderBox_MouseMove;
                Canvas.SetLeft(borderBox, 50 + (pallet.ItemsList[i].X / drawingScale));
                Canvas.SetTop(borderBox, 50 + (pallet.ItemsList[i].Y / drawingScale));
                _ = canvas.Children.Add(borderBox);
            }
        }

        public static Point offsetPoint = new();

        private void BorderBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop((DependencyObject)e.Source, new DataObject(DataFormats.Serializable, e.Source), DragDropEffects.Move);
            }
        }

        public static int zIndex;

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            Point dropPosition = e.GetPosition(canvas);
            object data = e.Data.GetData(DataFormats.Serializable);
            if (data is UIElement element)
            {
                zIndex++;
                Canvas.SetLeft(element, dropPosition.X);
                Canvas.SetTop(element, dropPosition.Y);
                Panel.SetZIndex(element, zIndex);
            }
        }

        // settigs menu opener
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new(this);
            _ = settingsWindow.ShowDialog();
        }

        private List<Pallet> pallets;
        // open folder button click
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LoadingLabel.Content = "Loading...";
            // Open a file dialog and get file path to a json file, then save that path to a string
            string filepath = null;
            CommonOpenFileDialog openFolderDialog = new();
            openFolderDialog.InitialDirectory = "c:\\";
            openFolderDialog.RestoreDirectory = true;
            openFolderDialog.IsFolderPicker = true;
            openFolderDialog.Title = "Browse folders";
            if (openFolderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                filepath = openFolderDialog.FileName;
            }
            // If the file path is not null, then read the file and deserialize it to a list of orders
            if (filepath != null)
            {
                List<Order> orders = FileIO.ReadFile(filepath + "\\" + "orders.json");
                string loadingDate = orders[0].LoadingDate;
                string clientInfo = orders[0].ClientInfo;
                // If the list of orders is not null, then create a list of pallets from the list of orders
                if (orders != null)
                {
                    pallets = FileIO.CreatePallets(orders, filepath);
                    // If the list of pallets is not null, then create a list of pallets from the list of pallets
                    if (pallets != null)
                    {
                        // Sets the current view number to 'first' pallet
                        currentPalletNumber.Content = 1;
                        // -----------------------------

                        // Starts the Excel file generation
                        ExcelController.CreateFile(pallets, clientInfo, loadingDate);
                    }
                }
            }
            LoadingLabel.Content = "";
        }

        private int currentPalletViewNumber;

        private void Button_Previous_Pallet_View_Click(object sender, RoutedEventArgs e)
        {
            if (currentPalletViewNumber == 0) return;
            currentPalletViewNumber--;
            currentPalletNumber.Content = currentPalletViewNumber + 1;
        }

        private void Button_Next_Pallet_View_Click(object sender, RoutedEventArgs e)
        {
            if (currentPalletViewNumber >= pallets.Count - 1) return;
            currentPalletViewNumber++;
            currentPalletNumber.Content = currentPalletViewNumber + 1;
        }

        private void Button_Add_New_Pallet(object sender, RoutedEventArgs e)
        {
            pallets.Add(new(pallets.Count));
        }

        private void Button_Remove_Current_Pallet(object sender, RoutedEventArgs e)
        {
            pallets.RemoveAt(currentPalletViewNumber - 1);
        }
    }
}