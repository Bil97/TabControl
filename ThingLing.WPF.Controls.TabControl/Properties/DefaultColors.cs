using System.Windows.Media;

namespace ThingLing.WPF.Controls.Properties
{
    internal static class DefaultColors
    {
        /// <summary>
        /// Holds the default Background color of the TabItem header when it is focused
        /// </summary>
        public static Brush FocusedTabItemBackground => Brushes.Teal;

        /// <summary>
        /// Holds the default font color of the TabItem header when it is focused
        /// </summary>
        public static Brush FocusedTabItemForeground => Brushes.Tan;

        /// <summary>
        /// Holds the default Background color of the TabItem header when it is not focused
        /// </summary>
        public static  Brush UnFocusedTabItemBackground =>Brushes.CadetBlue;
        
        /// <summary>
        /// Holds the default font color of the TabItem header when it is not focused
        /// </summary>
        public static Brush UnFocusedTabItemForeground => Brushes.BurlyWood;
        
        /// <summary>
        /// Holds the default font color of the SeparatorBorder
        /// </summary>
        public static Brush SeparatorBorder => Brushes.Teal;
        
        /// <summary>
        /// Holds the default background color of the TabItem Body 
        /// </summary>
        public  static  Brush TabItemBodyBackground=>Brushes.SeaGreen;
        
        /// <summary>
        /// Holds the default font color of the TabItem Body
        /// </summary>
        public  static  Brush TabItemBodyForeground=>Brushes.PeachPuff;
    }
}