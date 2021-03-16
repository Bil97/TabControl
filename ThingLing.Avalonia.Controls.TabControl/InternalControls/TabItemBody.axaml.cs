using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ThingLing.Avalonia.Controls.InternalControls
{
    public class TabItemBody : UserControl
    {
        public TabItemBody()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
