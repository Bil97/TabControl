using System.Windows;
using System.Windows.Controls;
using ThingLing.WPF.Controls;
using TabControl = ThingLing.WPF.Controls.TabControl;
using TabItem = ThingLing.WPF.Controls.TabItem;

namespace WPFTestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private readonly TabControl _tabControl = new TabControl();

        public MainWindow()
        {
            InitializeComponent();

            //_tabControl.TabMode = TabMode.Document;
            ////_tabControl.TabMode = TabMode.Window;
            //_tabControl.TabStripPlacement = TabStripPlacement.Top;
            //_tabControl.TabStripPlacement = TabStripPlacement.Bottom;
            //MainPanel.Children.Add(_tabControl);
        }

        private int i = 0;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var tabItem = new TabItem
            {
                ContentTitle = $"Hello RichTextBox {++i}",
                Content = new TextBox { Text = $"Helloo {i}", TextWrapping = TextWrapping.Wrap },
                ToolTip = $"RichTextBox {i}"
            };
            _tabControl.Add(tabItem);
        }
    }
}
