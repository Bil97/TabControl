using Avalonia.Controls;
using Avalonia.Media;
using ThingLing.Avalonia.Controls.InternalControls;
using ThingLing.Avalonia.Controls.Properties;
using Tool_Tip = Avalonia.Controls.ToolTip;

namespace ThingLing.Avalonia.Controls
{
    /// <summary>
    ///  Represents a selectable item inside a ThingLing.Avalonia.Controls.TabControl.
    /// </summary>
    public class TabItem
    {
        private readonly TabItemHeader _tabItemHeader = new TabItemHeader();
        private readonly TabItemBody _tabItemBody = new TabItemBody();
        private Brush _backgroundWhenFocused;
        private Brush _backgroundWhenUnFocused;
        private Brush _foregroundWhenFocused;
        private Brush _foregroundWhenUnFocused;
        private Control _content;
        private Brush _tabItemBodyBackground;
        private static Brush _tabItemBodyForeground;

        /// <summary>
        /// Holds the Title of the TabItem
        /// </summary>
        public string Header { get; set; }

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
        public Control Content
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
            get => _backgroundWhenFocused ??= DefaultColors.FocusedTabItemBackground;
            set => _backgroundWhenFocused = value;
        }

        /// <summary>
        /// Holds the Background color of the TabItem header when it is not focused
        /// </summary>
        public Brush BackgroundWhenUnFocused
        {
            get => _backgroundWhenUnFocused ??= DefaultColors.UnFocusedTabItemBackground;
            set => _backgroundWhenUnFocused = value;
        }

        /// <summary>
        /// Holds the font color of the TabItem header when it is focused
        /// </summary>
        public Brush ForegroundWhenFocused
        {
            get => _foregroundWhenFocused ??= DefaultColors.FocusedTabItemForeground;
            set => _foregroundWhenFocused = value;
        }

        /// <summary>
        /// Holds the font color of the TabItem header when it is not focused
        /// </summary>
        public Brush ForegroundWhenUnFocused
        {
            get => _foregroundWhenUnFocused ??= DefaultColors.UnFocusedTabItemForeground;
            set => _foregroundWhenUnFocused = value;
        }

        /// <summary>
        /// Holds the default background color of the TabItem Body 
        /// </summary>
        public Brush TabItemBodyBackground
        {
            get => _tabItemBodyBackground ??= DefaultColors.TabItemBodyBackground;
            set => _tabItemBodyBackground = value;
        }

        /// <summary>
        /// Holds the default font color of the TabItem Body
        /// </summary>
        public static Brush TabItemBodyForeground
        {
            get => _tabItemBodyForeground ??= DefaultColors.TabItemBodyForeground;
            set => _tabItemBodyForeground = value;
        }

        /// <summary>
        /// Holds the content displayed in the TabItem Header
        /// </summary>
        internal TabItemHeader TabItemHeader()
        {
            _tabItemHeader.ContentIcon = ContentIcon;
            _tabItemHeader.ContentTitle.Text = Header;
            Tool_Tip.SetTip(_tabItemHeader, ToolTip);
            _tabItemHeader.ContentChanged.IsVisible = ContentChanged;
            _tabItemHeader.Background = BackgroundWhenFocused;
            _tabItemHeader.Foreground = ForegroundWhenFocused;
            return _tabItemHeader;
        }

        /// <summary>
        /// Holds the content displayed in the TabItem Body
        /// </summary>
        internal TabItemBody TabItemBody()
        {
            _tabItemBody.TabItemHeader.ContentIcon = ContentIcon;
            _tabItemBody.TabItemHeader.ContentTitle.Text = Header;
            Tool_Tip.SetTip(_tabItemBody, ToolTip);
            _tabItemBody.TabItemHeader.ContentChanged.IsVisible = ContentChanged;
            _tabItemBody.TabItemHeader.Background = BackgroundWhenFocused;
            _tabItemBody.TabItemHeader.Foreground = ForegroundWhenFocused;
            _tabItemBody.ContentPanel.Background = TabItemBodyBackground;
            _tabItemBody.Foreground = TabItemBodyForeground;
            _tabItemBody.ContentPanel.Children.Add(Content);
            return _tabItemBody;
        }
    }
}
