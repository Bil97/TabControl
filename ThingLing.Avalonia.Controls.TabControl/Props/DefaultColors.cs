using Avalonia.Media;

namespace ThingLing.Avalonia.Controls.Properties
{
    internal static class DefaultColors
    {
        /// <summary>
        /// Holds the default Background color of the TabItem header when it is focused
        /// </summary>
        public static Brush FocusedTabItemBackground => new SolidColorBrush(Colors.Teal);

        /// <summary>
        /// Holds the default font color of the TabItem header when it is focused
        /// </summary>
        public static Brush FocusedTabItemForeground => new SolidColorBrush(Colors.Tan);

        /// <summary>
        /// Holds the default Background color of the TabItem header when it is not focused
        /// </summary>
        public static Brush UnFocusedTabItemBackground => new SolidColorBrush(Colors.CadetBlue);

        /// <summary>
        /// Holds the default font color of the TabItem header when it is not focused
        /// </summary>
        public static Brush UnFocusedTabItemForeground => new SolidColorBrush(Colors.BurlyWood);

        /// <summary>
        /// Holds the default font color of the SeparatorBorder
        /// </summary>
        public static Brush SeparatorBorder => new SolidColorBrush(Colors.Teal);

        /// <summary>
        /// Holds the default background color of the TabItem Body 
        /// </summary>
        public static Brush TabItemBodyBackground => new SolidColorBrush(Colors.SeaGreen);

        /// <summary>
        /// Holds the default font color of the TabItem Body
        /// </summary>
        public static Brush TabItemBodyForeground => new SolidColorBrush(Colors.PeachPuff);
    }
}