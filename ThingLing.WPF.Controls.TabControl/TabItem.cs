using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ThingLing.WPF.Controls.Props;

namespace ThingLing.WPF.Controls
{
    /// <summary>
    ///  Represents a selectable item inside a Thingling.WPF.Controls.TabControl.
    /// </summary>
    public class TabItem
    {
        private TabItemHeader _tabItemHeader = new TabItemHeader();
        private TabItemBody _tabItemBody = new TabItemBody();
        private Brush _backgroundWhenFocused;
        private Brush _backgroundWhenUnFocused;
        private Brush _foregroundWhenFocused;
        private Brush _foregroundWhenUnFocused;
        private UIElement _content;
        private Brush _tabItemBodyBackground;
        private static Brush _tabItemBodyForeground;

        /// <summary>
        /// Holds the Title text of the TabItem
        /// </summary>
        public string ContentTitle { get; set; }

        /// <summary>
        /// Holds the Icon of the content displayed in the TabItem body
        /// </summary>
        public Image ContentIcon { get; set; }

        /// <summary>
        /// Holds extra information displayed as tooltip in the TabItem header
        /// </summary>
        public string ToolTip { get; set; }

        /// <summary>
        /// Holds the content displayed in the TabItem body
        /// </summary>
        public UIElement Content
        {
            get => _content;
            set
            {
                _content = value;
                _content.Focusable = true;
            }
        }

        /// <summary>
        /// Indicates whether change has occured in the TabItem body. It is show by a * in the TabItem header
        /// </summary>
        public bool ContentChanged { get; set; }

        /// <summary>
        /// Holds the Background color of the TabItem header when it is focused
        /// </summary>
        public Brush BackgroundWhenFocused
        {
            get => _backgroundWhenFocused ??= CurrentTheme.FocusedTabItemBackground;
            set => _backgroundWhenFocused = value;
        }

        /// <summary>
        /// Holds the Background color of the TabItem header when it is not focused
        /// </summary>
        public Brush BackgroundWhenUnFocused
        {
            get => _backgroundWhenUnFocused ??= CurrentTheme.UnFocusedTabItemBackground;
            set => _backgroundWhenUnFocused = value;
        }

        /// <summary>
        /// Holds the font color of the TabItem header when it is focused
        /// </summary>
        public Brush ForegroundWhenFocused
        {
            get => _foregroundWhenFocused ??= CurrentTheme.FocusedTabItemForeground;
            set => _foregroundWhenFocused = value;
        }

        /// <summary>
        /// Holds the font color of the TabItem header when it is not focused
        /// </summary>
        public Brush ForegroundWhenUnFocused
        {
            get => _foregroundWhenUnFocused ??= CurrentTheme.UnFocusedTabItemForeground;
            set => _foregroundWhenUnFocused = value;
        }

        /// <summary>
        /// Holds the default background color of the TabItem Body 
        /// </summary>
        public Brush TabItemBodyBackground
        {
            get => _tabItemBodyBackground ??= CurrentTheme.TabItemBodyBackground;
            set => _tabItemBodyBackground = value;
        }

        /// <summary>
        /// Holds the default font color of the TabItem Body
        /// </summary>
        public static Brush TabItemBodyForeground
        {
            get => _tabItemBodyForeground ??= CurrentTheme.TabItemBodyForeground;
            set => _tabItemBodyForeground = value;
        }

        /// <summary>
        /// Holds the content displayed in the TabItem Header
        /// Do <bold>NOT</bold> use this property in your project! It is for internal use with the ThingLing DockControl
        /// </summary>
        public TabItemHeader TabItemHeader()
        {
            _tabItemHeader.ContentIcon = ContentIcon;
            _tabItemHeader.ContentTitle.Text = ContentTitle;
            _tabItemHeader.ContentTitle.ToolTip = ToolTip;
            _tabItemHeader.ContentChanged.Visibility = ContentChanged ? Visibility.Visible : Visibility.Collapsed;
            _tabItemHeader.Background = BackgroundWhenFocused;
            _tabItemHeader.Foreground = ForegroundWhenFocused;
            return _tabItemHeader;
        }

        /// <summary>
        /// Holds the content displayed in the TabItem Body. 
        /// Do <bold>NOT</bold> use this property in your project! It is for internal use with the ThingLing DockControl
        /// </summary>
        public TabItemBody TabItemBody()
        {
            _tabItemBody.TabItemHeader.ContentIcon = ContentIcon;
            _tabItemBody.TabItemHeader.ContentTitle.Text = ContentTitle;
            _tabItemBody.TabItemHeader.ContentTitle.ToolTip = ToolTip;
            _tabItemBody.TabItemHeader.ContentChanged.Visibility = ContentChanged ? Visibility.Visible : Visibility.Collapsed;
            _tabItemBody.TabItemHeader.Background = BackgroundWhenFocused;
            _tabItemBody.TabItemHeader.Foreground = ForegroundWhenFocused;
            _tabItemBody.ContentPanel.Background = TabItemBodyBackground;
            _tabItemBody.Foreground = TabItemBodyForeground;
            return _tabItemBody;
        }
    }
}
