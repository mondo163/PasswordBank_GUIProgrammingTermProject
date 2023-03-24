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

namespace PasswordBank.Pages
{
    /// <summary>
    /// Interaction logic for PasswordDatabasePage.xaml
    /// </summary>
    public partial class PasswordDatabasePage : Window
    {
        //used to not double close since there is an event handler for when the window is closing
        private bool closingInitiated = false;
        public PasswordDatabasePage()
        {
            InitializeComponent();
            //two event handlers for when the page loads and closes
            Loaded += PasswordDatabasePage_Loaded;
            Closing += PasswordDatabasePage_Closing;
          
        }
        //updates the two actions in the form depending on how the window is closing
        private void PasswordDatabasePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.PasswordDatabaseVM vm)
            {
                //updates the action for logging out when closed
                vm.LogOutClose += () =>
                {
                    closingInitiated = true; //sets it to true so the closing event handler doesnt invoke its method
                    vm.UpdateDatabase();
                    this.Close();
                };
                //updates the action for closing through the red X
                vm.RedXClose += () =>
                {
                    vm.UpdateDatabase();
                };
            }
        }
        //is only invoked when the closingInitiated is set to false
        private void PasswordDatabasePage_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is ViewModels.PasswordDatabaseVM vm && !closingInitiated)
            {
                vm.RedXClose?.Invoke();
            }
        }
    }
}
