using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UserPasswordDatabaseLib;

namespace PasswordBank.ViewModels
{
    public class PasswordBankMainPageVM : BaseVM
    {
        #region Main window properties
        private string enteredUsername;

        public string EnteredUsername
        {
            get { return enteredUsername; }
            set { enteredUsername = value; NotifyProperty(); }
        }
        private string enteredPassword;

        public string EnteredPassword
        {
            get { return enteredPassword; }
            set { enteredPassword = value; NotifyProperty(); }
        }
        private string enteredPasskey;

        public string EnteredPasskey
        {
            get { return enteredPasskey; }
            set { enteredPasskey = value; NotifyProperty(); }
        }
        public ICommand ExitCom { get; set; }
        public ICommand HelpCom { get; set; }
        public ICommand SubmitCom { get; set; }
        public ICommand CreateAccount { get; set; }
        public ICommand RecoverUsername { get; set; }
        public ICommand RecoverPassword { get; set; }
        public ICommand RecoverPasskey { get; set; }
        public Action RedXClosing { get; set; }
        public Action ExitComClosing { get; set; }
        public Action ClearForm { get; set; }
        #endregion
        private readonly UsersDatabase database;
        public PasswordBankMainPageVM(UsersDatabase database)
        {
            //gets database from ninject and loads it up
            this.database = database;
            database.LoadDatabase();

            //sets commands for buttons
            ExitCom = new RelayCommand(Exit);
            HelpCom = new RelayCommand(Help);
            SubmitCom = new RelayCommand(Submit);
            CreateAccount = new RelayCommand(AccountCreation);
            RecoverUsername = new RelayCommand(UsernameRecovery);
            RecoverPassword = new RelayCommand(PasswordRecovery);
            RecoverPasskey = new RelayCommand(PasskeyRecovery);
        }
        //exit menu command
        private void Exit()
        {
            ExitComClosing?.Invoke();
        }
        //directions on how to use app
        private void Help()
        {
            MessageBox.Show("           ---------------- Password Bank -------------------\n" +
                "First Time Use:\n" +
                " - Click on the Create Account Button.\n" +
                " - Enter the required information that fits the criteria given.\n" +
                " - Once Complete, you will be given a passkey need for you to log in\n" +
                " - Use your account information to log in.\n" +
                " - If login is succesful, you will be navigated to your database. \n" +
                "   Otherwise, you will need receive an error and have to log in\n" +
                "   again.\n\n" +
                "Information Recovery:\n" +
                " - If you have forgotten one of your login credentials, you can reset\n" +
                "   your credentials from the corresponding buttons at the bottom of\n" +
                "   the page\n" +
                " - For Username recovery: You will need your email and the answers\n" +
                "   to your security questions.\n" +
                " - For Password recovery: You will need your username and the\n" +
                "   answers to your security questions\n" +
                " - For Passkey recovery: You will need your user name, password, and \n" +
                "   answers to your security questions\n" +
                " - Depending on what you have forgotten, you will be navigated to\n" +
                "   the correct corresponding recovery page.", 
                "Help", 
                MessageBoxButton.OK, 
                MessageBoxImage.Information);
            return;
        }
        private void Submit()
        {
            //checks if strings are left empty with error message
            if (string.IsNullOrEmpty(EnteredUsername) ||
                string.IsNullOrEmpty(EnteredPasskey) ||
                string.IsNullOrEmpty(EnteredPassword))
            {
                MessageBox.Show("Entries cannot be blank.\n" +
                    "Enter all information before\n" +
                    "hitting submit.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            //checks login credentials 
            string message = null;
            if (!database.LogInUser(ref message, EnteredUsername.ToLower(), EnteredPassword, EnteredPasskey))
            {
                MessageBox.Show(message, "Log-In Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Empties out the form once log in is succesful
            RefreshForm();

            //redirects to the users password database
            Pages.PasswordDatabasePage passwordDatabasePage = new Pages.PasswordDatabasePage();
            passwordDatabasePage.ShowDialog();

        }
        //opens up the account creation form
        private void AccountCreation()
        {
            //empties form
            RefreshForm();

            CreateAccountPage createAccountPage = new CreateAccountPage();
            createAccountPage.ShowDialog();
        }
        //opens up the username recovery form
        private void UsernameRecovery()
        {
            RefreshForm();

            RecoverUsernamePage recoverUsernamePage = new RecoverUsernamePage();
            recoverUsernamePage.ShowDialog();
        }
        //opens up teh password recovery form
        private void PasswordRecovery()
        {
            RefreshForm();

            RecoverPasswordPage recoverPasswordPage = new RecoverPasswordPage();
            recoverPasswordPage.ShowDialog();
        }
        //opens up the passkey recovery form 
        private void PasskeyRecovery()
        {
            RefreshForm();

            RecoverPasskeyPage recoverPasskeyPage = new RecoverPasskeyPage();
            recoverPasskeyPage.Show();
        }

        //used to update the database and then save it before closing in the
        //event handler functions in the xaml.cs file. 
        public void UpdateDatabase()
        {
            //updates the database first and then saves the contents of the database to file
            database.UpdateDatabase();
            database.SaveDatabase();
        }
        //empties form contents. 
        private void RefreshForm()
        {
            EnteredUsername = "";
            EnteredPassword = "";
            EnteredPasskey = "";
            ClearForm?.Invoke();
        }
    }
}
