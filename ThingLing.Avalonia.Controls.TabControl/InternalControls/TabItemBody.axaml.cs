using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ThingLing.Avalonia.Controls.InternalControls
{
    public class TabItemBody : UserControl
    {
        internal TabItemHeader TabItemHeader;
        internal Grid ContentPanel;

        public TabItemBody()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            TabItemHeader = this.FindControl<TabItemHeader>(nameof(TabItemHeader));
            ContentPanel = this.FindControl<Grid>(nameof(ContentPanel));
        }
    }
}
