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

namespace ComTerm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewmodel mvm = new MainViewmodel();
        private DeviceManager dev = new DeviceManager();

        public MainWindow()
        {
            InitializeComponent();
            mvm.Initialize(dev);

            dev.OpenDevice();
            DataContext = mvm;
            mvm.RefreshDeviceList();
        }
    }
}
