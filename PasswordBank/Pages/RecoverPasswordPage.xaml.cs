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
using System.Windows.Shapes;

namespace PasswordBank
{
    /// <summary>
    /// Interaction logic for RecoverPasswordPage.xaml
    /// </summary>
    public partial class RecoverPasswordPage : Window
    {
        public RecoverPasswordPage()
        {
            InitializeComponent();
            Loaded += RecoverPasswordPage_Loaded;
        }

        //event handler that updates the action in the view model to close the window
        private void RecoverPasswordPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.ICloseWindow vm)
            {
                vm.Close += () =>
                {
                    this.Close();
                };
            }
        }
    }
}
