using bsd_shuxue_3.Utils;
using System;
using System.Collections.Generic;
using System.Windows;

namespace bsd_shuxue_3.Domain.Impl
{
    /// <summary>
    /// PropertyGridWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PropertyGridWindow : Window
    {
        public object SelectedObject { get; set; }

        public PropertyGridWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.propertyGrid.SelectedObject = this.SelectedObject;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            var valid = true;
            var validatable = this.SelectedObject as IValidatable;
            if (validatable != null)
            {
                List<String> messages = new List<String>();
                valid = validatable.Validate(messages);
                if (!valid)
                {
                    MsgBox.Error(messages[0]);
                }
            }
            if (valid)
            {
                this.DialogResult = true;
            }
        }
    }
}