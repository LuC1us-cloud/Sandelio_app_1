using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Sandelio_app_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ExperimentalMethod();
        }

        private void ExperimentalMethod()
        {
            int max = 12;
            int y = 0;
            for (int i = 0; i < max; i++)
            {
                if (i % 3 == 0) y++;
                Border borderBox = new()
                {
                    Name = $"Box_{i}",
                    Width = 80,
                    Height = 20,
                    Background = new SolidColorBrush(Color.FromRgb((byte)(0 + (255 / max * i)), (byte)(0 + (255 / max * i)), 255)),
                    CornerRadius = new(5)
                };
                borderBox.MouseMove += BorderBox_MouseMove;
                Canvas.SetLeft(borderBox, 10 + (90 * y));
                Canvas.SetTop(borderBox, 30 + (30 * (i % 3)));
                _ = canvas.Children.Add(borderBox);

                Border borderBox1 = new()
                {
                    Name = $"Box_1{i}",
                    Width = 80,
                    Height = 20,
                    Background = new SolidColorBrush(Color.FromRgb((byte)(0 + (255 / max * i)), 255, (byte)(0 + (255 / max * i)))),
                    CornerRadius = new(5)
                };
                borderBox1.MouseMove += BorderBox_MouseMove;
                Canvas.SetLeft(borderBox1, 10 + (90 * y));
                Canvas.SetTop(borderBox1, 140 + (30 * (i % 3)));
                _ = canvas.Children.Add(borderBox1);

                Border borderBox2 = new()
                {
                    Name = $"Box_1{i}",
                    Width = 80,
                    Height = 20,
                    Background = new SolidColorBrush(Color.FromRgb(255, (byte)(0 + (255 / max * i)), (byte)(0 + (255 / max * i)))),
                    CornerRadius = new(5)
                };
                borderBox2.MouseMove += BorderBox_MouseMove;
                Canvas.SetLeft(borderBox2, 10 + (90 * y));
                Canvas.SetTop(borderBox2, 250 + (30 * (i % 3)));
                _ = canvas.Children.Add(borderBox2);
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