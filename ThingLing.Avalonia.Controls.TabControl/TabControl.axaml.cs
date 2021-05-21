using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using ThingLing.Avalonia.Controls.InternalControls;
using ThingLing.Avalonia.Controls.Properties;
using ThingLing.Avalonia.ControlsProperties;

namespace ThingLing.Avalonia.Controls
{
    /// <summary>
    ///  Represents a control that contains multiple items that share the same space on the screen.
    /// </summary>
    public partial class TabControl : UserControl
    {
        /// <summary>
        /// Determines whether the TabControl arranges TabItems as windows or as documents
        /// </summary>
        public TabMode TabMode { get; set; }

        /// <summary>
        /// Determines the location of the TabStrip or TabItem headers; whether Top, Left, Bottom or Right
        /// </summary>
        public TabStripPlacement TabStripPlacement { get; set; }

        /// <summary>
        /// Determines the Angle of rotation of a TabIte
        /// </summary>
        public double? TabItemRotationAngle;

        /// <summary>
        /// Holds the default font color of the SeparatorBorder
        /// </summary>
        public Brush SeparatorBorderColor { get; set; }

        /// <summary>
        /// Do <bold>NOT</bold> use this property in your project!
        /// </summary>
        public static bool ShowDockButtons { get; set; } = false;

        internal readonly List<TabItem> TabItems = new List<TabItem>();

        /// <summary>
        /// Holds the index of the current focused TabItem in this TabControl
        /// </summary>
        internal int tabIndex = -1;

        #region Controls
        internal DockPanel TabStrip;
        internal Image DocMenu;
        internal StackPanel HeaderPanel;
        internal Border SeparatorBorder;
        internal Grid ContentPanel;
        #endregion Controls
        public TabControl()
        {
            InitializeComponent();
            SeparatorBorder.BorderBrush = SeparatorBorderColor ??= DefaultColors.SeparatorBorder;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            TabStrip = this.FindControl<DockPanel>(nameof(TabStrip));
            DocMenu = this.FindControl<Image>(nameof(DocMenu));
            HeaderPanel = this.FindControl<StackPanel>(nameof(HeaderPanel));
            SeparatorBorder = this.FindControl<Border>(nameof(SeparatorBorder));
            ContentPanel = this.FindControl<Grid>(nameof(ContentPanel));
        }


        private void TabControl_Initialized(object sender, EventArgs e)
        {
            switch (TabStripPlacement)
            {
                case TabStripPlacement.Top:
                    TabItemRotationAngle ??= 0;
                    DockPanel.SetDock(TabStrip, Dock.Top);
                    DockPanel.SetDock(SeparatorBorder, Dock.Top);
                    HeaderPanel.Orientation = Orientation.Horizontal;
                    DocMenu.RenderTransform = new RotateTransform(0);
                    break;
                case TabStripPlacement.Left:
                    TabItemRotationAngle ??= -45;
                    DockPanel.SetDock(TabStrip, Dock.Left);
                    DockPanel.SetDock(SeparatorBorder, Dock.Left);
                    HeaderPanel.Orientation = Orientation.Vertical;
                    DocMenu.RenderTransform = new RotateTransform(-45);
                    break;
                case TabStripPlacement.Bottom:
                    TabItemRotationAngle ??= 0;
                    DockPanel.SetDock(TabStrip, Dock.Bottom);
                    DockPanel.SetDock(SeparatorBorder, Dock.Bottom);
                    HeaderPanel.Orientation = Orientation.Horizontal;
                    DocMenu.RenderTransform = new RotateTransform(0);
                    break;
                case TabStripPlacement.Right:
                    TabItemRotationAngle ??= 45;
                    DockPanel.SetDock(TabStrip, Dock.Right);
                    DockPanel.SetDock(SeparatorBorder, Dock.Right);
                    HeaderPanel.Orientation = Orientation.Vertical;
                    DocMenu.RenderTransform = new RotateTransform(45);
                    break;
            }

            if (TabItemRotationAngle == null) return;
            HeaderPanel.RenderTransform = new RotateTransform((double)TabItemRotationAngle);
        }

        /// <summary>
        /// Adds a new TabItem to the target InpossibleTabControl
        /// </summary>
        /// <param name="tabItem"></param>
        public void Add(TabItem tabItem)
        {
            ++tabIndex;
            switch (TabMode)
            {
                case TabMode.Document:
                    HeaderPanel.Children.Insert(tabIndex, tabItem.TabItemHeader());
                    ContentPanel.Children.Insert(tabIndex, tabItem.Content);
                    tabItem.TabItemHeader().HideButton.IsVisible = false;
                    tabItem.TabItemHeader().MenuButton.IsVisible = false;
                    break;
                case TabMode.Window:
                    tabItem.TabItemHeader().CloseButton.IsVisible = false;
                    tabItem.TabItemHeader().HideButton.IsVisible = false;
                    tabItem.TabItemHeader().MenuButton.IsVisible = false;
                    HeaderPanel.Children.Insert(tabIndex, tabItem.TabItemHeader());
                    ContentPanel.Children.Insert(tabIndex, tabItem.TabItemBody());

                    break;
            }

            void FocusThisTabItem()
            {
                foreach (var child in HeaderPanel.Children)
                {
                    ((TabItemHeader)child).Background = tabItem.BackgroundWhenUnFocused;
                    ((TabItemHeader)child).Foreground = tabItem.ForegroundWhenUnFocused;
                }

                foreach (var child in ContentPanel.Children)
                {
                    child.IsVisible = false;
                }

                tabIndex = HeaderPanel.Children.IndexOf(tabItem.TabItemHeader());

                var element = (TabItemHeader)HeaderPanel.Children[tabIndex];
                element.Background = tabItem.BackgroundWhenFocused;
                element.Foreground = tabItem.ForegroundWhenFocused;
                ContentPanel.Children[tabIndex].IsVisible = true;
                ContentPanel.Children[tabIndex].Focus();
                element.BringIntoView(new Rect(new Size(element.Width, element.Height)));

            }

            void Click()
            {
                FocusThisTabItem();
                // Set this as the first TabItem in the OpenTabItems list
                TabItems.Remove(tabItem);
                TabItems.Insert(0, tabItem);
            }

            tabItem.Content ??= ErrorMessage.EmptyContent;
            tabItem.Content.GotFocus += (sender, e) => { FocusThisTabItem(); };
            tabItem.TabItemHeader().PointerReleased += (sender, e) => { Click(); };

            FocusThisTabItem();

            TabItems.Insert(0, tabItem);
            ContentPanel.Children[tabIndex].Focus();
        }

        private void OpenTabs_PointerReleased(object sender, PointerReleasedEventArgs e)
        {
            var menuItems = new List<MenuItem>();
            foreach (var tabItem in TabItems)
            {
                var menuItem = new MenuItem
                {
                    Header = tabItem.Header,
                    Icon = tabItem.ContentIcon
                };
                menuItem.Click += (sender1, e1) =>
                {
                    foreach (var child in HeaderPanel.Children)
                    {
                        ((TabItemHeader)child).Background = tabItem.BackgroundWhenUnFocused;
                        ((TabItemHeader)child).Foreground = tabItem.ForegroundWhenUnFocused;
                    }

                    foreach (var child in ContentPanel.Children)
                    {
                        child.IsVisible = false;
                    }

                    var tabIndex = HeaderPanel.Children.IndexOf(tabItem.TabItemHeader());
                    ((TabItemHeader)HeaderPanel.Children[tabIndex]).Background = tabItem.BackgroundWhenFocused;
                    ((TabItemHeader)HeaderPanel.Children[tabIndex]).Foreground = tabItem.ForegroundWhenFocused;
                    var element = (TabItemHeader)HeaderPanel.Children[tabIndex];

                    element.BringIntoView(new Rect(new Size(element.Width, element.Height)));

                    ContentPanel.Children[tabIndex].IsVisible = true;
                    ContentPanel.Children[tabIndex].Focus();
                    // Set this as the first TabItem in the OpenTabItems list
                    TabItems.Remove(tabItem);
                    TabItems.Insert(0, tabItem);
                };
                menuItems.Add(menuItem);
            }

            var contextMenu = new ContextMenu { Items = menuItems };
            ((Image)sender).ContextMenu = contextMenu;

            if (e.InitialPressMouseButton == MouseButton.Left)
            {
                contextMenu.Open();
                e.Handled = true;
            }
        }

        private void ContentPanel_LayoutUpdated(object sender, EventArgs e)
        {
            switch (ContentPanel.Children.Count)
            {
                case 0:
                    IsVisible = false;
                    break;
                case 1:
                    IsVisible = true;
                    if (TabMode == TabMode.Window)
                    {
                        SeparatorBorder.IsVisible = false;
                        TabStrip.IsVisible = false;
                    }

                    break;
                default:
                    if (ContentPanel.Children.Count > 1)
                    {
                        IsVisible = true;
                        SeparatorBorder.IsVisible = true;
                        TabStrip.IsVisible = true;
                    }

                    break;
            }
        }
    }
}
