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

        // Not used, left as reference for visualizing the pallet
        private void ExperimentalMethod(Pallet pallet)
        {
            canvas.Children.Clear();
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

        private void BorderBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _ = DragDrop.DoDragDrop((DependencyObject)e.Source, new DataObject(DataFormats.Serializable, e.Source), DragDropEffects.Copy);
            }
        }

        private static int zIndex;

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            Point dropPosition = e.GetPosition(canvas);
            // get the item that was dragged
            object data = e.Data.GetData(DataFormats.Serializable);
            if (data is UIElement element)
            {
                zIndex++;
                Canvas.SetLeft(element, dropPosition.X);
                Canvas.SetTop(element, dropPosition.Y);
                Panel.SetZIndex(element, zIndex);
            }
        }

        // settings menu opener
        private void Button_Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new(this);
            _ = settingsWindow.ShowDialog();
        }

        private List<Pallet> pallets;
        // open folder button click
        private void Button_Open_Folder(object sender, RoutedEventArgs e)
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
                // copy the contents of orders to tempOrders for later reference
                List<Order> tempOrders = new();
                tempOrders.AddRange(orders);
                string loadingDate = orders[0].LoadingDate;
                string clientInfo = orders[0].ClientInfo;
                // If the list of orders is not null, then create a list of pallets from the list of orders
                if (orders != null)
                {
                    pallets = FileIO.CreatePallets(orders, filepath);
                    ChangeInfoPanelValues(tempOrders, pallets);
                    // If the list of pallets is not null, then create a list of pallets from the list of pallets
                    if (pallets != null)
                    {
                        // Sets the current view number to 'first' pallet
                        ChangeCurrentPalletView(0);
                        // -----------------------------

                        // Starts the Excel file generation
                        //ExcelController.CreateFile(pallets, clientInfo, loadingDate);
                    }
                }
            }
            LoadingLabel.Content = "";
        }

        private void ChangeInfoPanelValues(List<Order> orders, List<Pallet> pallets)
        {
            string orderCount = $"Orders: {orders.Count}";
            string palletCount = $"Pallets: {pallets.Count}";
            string loadingDate = $"Date: {orders[0].LoadingDate}";
            string clientInfo = $"Client info: {orders[0].ClientInfo}";
            string clientName = $"Client name: {orders[0].ClientName}";
            int totalItemCount = 0;
            foreach (var pallet in pallets)
            {
                totalItemCount += pallet.ItemsList.Count;
            }
            string totalItems = $"Items: {totalItemCount}";

            OrderCountLabel.Content = orderCount;
            PalletCountLabel.Content = palletCount;
            LoadingDateLabel.Content = loadingDate;
            ClientInfoLabel.Content = clientInfo;
            ClientNameLabel.Content = clientName;
            TotalItemsLabel.Content = totalItems;
        }

        private void ChangeCurrentPalletView(int palletIndex)
        {
            currentPalletViewNumber = palletIndex;

            string content = $"{palletIndex + 1}/{pallets.Count}";
            currentPalletNumber.Content = content;
            currentPalletLength.Content = $"Length: {pallets[palletIndex].Length}";
            currentPalletWidth.Content = $"Width: {pallets[palletIndex].Width}";

            ExperimentalMethod(pallets[palletIndex]);
        }

        private int currentPalletViewNumber;

        private void Button_Previous_Pallet_View_Click(object sender, RoutedEventArgs e)
        {
            if (pallets is null) return;
            if (currentPalletViewNumber == 0) return;
            currentPalletViewNumber--;
            ChangeCurrentPalletView(currentPalletViewNumber);
        }

        private void Button_Next_Pallet_View_Click(object sender, RoutedEventArgs e)
        {
            if (pallets is null) return;
            if (currentPalletViewNumber >= pallets.Count - 1) return;
            currentPalletViewNumber++;
            ChangeCurrentPalletView(currentPalletViewNumber);
        }

        private void Button_Add_New_Pallet(object sender, RoutedEventArgs e)
        {
            if (pallets is null) return;
            pallets.Add(new(pallets.Count));
            currentPalletViewNumber = pallets.Count - 1;
            ChangeCurrentPalletView(currentPalletViewNumber);
        }

        private void Button_Remove_Current_Pallet(object sender, RoutedEventArgs e)
        {
            if (pallets is null) return;
            if (pallets.Count == 1) return;
            pallets.RemoveAt(currentPalletViewNumber);
            currentPalletViewNumber--;
            if (currentPalletViewNumber < 0) currentPalletViewNumber = 0;
            ChangeCurrentPalletView(currentPalletViewNumber);
        }

        private void Button_Generate_Excel_Sheet(object sender, RoutedEventArgs e)
        {
            //ExperimentalMethod();
        }
    }
}