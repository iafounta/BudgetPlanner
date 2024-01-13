using SkiaSharp;

namespace BudgetPlanner.Presentation.Helpers
{
    public static class ColorUtilsHelper
    {
        private static readonly Random random = new Random();
        public static SKColor GetRandomColor()
        {
            // Generate random numbers for RGB values
            int r = random.Next(256); // Red: 0-255
            int g = random.Next(256); // Green: 0-255
            int b = random.Next(256); // Blue: 0-255

            // Convert to hexadecimal string
            string hexColor = $"#{r:X2}{g:X2}{b:X2}";

            // Parse and return the SKColor
            return SKColor.Parse(hexColor);
        }
    }
}

