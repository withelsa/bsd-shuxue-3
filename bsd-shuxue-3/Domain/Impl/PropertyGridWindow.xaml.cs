using System.Windows;
using System.Windows.Forms;

namespace bsd_shuxue_3.Domain.Impl
{
    /// <summary>
    /// PropertyGridWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PropertyGridWindow : Window
    {
        public PropertyGridWindow()
        {
            InitializeComponent();
        }

        public PropertyGrid PropertyGrid
        {
            get
            {
                return this.propertyGrid;
            }
        }
    }
}