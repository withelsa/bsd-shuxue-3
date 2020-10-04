using bsd_shuxue_3.Domain.Impl;
using System.Windows;

namespace bsd_shuxue_3
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private JiaJianHunHeQuestionFactory questionFactory = new JiaJianHunHeQuestionFactory();

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            questionFactory.DoConfig(this);
        }
    }
}