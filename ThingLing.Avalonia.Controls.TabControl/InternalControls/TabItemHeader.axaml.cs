using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;
using System.Linq;

namespace ThingLing.Avalonia.Controls.InternalControls
{
    public class TabItemHeader : UserControl
    {
        internal Image ContentIcon;
        internal TextBlock ContentChanged;
        internal Border CloseButton;
        internal Border HideButton;
        internal Border MenuButton;
        internal TextBlock ContentTitle;
        public TabItemHeader()
        {
            InitializeComponent();
            Initialized += TabItemHeader_Initialized;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            ContentIcon = this.FindControl<Image>(nameof(ContentIcon));
            ContentChanged = this.FindControl<TextBlock>(nameof(ContentChanged));
            CloseButton = this.FindControl<Border>(nameof(CloseButton));
            HideButton = this.FindControl<Border>(nameof(HideButton));
            MenuButton = this.FindControl<Border>(nameof(MenuButton));
            ContentTitle = this.FindControl<TextBlock>(nameof(ContentTitle));
        }

        private void TabItemHeader_Initialized(object sender, EventArgs e)
        {
            this.BringIntoView(new Rect(new Size(this.Width, this.Height)));
        }

        private void Border_PointerEnter(object sender, PointerEventArgs e)
        {
            ((Border)sender).BorderBrush = Brushes.White;
        }

        private void Border_PointerLeave(object sender, PointerEventArgs e)
        {
            ((Border)sender).BorderBrush = Brushes.Transparent;
        }

        private void Close()
        {
            TabControl parent;
            int tabIndex;
            TabItem tabItem;

            if ((Parent as Panel)?.Parent.GetType() == typeof(TabItemBody))
            {
                var panelParent = ((Panel)Parent).Parent as TabItemBody;
                parent = (TabControl)((Panel)((Panel)((TabItemBody)((Panel)Parent).Parent).Parent).Parent).Parent;
                tabIndex = parent.ContentPanel.Children.IndexOf(panelParent);
                tabItem = parent.TabItems!.FirstOrDefault(i => i.Content == panelParent?.ContentPanel.Children[0]);
            }
            else
            {
                parent = (TabControl)((Panel)((Panel)((ScrollViewer)((Panel)Parent).Parent).Parent).Parent).Parent;
                tabIndex = parent.HeaderPanel.Children.IndexOf(this);
                tabItem = parent.TabItems!.FirstOrDefault(i => i.TabItemHeader() == this);
            }

            parent.ContentPanel.Children.RemoveAt(tabIndex);
            parent.HeaderPanel.Children.RemoveAt(tabIndex);

            parent.TabItems.Remove(tabItem);

            if (parent.ContentPanel.Children.Count < 1)
            {
                parent.tabIndex = -1;
                return;
            }
            var nextTabIndex = parent.HeaderPanel.Children.IndexOf(parent.TabItems[0].TabItemHeader());

            ((TabItemHeader)parent.HeaderPanel.Children[nextTabIndex]).Background = tabItem?.BackgroundWhenFocused;
            ((TabItemHeader)parent.HeaderPanel.Children[nextTabIndex]).Foreground = tabItem?.ForegroundWhenFocused;

            var element = (TabItemHeader)parent.HeaderPanel.Children[nextTabIndex];

            element.BringIntoView(new Rect(new Size(element.Width, element.Height)));

            parent.ContentPanel.Children[nextTabIndex].IsVisible = true;
            parent.ContentPanel.Children[nextTabIndex].Focus();

            parent.tabIndex = nextTabIndex;
        }

        private void CloseWindow_PointerReleased(object sender, PointerReleasedEventArgs e)
        {
            Close();
            e.Handled = true;
        }

        private void HideWindow_PointerReleased(object sender, PointerReleasedEventArgs e)
        {
            //Close();
        }

        private void WindowMenu_PointerReleased(object sender, PointerReleasedEventArgs e)
        {
            var contextMenu = ((Border)sender).ContextMenu;
            if (contextMenu != null) contextMenu.Open();
            // Prevent the event from bubbling up to the parent
            e.Handled = true;
        }

        private void DockTop_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void DockBottom_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void DockDocument_Click(object sender, RoutedEventArgs e)
        {
            //
        }
    }
}
