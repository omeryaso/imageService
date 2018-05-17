using ImageServiceGUI.Model;
using ImageServiceGUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageServiceGUI
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        SettingsViewModel vm;
        public SettingsView()
        {
            InitializeComponent();
            vm = new SettingsViewModel(new SettingsModel());
            DataContext = vm;
        }

        private void removeBtn_Click(object sender, RoutedEventArgs e)
        {
            vm.VM_HandlerClose((string)lstBox.SelectedItem);
        }
    }
}
