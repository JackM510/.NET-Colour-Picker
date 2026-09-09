using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Drawing;
using Color = System.Windows.Media.Color;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;

namespace net_colour_picker
{
    public partial class MainWindow : Window
    {
        private bool showHex = true;
        private string currentHex = "#000000";
        private string currentRgb = "0, 0, 0";
        
        public MainWindow()
        {
            InitializeComponent();
        }

        // Btn: Toggle Colour Format
        private void ToggleColourFormat(object sender, RoutedEventArgs e)
        {
            showHex = !showHex;
            ColourText.Content = showHex ? currentHex : currentRgb;
        }

        // Btn: Colour Picker
        private void StartColourPicker(object sender, RoutedEventArgs e)
        {
            SetCursorPicker();
            var overlay = new OverlayWindow();
            overlay.PixelClicked += OnPixelClicked;
            overlay.Show();
        }

        // Btn: Copy to Clipboard
        private async void CopyToClipboard(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(ColourText.Content.ToString());
            // Update Icons
            CopyIcon.Visibility = Visibility.Collapsed;
            CheckIcon.Visibility = Visibility.Visible;
            await Task.Delay(250);
            CheckIcon.Visibility = Visibility.Collapsed;
            CopyIcon.Visibility = Visibility.Visible;
        }

        // Check Luminance
        private bool CheckLuminance(Color c)
        {
            double l = (0.2126 * c.R) + (0.7152 * c.G) + (0.0722 * c.B);
            return l < 128;
        }

        // Determine Button & Path colours
        private Brush GetContrastBrush(Color c)
        {
            return CheckLuminance(c) ? Brushes.White : Brushes.Black;
        }

        // Set Button & Path colours
        private void SetBrushContrast(Object el, Brush b)
        {
            switch (el)
            {   
                // Btn Text
                case Control c:
                    c.Foreground = b;
                    break;
                // Icons
                case Shape s:
                    s.Fill = b;
                    break;
            }
        }

        // Set Cursor Cross
        private void SetCursorPicker()
        {
            Mouse.OverrideCursor = Cursors.Cross;
        }

        // Set Cursor Pointer
        private void SetCursorPointer()
        {
            Mouse.OverrideCursor = null;
        }

        // Colour Clicked
        private void OnPixelClicked(System.Windows.Point screenPos)
        {
            // Update cursor & get colour
            SetCursorPointer();
            var colour = GetPixelColour((int)screenPos.X, (int)screenPos.Y);
            currentHex = $"#{colour.R:X2}{colour.G:X2}{colour.B:X2}";
            currentRgb = $"{colour.R}, {colour.G}, {colour.B}";

            // Update background, Btn Text & Icons
            ColourWindow.Background = new SolidColorBrush(colour);
            ColourText.Content = showHex ? currentHex : currentRgb;
            var brush = GetContrastBrush(colour);
            SetBrushContrast(ColourText, brush);
            SetBrushContrast(PickerIcon, brush);
            SetBrushContrast(CopyIcon, brush);
            SetBrushContrast(CheckIcon, brush);
        }

        // Get Pixel Colour
        private Color GetPixelColour(int x, int y)
        {
            // Create empty 1x1 bitmap
            using (var bmap = new Bitmap(1,1))
            {   
                // Create graphics object from the bitmap
                using (var g = Graphics.FromImage(bmap))
                {
                    // Copy pixel from x,y onto the bitmap
                    g.CopyFromScreen(x, y, 0, 0, new System.Drawing.Size(1, 1));
                }
                // Read pixel colour from bitmap (returns RGB)
                var c = bmap.GetPixel(0, 0);
                return Color.FromRgb(c.R, c.G, c.B);
            }
        }
    }
}
