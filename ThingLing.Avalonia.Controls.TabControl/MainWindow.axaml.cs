using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace ThingLing.Avalonia.Controls
{
    public class MainWindow : Window
    {
        private TabControl tab;
        public static MainWindow Window { get; set; }
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            Window = this;

            tab = this.FindControl<TabControl>("tab");
            tab.TabMode = TabMode.Window;
            tab.TabStripPlacement = TabStripPlacement.Left;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private int i = 0;
       async private void Button_Click(object sender, RoutedEventArgs e)
        {
            var tabItem = new TabItem
            {
                Header = $"Hello TextBox {++i}",
                Content = new TextBox { Text = $"Helloo {i}", TextWrapping = TextWrapping.Wrap },
                ToolTip = $"RichTextBox {i}"
            };
            tab.Add(tabItem);

        }

    }
}
