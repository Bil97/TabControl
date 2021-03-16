using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ThingLing.Avalonia.Controls.InternalControls
{
    public class TabItemHeader : UserControl
    {
        public TabItemHeader()
        {
            InitializeComponent();
            Initialized += TabItemHeader_Initialized;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void TabItemHeader_Initialized(object? sender, EventArgs e)
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
            TabControl? parent;
            int tabIndex;
            TabItem tabItem;
            Grid parentContentPanel = new Grid();
            Grid panelParentContentPanel = new Grid();
            StackPanel parentHeaderPanel = new StackPanel();

            if ((Parent as Panel)?.Parent.GetType() == typeof(TabItemBody))
            {
                var panelParent = ((Panel)Parent).Parent as TabItemBody;
                parent = (TabControl)((Panel)((Panel)((TabItemBody)((Panel)Parent).Parent).Parent).Parent).Parent;

                var headerPanelParent = (panelParent.Parent as Panel).Parent as Panel;
                parentHeaderPanel = headerPanelParent.FindControl<StackPanel>("HeaderPanel");

                parentContentPanel = parent.FindControl<Grid>("ContentPanel");
                tabIndex = parentContentPanel.Children.IndexOf(panelParent);

                panelParentContentPanel = parent.FindControl<Grid>("ContentPanel");
                tabItem = parent.TabItems!.FirstOrDefault(i => i.Content == panelParentContentPanel.Children[0]);
            }
            else
            {
                parent = (TabControl)((Panel)((Panel)((ScrollViewer)((Panel)Parent).Parent).Parent).Parent).Parent;

                tabIndex = parentHeaderPanel.Children.IndexOf(this);
                tabItem = parent.TabItems!.FirstOrDefault(i => i.TabItemHeader() == this);
            }

            parentContentPanel.Children.RemoveAt(tabIndex);
            parentHeaderPanel.Children.RemoveAt(tabIndex);

            parent.TabItems.Remove(tabItem);

            if (parentContentPanel.Children.Count < 1)
            {
                parent.tabIndex = -1;
                return;
            }
            //var nextTabIndex = parentHeaderPanel.Children.IndexOf(parent.TabItems[0].TabItemHeader());
            var task = new Task(async () =>
            {
                await MessageBox.ShowAsync(MainWindow.Window, $"{parent.TabItems.Count}");
            });
            task.RunSynchronously();

            //((TabItemHeader)parentHeaderPanel.Children[nextTabIndex]).Background = tabItem?.BackgroundWhenFocused;
            //((TabItemHeader)parentHeaderPanel.Children[nextTabIndex]).Foreground = tabItem?.ForegroundWhenFocused;

            //var element = (TabItemHeader)parentHeaderPanel.Children[nextTabIndex];

            //element.BringIntoView(new Rect(new Size(element.Width, element.Height)));

            //parentContentPanel.Children[nextTabIndex].IsVisible = false;
            //parentContentPanel.Children[nextTabIndex].Focus();

            //parent.tabIndex = nextTabIndex;
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
