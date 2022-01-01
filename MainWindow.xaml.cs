using Microsoft.Win32;
using Sandelio_app_1.classes;
using Sandelio_app_1.controllers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
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
            FileIO.WriteOrders(FileIO.GenerateOrders(), "items.json");
            InitializeComponent();
        }

        private const int drawingScale = 4;
        private static Pallet pallet = new(1);


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

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new();
            settingsWindow.ShowDialog();
        }
        // Top most button click
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<Order> a = FileIO.GenerateOrders();
            List<Pallet> b = FileIO.CreatePallets(a);
            pallet = b[0];
            ExperimentalMethod();
            ExcelController.CreateFile(b, a[0].Name);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Open a file dialog and get file path to a json file, then save that path to a string
            string filePath = null;
            OpenFileDialog openFileDialog = new();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Documents (*.json)|*.json";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
            }
            // If the file path is not null, then read the file and deserialize it to a list of orders
            if (filePath != null)
            {
                List<Order> orders = FileIO.ReadFile(filePath);
                // If the list of orders is not null, then create a list of pallets from the list of orders
                if (orders != null)
                {
                    List<Pallet> pallets = FileIO.CreatePallets(orders);
                    // If the list of pallets is not null, then create a list of pallets from the list of pallets
                    if (pallets != null)
                    {
                        pallet = pallets[0];
                        ExperimentalMethod();
                        ExcelController.CreateFile(pallets, orders[0].Name);
                    }
                }
            }

        }
    }
}