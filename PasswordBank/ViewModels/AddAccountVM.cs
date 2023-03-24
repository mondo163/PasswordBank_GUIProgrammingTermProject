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
    public class AddAccountVM : BaseVM, ICloseWindow
    {
        #region Add account form properties
        private string enteredURL;

        public string EnteredURL
        {
            get { return enteredURL; }
            set { enteredURL = value; }
        }
        private string enteredNickname;

        public string EnteredNickname
        {
            get { return enteredNickname; }
            set { enteredNickname = value; }
        }
        private string enteredUsername;

        public string EnteredUsername
        {
            get { return enteredUsername; }
            set { enteredUsername = value; }
        }
        private string enteredPassword;

        public string EnteredPassword
        {
            get { return enteredPassword; }
            set { enteredPassword = value; }
        }

        public ICommand HelpCommand { get; set; }
        public ICommand SubmitCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public Action Close { get ; set ; }
        #endregion
        private readonly UsersDatabase database;
        public AddAccountVM(UsersDatabase database)
        {
            this.database = database;
            HelpCommand = new RelayCommand(Help);
            SubmitCommand = new RelayCommand(Submit);
            CancelCommand = new RelayCommand(Cancel);
        }
        //Directions for the app
        private void Help()
        {
            MessageBox.Show("Add an account: \n" +
                " - Enter the following fields with the\n" +
                "   information you wish to store within\n" +
                "   your database. Nickname is optional.",
                "Help",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        private void Submit()
        {
            //confirms that user wants to commit to these changes. 
            if (MessageBox.Show("Are your sure you want to complete\n" +
                "these changes? Yes to continue.",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            //checks if required strings are empty
            if (string.IsNullOrEmpty(EnteredURL))
            {
                MessageBox.Show("Please enter a URL.", "URL error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (string.IsNullOrEmpty(EnteredUsername))
            {
                MessageBox.Show("Please enter a Username.", "Username error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (string.IsNullOrEmpty(EnteredPassword))
            {
                MessageBox.Show("Please enter a Password.", "Password error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            //sets nickname to nothing if empty
            if (string.IsNullOrEmpty(EnteredNickname))
                EnteredNickname = "";

            //creates new database entry and adds it to the current user database
            AccountEntryInformation newEntry = new AccountEntryInformation
            {
                URL = EnteredURL,
                NickName = EnteredNickname,
                Username = EnteredUsername,
                Password = enteredPassword,
                DateAdded = DateTime.Now.ToString("MM-dd-yyyy HH:mm")
            };

            if (!database.currentActiveUser.AddAccount(newEntry))
            {
                MessageBox.Show("Entry could not be added.\nMake sure it is not a copy.",
                    "Adding Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //closes the window
            Close?.Invoke();

        }
        //closes window without saving changes
        private void Cancel()
        {
            Close?.Invoke();
        }
    }
}
