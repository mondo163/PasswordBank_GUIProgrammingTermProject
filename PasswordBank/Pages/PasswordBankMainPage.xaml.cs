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
using UserPasswordDatabaseLib;

namespace PasswordBank
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class PasswordBankMainPage : Window
    {
        private bool closinginitiated = false;
        private bool passwdHidden = true;
        private bool passkyHidden = true;
        public PasswordBankMainPage()
        {
            InitializeComponent();
            Loaded += PasswordBankMainPage_Loaded;
            Closing += PasswordBankMainPage_Closing;
        }
        //updates the Actions in the Main window depending on where the app is closing on when the page is loaded
        private void PasswordBankMainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.PasswordBankMainPageVM vm)
            {
                //action for the  exit command in the file menu
                vm.ExitComClosing += () =>
                {
                    closinginitiated = true;
                    vm.UpdateDatabase();
                    this.Close();
                };
                //action command for hitting the Red x. Only needs to update since window is closing
                vm.RedXClosing += () =>
                {
                    vm.UpdateDatabase();
                };
                //action used to clear the password box and passkey box
                vm.ClearForm += () =>
                {
                    pwdBox.Clear();
                    pkyBox.Clear();
                };
            }
        }
        private void PasswordBankMainPage_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is ViewModels.PasswordBankMainPageVM vm && !closinginitiated)
            {
                //invokes the red x action to update the database on leaving.
                vm.RedXClosing?.Invoke();
             
            }
        }
        //as the password is changing, it is changing the property in the view model with the
        //update password. Found out the password can't be directly binded to a property, so this is
        //the solution I found
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).EnteredPassword = ((PasswordBox)sender).Password;
            }
        }
        //Changes the password box for the text box that the user can write in and allows for either
        //hidden password or a visible one. 
        private void PasswordShowButton_Click(object sender, RoutedEventArgs e)
        {
            if (passwdHidden)
            {
                pwdTxtBox.Text = pwdBox.Password;
                pwdBox.Visibility = Visibility.Collapsed;
                pwdTxtBox.Visibility = Visibility.Visible;
                passwdHidden = false;
            }
            else
            {
                pwdBox.Password = pwdTxtBox.Text;
                pwdTxtBox.Visibility = Visibility.Collapsed;
                pwdBox.Visibility = Visibility.Visible;
                passwdHidden = true;
            }
        }
        //assings the entered passkey variable in the VM to the password entry
        private void PasswordBox_PasskeyChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).EnteredPasskey = ((PasswordBox)sender).Password;
            }
        }
        //changes the passkey visibility from invisible to visible and back
        private void PasskeyShowButton_Click(object sender, RoutedEventArgs e)
        {
            if (passkyHidden)
            {
                pkyTxtBox.Text = pkyBox.Password;
                pkyBox.Visibility = Visibility.Collapsed;
                pkyTxtBox.Visibility = Visibility.Visible;
                passkyHidden = false;
            }
            else
            {
                pkyBox.Password = pkyTxtBox.Text;  
                pkyTxtBox.Visibility = Visibility.Collapsed;
                pkyBox.Visibility = Visibility.Visible;
                passkyHidden = true;
            }
        }
    }
}
