# .NET Colour Picker
A lightweight utility built with .NET and WPF for quickly sampling colours anywhere on your screen. Click the picker icon, select any pixel, and the app displays the colour in both HEX and RGB formats. Copy the colour value instantly using the clipboard button.

## Demo Screenshots
<div style="display: flex;">
  <img src="demo/demo-screenshot.png" width="auto" />
</div>

## Features
- **Screen Colour Picker** — select any pixel on your screen and instantly capture its colour.
- **HEX / RGB Toggle** — switch between HEX and RGB formats with a single click.
- **Clipboard Copy** — copy the current colour value and get visual confirmation via a tick icon.

## How It Works
- **Initialises Fullscreen Overlay** — clicking the picker button creates a transparent overlay used to capture your screen click.
- **Captures Pixel Position** — when you click anywhere on the screen, the overlay sends the exact screen coordinates back to the main window.
- **Reads Pixel Colour** — `GetPixelColour` creates a 1×1 bitmap, uses `CopyFromScreen` to paint the pixel, and extracts the RGB values.
- **Updates Displayed Colour** — the main window updates its background and shows the colour in HEX or RGB format.
- **Toggles Colour Format** — clicking the colour text switches between HEX and RGB values.
- **Copies to Clipboard** — pressing the clipboard button copies the current value and briefly shows a tick icon for confirmation.

## Tech Stack
- C# (.NET)
- WPF (XAML)

## Installation
1. **Clone the Repository** — clone the project to your local machine.
2. **Open Project** — open the solution in Visual Studio and build/run.

## Usage
- **Colour Sampling** — pick any pixel on your screen using the picker button and instantly view its HEX or RGB value.
- **Copy to Clipboard** — copy the displayed colour value with one click for use in other applications.

## Future Improvements
- Additional colour formats (HSL, HSV, CMYK)
- Magnifier preview while picking
- Colour history