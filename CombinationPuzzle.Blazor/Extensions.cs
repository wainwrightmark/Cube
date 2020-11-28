using System;

namespace CombinationPuzzle.Blazor
{
    public static class Extensions
    {
        public static string ToHtml(this FaceColor? color)
        {
            var dColor = color switch
            {
                null => System.Drawing.Color.MediumPurple,
                FaceColor.D => System.Drawing.Color.FloralWhite,
                FaceColor.R => System.Drawing.Color.Green,
                FaceColor.F => System.Drawing.Color.Red,
                FaceColor.L => System.Drawing.Color.Blue,
                FaceColor.B => System.Drawing.Color.Orange,
                FaceColor.U => System.Drawing.Color.Yellow,
                _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
            };

            return ToHtml(dColor);

            static string ToHtml(System.Drawing.Color c)
            {
                return $"rgb({c.R},{c.G},{c.B})";
            }
        }
    }
}
