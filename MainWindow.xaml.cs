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



using System.Runtime.InteropServices;
using System.Drawing;

namespace net_colour_picker
{
    public partial class MainWindow : Window
    {
        private bool showHex = true;
        private string currentHex = "#000000";
        private string currentRgb = "0, 0, 0";
        
        [DllImport("user32.dll")]
        static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);

        [DllImport("user32.dll")]
        static extern IntPtr SetCursor(IntPtr hCursor);

        const int IDC_CROSS = 32515;
        const int IDC_ARROW = 32512;


        public MainWindow()
        {
            InitializeComponent();
        }

        /* ----- Button: Toggle Colour Format ----- */
        private void ToggleColourFormat(object sender, RoutedEventArgs e)
        {
            showHex = !showHex;
            ColourLabel.Content = showHex ? currentHex : currentRgb;
        }

        /* ----- Button: Copy to Clipboard ----- */
        private void CopyToClipboard(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(ColourLabel.Content.ToString());
        }

        /* ----- Button: Colour Picker ----- */
        private void ColourPickerBtn(object sender, RoutedEventArgs e)
        {
            StartPickerMode();
        }

        /* ----- Toggle Picker Mode ----- */
        private void StartPickerMode()
        {
            SetCursorPicker();
            var overlay = new OverlayWindow();
            overlay.PixelClicked += OnGlobalPixelClicked;
            overlay.Show();

        }

        /* ----- Check Luminance ----- */
        private bool CheckLuminance(System.Windows.Media.Color c)
        {
            double l = (0.2126 * c.R) + (0.7152 * c.G) + (0.0722 * c.B);
            return l < 128;
        }



        private void SetCursorPicker()
        {
            SetCursor(LoadCursor(IntPtr.Zero, IDC_CROSS));
        }

        private void SetCursorPointer()
        {
            SetCursor(LoadCursor(IntPtr.Zero, IDC_ARROW));
        }

        /*private void Window_MouseDown(object sender, RoutedEventArgs e)
        {

            var pos = Mouse.GetPosition(this);
            var screenPos = PointToScreen(pos);
            var colour = GetPixelColour((int)Math.Round(screenPos.X), (int)Math.Round(screenPos.Y));
            // Update ColourLabel
            ColourLabel.Foreground = new SolidColorBrush(colour);
            MouseDown -= Window_MouseDown;
        }*/

        private void OnGlobalPixelClicked(System.Windows.Point screenPos)
        {
            // Get pixel colours
            var colour = GetPixelColour((int)screenPos.X, (int)screenPos.Y);
            currentHex = $"#{colour.R:X2}{colour.G:X2}{colour.B:X2}";
            currentRgb = $"{colour.R}, {colour.G}, {colour.B}";
            // Change label colour
            ColourWindow.Background = new SolidColorBrush(colour);
            ColourLabel.Content = showHex ? currentHex : currentRgb;
            ColourLabel.Foreground = CheckLuminance(colour)
                ? System.Windows.Media.Brushes.White
                : System.Windows.Media.Brushes.Black;
            PickerIcon.Fill = CheckLuminance(colour)
                ? System.Windows.Media.Brushes.White
                : System.Windows.Media.Brushes.Black;
            CopyIcon.Fill = CheckLuminance(colour)
                ? System.Windows.Media.Brushes.White
                : System.Windows.Media.Brushes.Black;
        }

        /* ----- Get Pixel Colour ----- */
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
