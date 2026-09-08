using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Drawing;

namespace net_colour_picker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Colour Picker Button
        private void ColourPickerBtn(object sender, RoutedEventArgs e)
        {
            StartPickerMode();
        }

        // Secondary Click - Pixel Selected
        private void StartPickerMode()
        {
            MouseDown += Window_MouseDown;
        }

        // MouseDown
        private void Window_MouseDown(object sender, RoutedEventArgs e)
        {
            // Mouse Position relative to window
            var pos = Mouse.GetPosition(this);
            // Screen coords
            var screenPos = PointToScreen(pos);

            var colour = GetPixelColour((int)Math.Round(screenPos.X), (int)Math.Round(screenPos.Y));

            // Update ColourLabel
            ColourLabel.Foreground = new SolidColorBrush(colour);
            MouseDown -= Window_MouseDown;
        }

        private System.Windows.Media.Color GetPixelColour(int x, int y)
        {
            // Create a 1x1 bitmap and auto-dispose when finished
            // Bitmap is a tiny image stored in memory - which we can read the pixels colour from
            using (var bmap = new Bitmap(1,1))
            {   
                // Create graphics object that can copy pixel from Bitmap
                using (var g = System.Drawing.Graphics.FromImage(bmap))
                {
                    // Copy pixel from the screen at position (x,y)
                    g.CopyFromScreen(x, y, 0, 0, new System.Drawing.Size(1, 1));
                }
                // Read the colour of the pixel (returns Color object with RGB values)
                var c = bmap.GetPixel(0, 0);
                return System.Windows.Media.Color.FromRgb(c.R, c.G, c.B);
            }
        }
    }
}
