

using System;
using System.Collections.Generic;

namespace YourMoviesForum.Services.Providers.Background
{
    public static class BackgroundProvider
    {
        private static List<string> BackgroundColors =
            new List<string> { "#3C79B2", "#FF8F88", "#6FB9FF", "#C0CC44", "AFB28C", "#8B0000", "#808080", "#FFFACD", "#66CDAA", "#800000", "#4169E1", "#2E8B57" };
        public static string BackgroundPicker()
        {
            var randomIndex = new Random().Next(0, BackgroundColors.Count - 1);
            var bgColor = BackgroundColors[randomIndex];

            return bgColor;
        }
    }
}
