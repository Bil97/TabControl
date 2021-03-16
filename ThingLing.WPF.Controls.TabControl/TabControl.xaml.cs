using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ThingLing.WPF.Controls.InternalControls;
using ThingLing.WPF.Controls.Properties;
using ThingLing.WPF.ControlsProperties;

namespace ThingLing.WPF.Controls
{
    /// <summary>
    /// Interaction logic for TabControl.xaml
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

        internal readonly List<TabItem> TabItems = new List<TabItem>();

        /// <summary>
        /// Holds the index of the current focused TabItem in this TabControl
        /// </summary>
        internal int tabIndex = -1;
        public TabControl()
        {
            InitializeComponent();
            SeparatorBorder.BorderBrush = SeparatorBorderColor ??= DefaultColors.SeparatorBorder;
        }

        private void TabControl_Loaded(object sender, RoutedEventArgs e)
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
                    tabItem.TabItemHeader().HideButton.Visibility = Visibility.Collapsed;
                    tabItem.TabItemHeader().MenuButton.Visibility = Visibility.Collapsed;
                    break;
                case TabMode.Window:
                    tabItem.TabItemHeader().CloseButton.Visibility = Visibility.Collapsed;
                    tabItem.TabItemHeader().HideButton.Visibility = Visibility.Collapsed;
                    tabItem.TabItemHeader().MenuButton.Visibility = Visibility.Collapsed;
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

                foreach (UIElement child in ContentPanel.Children)
                {
                    child.Visibility = Visibility.Collapsed;
                }

                tabIndex = HeaderPanel.Children.IndexOf(tabItem.TabItemHeader());

                var element = (TabItemHeader)HeaderPanel.Children[tabIndex];
                element.Background = tabItem.BackgroundWhenFocused;
                element.Foreground = tabItem.ForegroundWhenFocused;
                ContentPanel.Children[tabIndex].Visibility = Visibility.Visible;
                ContentPanel.Children[tabIndex].Focus();
                element.BringIntoView(new Rect(new Size(element.ActualWidth, element.ActualHeight)));

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
            tabItem.TabItemHeader().MouseLeftButtonUp += (sender, e) => { Click(); };

            FocusThisTabItem();

            TabItems.Insert(0, tabItem);
            ContentPanel.Children[tabIndex].Focus();
        }

        private void OpenTabs_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var contextMenu = new ContextMenu();
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

                    foreach (UIElement child in ContentPanel.Children)
                    {
                        child.Visibility = Visibility.Collapsed;
                    }

                    var tabIndex = HeaderPanel.Children.IndexOf(tabItem.TabItemHeader());
                    ((TabItemHeader)HeaderPanel.Children[tabIndex]).Background = tabItem.BackgroundWhenFocused;
                    ((TabItemHeader)HeaderPanel.Children[tabIndex]).Foreground = tabItem.ForegroundWhenFocused;
                    var element = (TabItemHeader)HeaderPanel.Children[tabIndex];

                    element.BringIntoView(new Rect(new Size(element.ActualWidth, element.ActualHeight)));

                    ContentPanel.Children[tabIndex].Visibility = Visibility.Visible;
                    ContentPanel.Children[tabIndex].Focus();
                    // Set this as the first TabItem in the OpenTabItems list
                    TabItems.Remove(tabItem);
                    TabItems.Insert(0, tabItem);
                };
                contextMenu.Items.Add(menuItem);
            }

            ((Image)sender).ContextMenu = contextMenu;
            contextMenu.IsOpen = true;
        }

        private void ContentPanel_LayoutUpdated(object sender, EventArgs e)
        {
            switch (ContentPanel.Children.Count)
            {
                case 0:
                    Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    Visibility = Visibility.Visible;
                    if (TabMode == TabMode.Window)
                    {
                        SeparatorBorder.Visibility = Visibility.Collapsed;
                        TabStrip.Visibility = Visibility.Collapsed;
                    }

                    break;
                default:
                    if (ContentPanel.Children.Count > 1)
                    {
                        Visibility = Visibility.Visible;
                        SeparatorBorder.Visibility = Visibility.Visible;
                        TabStrip.Visibility = Visibility.Visible;
                    }

                    break;
            }
        }
    }
}
