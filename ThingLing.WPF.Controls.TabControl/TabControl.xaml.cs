using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ThingLing.WPF.Controls.Methods;
using ThingLing.WPF.Controls.Props;

namespace ThingLing.WPF.Controls
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
        public TabStripPlacement? TabStripPlacementSide { get; set; }

        /// <summary>
        /// Determines the Angle of rotation of a TabIte
        /// </summary>
        public double? TabItemRotationAngle { get; set; }
        //public double TabItemRotationAngle { get; set; } = 0D;

        /// <summary>
        /// Determines the Angle of rotation of a TabItem
        /// </summary>
        public int TabItemsCount { get; set; }

        /// <summary>
        /// The currently selected TabItem 
        /// </summary>
        public TabItem SelectedTabItem { get; set; }

        public static ContextMenu DockingContextMenu { get; set; }

        internal readonly List<TabItem> TabItems = new List<TabItem>();

        /// <summary>
        /// Holds the index of the current focused TabItem in this TabControl
        /// </summary>
        internal int tabIndex = -1;
        public TabControl()
        {
            InitializeComponent();
            _ = new LoadTheme();
        }

        public TabControl(TabControlTheme theme)
        {
            InitializeComponent();
            _ = new LoadTheme(theme);
        }

        private void TabControl_Loaded(object sender, RoutedEventArgs e)
        {
            SeparatorBorder.BorderBrush = CurrentTheme.SeparatorBorder;

            /* var parent = $"{((this.Parent as Panel)?.Parent as Panel)?.Parent.GetType()}";
             var floatingWindow = "ThingLing.WPF.Controls.InternalControls.FloatingWindow";
             if (parent != floatingWindow)*/
            this.ContentPanel.LayoutUpdated += ContentPanel_LayoutUpdated;
            //TabStrip_Placement();
        }

        private void TabStrip_Placement()
        {
            switch (TabStripPlacementSide)
            {
                case TabStripPlacement.Top:
                    TabItemRotationAngle ??= 0;
                    DockPanel.SetDock(TabStrip, Dock.Top);
                    DockPanel.SetDock(SeparatorBorder, Dock.Top);
                    HeaderPanel.Orientation = Orientation.Horizontal;
                    DockPanel.SetDock(DocMenu, Dock.Right);
                    break;
                case TabStripPlacement.Left:
                    TabItemRotationAngle ??= -90;
                    DockPanel.SetDock(TabStrip, Dock.Left);
                    DockPanel.SetDock(SeparatorBorder, Dock.Left);
                    HeaderPanel.Orientation = Orientation.Vertical;
                    DockPanel.SetDock(DocMenu, Dock.Bottom);
                    break;
                case TabStripPlacement.Bottom:
                    TabItemRotationAngle ??= 0;
                    DockPanel.SetDock(TabStrip, Dock.Bottom);
                    DockPanel.SetDock(SeparatorBorder, Dock.Bottom);
                    HeaderPanel.Orientation = Orientation.Horizontal;
                    DockPanel.SetDock(DocMenu, Dock.Right);
                    break;
                case TabStripPlacement.Right:
                    TabItemRotationAngle ??= 90;
                    DockPanel.SetDock(TabStrip, Dock.Right);
                    DockPanel.SetDock(SeparatorBorder, Dock.Right);
                    HeaderPanel.Orientation = Orientation.Vertical;
                    DockPanel.SetDock(DocMenu, Dock.Bottom);
                    break;
            }
        }

        /// <summary>
        /// Adds a new TabItem to this TabControl
        /// </summary>
        /// <param name="tabItem"></param>
        public void Add(TabItem tabItem)
        {
            // Select a TabItem
            this.SelectedTabItem = tabItem;
            tabItem.TabItemHeader().MouseDown += TabItem_MouseDown;
            tabItem.TabItemBody().MouseDown += TabItem_MouseDown;
            void TabItem_MouseDown(object sender, MouseButtonEventArgs e)
            {
                this.SelectedTabItem = tabItem;
            }

            ++tabIndex;
            switch (TabMode)
            {
                default:
                case TabMode.Document:
                    // Do not mess this order
                    TabStripPlacementSide ??= TabStripPlacement.Top;
                    TabStrip_Placement();
                    tabItem.TabItemHeader().LayoutTransform = new RotateTransform((double)TabItemRotationAngle);
                    HeaderPanel.Children.Insert(tabIndex, tabItem.TabItemHeader());
                    ContentPanel.Children.Insert(tabIndex, tabItem.Content);
                    break;
                case TabMode.Window:
                    // Do not mess this order
                    TabStripPlacementSide ??= TabStripPlacement.Bottom;
                    TabStrip_Placement();
                    tabItem.TabItemHeader().LayoutTransform = new RotateTransform((double)TabItemRotationAngle);
                    tabItem.TabItemHeader().CloseButton.Visibility = Visibility.Collapsed;
                    HeaderPanel.Children.Insert(tabIndex, tabItem.TabItemHeader());
                    tabItem.TabItemBody().ContentPanel.Children.Add(tabItem.Content);
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
                    Header = tabItem.ContentTitle,
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
            TabItemsCount = ContentPanel.Children.Count;
            switch (TabItemsCount)
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
