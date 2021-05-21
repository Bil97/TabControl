using System.Windows.Media;
using ThingLing.WPF.Controls.Props;

namespace ThingLing.WPF.Controls.Methods
{
    class LoadTheme
    {
        public LoadTheme(TabControlTheme theme = null)
        {
            if (theme == null)
            {
                CurrentTheme.FocusedTabItemBackground = Brushes.Teal;
                CurrentTheme.FocusedTabItemForeground = Brushes.Tan;
                CurrentTheme.UnFocusedTabItemBackground = Brushes.CadetBlue;
                CurrentTheme.UnFocusedTabItemForeground = Brushes.BurlyWood;
                CurrentTheme.SeparatorBorder = Brushes.Teal;
                CurrentTheme.TabItemBodyBackground = Brushes.SeaGreen;
                CurrentTheme.TabItemBodyForeground = Brushes.PeachPuff;
            }
            else
            {
                CurrentTheme.FocusedTabItemBackground = theme.FocusedTabItemBackground;
                CurrentTheme.FocusedTabItemForeground = theme.FocusedTabItemForeground;
                CurrentTheme.UnFocusedTabItemBackground = theme.UnFocusedTabItemBackground;
                CurrentTheme.UnFocusedTabItemForeground = theme.UnFocusedTabItemForeground;
                CurrentTheme.SeparatorBorder = theme.SeparatorBorder;
                CurrentTheme.TabItemBodyBackground = theme.TabItemBodyBackground;
                CurrentTheme.TabItemBodyForeground = theme.TabItemBodyForeground;
            }
        }
    }
}
