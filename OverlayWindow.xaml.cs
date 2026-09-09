using System;
using System.Windows;
using System.Windows.Input;

namespace net_colour_picker
{
    public partial class OverlayWindow : Window
    {
        public event Action<Point> PixelClicked;
        public OverlayWindow()
        {
            InitializeComponent();
            // Cover ALL monitors
            Left = SystemParameters.VirtualScreenLeft;
            Top = SystemParameters.VirtualScreenTop;
            Width = SystemParameters.VirtualScreenWidth;
            Height = SystemParameters.VirtualScreenHeight;
        }
        public void Overlay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(this); // Get overlay click position
            var screenPos = PointToScreen(pos); // Convert to screen coords
            this.Hide(); 
            PixelClicked?.Invoke(screenPos); // Pass coords to OnPixelClicked()
            this.Close();
        }
    }
}