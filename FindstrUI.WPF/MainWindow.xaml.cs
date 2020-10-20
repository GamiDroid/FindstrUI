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
using FindstrUI.WPF.ViewModels;

namespace FindstrUI.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel; 
        }

        private readonly MainWindowViewModel _viewModel;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ExecuteCommand();
        }

        private void BrowseBtn_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Browse();
        }

        private void OpenMsDocsFindstrMnu_Click(object sender, RoutedEventArgs e)
        {
            var url = @"https://docs.microsoft.com/en-us/windows-server/administration/windows-commands/findstr";
            try
            {
                System.Diagnostics.Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
