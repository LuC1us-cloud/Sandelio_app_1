using Sandelio_app_1.classes;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Sandelio_app_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            tempTesting();
            InitializeComponent();
            ExperimentalMethod();
        }

        private const int drawingScale = 4;
        private static Pallet pallet = new(1);

        private static void tempTesting()
        {
            List<Item> list = new();
            list.Add(new(200, 2850, 2050, 50, "item1"));
            list.Add(new(115, 1250, 1400, 50, "item2"));
            list.Add(new(165, 1250, 1400, 50, "item3"));
            list.Add(new(115, 1250, 1400, 50, "item4"));
            list.Add(new(115, 1250, 1400, 50, "item5"));
            list.Add(new(115, 1680, 1780, 50, "item6"));
            list.Add(new(115, 952, 1000, 50, "item7"));
            list.Add(new(115, 952, 1000, 50, "item8"));
            for (int i = 9; i < 9; i++)
            {
                list.Add(new(115, 852, 852, 50, $"item{i}"));
            }
            list = pallet.Initialize(list);
            Debug.WriteLine($"Items returned {list.Count}");
            Debug.WriteLine(pallet);
        }

        private void ExperimentalMethod()
        {
            Border palletBorder = new()
            {
                Name = "Pallet",
                Width = pallet.Length/ drawingScale,
                Height = pallet.Width/ drawingScale,
                Background = Brushes.SandyBrown,
                CornerRadius = new(10)
            };
            Canvas.SetLeft(palletBorder, 47);
            Canvas.SetTop(palletBorder, 47);
            _ = canvas.Children.Add(palletBorder);

            for (int i = 0; i < pallet.itemsList.Count; i++)
            {
                Border borderBox = new()
                {
                    Name = $"Box_{i}",
                    Width = (pallet.itemsList[i].Length / drawingScale) - 5,
                    Height = (pallet.itemsList[i].Width / drawingScale) - 5,
                    Background = new SolidColorBrush(Color.FromRgb((byte)(0 + (255 / 10 * i)), (byte)(0 + (255 / 10 * i)), 255)),
                    CornerRadius = new(5),
                    Child = new Label()
                    {
                        Content = pallet.itemsList[i],
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontSize = pallet.itemsList[i].Width / 12,
                        FontWeight = FontWeights.DemiBold,
                        Background = Brushes.Transparent
                    }
                };
                borderBox.MouseMove += BorderBox_MouseMove;
                borderBox.Child.MouseMove += BorderBox_MouseMove;
                Canvas.SetLeft(borderBox, 50 + (pallet.itemsList[i].X / drawingScale));
                Canvas.SetTop(borderBox, 50 + (pallet.itemsList[i].Y / drawingScale));
                _ = canvas.Children.Add(borderBox);
            }
            //int max = 12;
            //int y = 0;
            //for (int i = 0; i < max; i++)
            //{
            //    if (i % 3 == 0)
            //    {
            //        y++;
            //    }

            //Border borderBox = new()
            //{
            //    Name = $"Box_{i}",
            //    Width = 80,
            //    Height = 20,
            //    Background = new SolidColorBrush(Color.FromRgb((byte)(0 + (255 / max * i)), (byte)(0 + (255 / max * i)), 255)),
            //    CornerRadius = new(5)
            //};
            //borderBox.MouseMove += BorderBox_MouseMove;
            //Canvas.SetLeft(borderBox, 10 + (90 * y));
            //Canvas.SetTop(borderBox, 30 + (30 * (i % 3)));
            //_ = canvas.Children.Add(borderBox);

            //Border borderBox1 = new()
            //{
            //    Name = $"Box_1{i}",
            //    Width = 80,
            //    Height = 20,
            //    Background = new SolidColorBrush(Color.FromRgb((byte)(0 + (255 / max * i)), 255, (byte)(0 + (255 / max * i)))),
            //    CornerRadius = new(5)
            //};
            //borderBox1.MouseMove += BorderBox_MouseMove;
            //Canvas.SetLeft(borderBox1, 10 + (90 * y));
            //Canvas.SetTop(borderBox1, 140 + (30 * (i % 3)));
            //_ = canvas.Children.Add(borderBox1);

            //Border borderBox2 = new()
            //{
            //    Name = $"Box_1{i}",
            //    Width = 80,
            //    Height = 20,
            //    Background = new SolidColorBrush(Color.FromRgb(255, (byte)(0 + (255 / max * i)), (byte)(0 + (255 / max * i)))),
            //    CornerRadius = new(5)
            //};
            //borderBox2.MouseMove += BorderBox_MouseMove;
            //Canvas.SetLeft(borderBox2, 10 + (90 * y));
            //Canvas.SetTop(borderBox2, 250 + (30 * (i % 3)));
            //_ = canvas.Children.Add(borderBox2);
            //}
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
            Window window = new();
            window.Owner = this;
            window.Height = 400;
            window.Width = 400;
            window.Background = new SolidColorBrush(Color.FromRgb(12, 14, 19));
            window.WindowStyle = WindowStyle.None;

            window.ShowDialog();
        }
    }
}