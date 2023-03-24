using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UserPasswordDatabaseLib;

namespace PasswordBank.ViewModels
{
    public class PasswordDatabaseVM: BaseVM
    {
        #region password database properties
        public IUserType CurrentUser { get; set; }

        private ObservableCollection<AccountEntryInformation> usersAccounts;
        public ObservableCollection<AccountEntryInformation> UsersAccounts
        {
            get { return usersAccounts; }
            set { usersAccounts = value; NotifyProperty();}
        }
        private AccountEntryInformation selectedItem;
        public AccountEntryInformation SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; NotifyProperty(); }
        }
        private string welcomeString;

        public string WelcomeString
        {
            get { return welcomeString; }
            set { welcomeString = value; NotifyProperty(); }
        }

        private bool deleteEnabled;
        public bool DeleteEnabled
        {
            get { return deleteEnabled; }
            set { deleteEnabled = value; NotifyProperty(); }
        }
        private bool deleteAllEnabled;

        public bool DeleteAllEnabled
        {
            get { return deleteAllEnabled; }
            set { deleteAllEnabled = value; NotifyProperty(); }
        }

        public ICommand LogOutCommand { get; set; }
        public ICommand HelpCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand DeleteAllCommand { get; set; }
        public ICommand EditUserAccountCommand { get; set; }
        public Action LogOutClose { get ; set ; }
        public Action RedXClose { get; set; }
        #endregion
        private readonly UsersDatabase database;
        public PasswordDatabaseVM(UsersDatabase database)
        {
            this.database = database;
            //sets the current user and their accounts to the observable list
            //used with the listview
            CurrentUser = database.currentActiveUser;
            UsersAccounts = database.currentActiveUser.RegisteredAccounts;
            WelcomeString = "Welcome " + CurrentUser.Name;

            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit);
            DeleteCommand = new RelayCommand(Delete);
            DeleteAllCommand = new RelayCommand(DeleteAll);
            EditUserAccountCommand = new RelayCommand(EditUserAccount);
            LogOutCommand = new RelayCommand(LogOut);
            HelpCommand = new RelayCommand(Help);
            DeleteAllEnabled = !(UsersAccounts.Count == 0);
            DeleteEnabled = !(UsersAccounts.Count == 0);
        }
        //adds entries
        private void Add()
        {
            //opens the add page that will add the new account
            Pages.AddAccountPage addAccountPage = new Pages.AddAccountPage();
            addAccountPage.ShowDialog();
            //updates the observable list with the new one
            UsersAccounts = database.currentActiveUser.RegisteredAccounts;
            //enables delete buttons
            DeleteAllEnabled = !(UsersAccounts.Count == 0);
            DeleteEnabled = !(UsersAccounts.Count == 0);
        }
        //edits entries
        private void Edit()
        {
            //makes sure something is selected
            if (SelectedItem == null)
            {
                MessageBox.Show("No entry selected.\n" +
                    "Click on the entry and then\n" +
                    "on the button.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            //sets the database selected entry to the selected item so it can be edited in the other view model
            database.currentActiveUser.SelectedEntry = SelectedItem;
            //opens edit page
            Pages.EditAccountPage editAccountPage = new Pages.EditAccountPage();
            editAccountPage.ShowDialog();
            //updates the list with the new edited list
            UsersAccounts = database.currentActiveUser.RegisteredAccounts;
        }
        //deletes entries
        private void Delete()
        {
            //makes sure something is selected
            if (SelectedItem == null)
            {
                MessageBox.Show("No entry selected.\n" +
                    "Click on the entry and then\n" +
                    "on the button.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            //confirms that user wants to commit to these changes.
            if (MessageBox.Show("Are your sure you want to delete\n" +
                "this entry? Yes to continue.",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            //removes the entry from the observable list
            UsersAccounts.Remove(SelectedItem);
            //updates the actual database list
            database.currentActiveUser.RegisteredAccounts = UsersAccounts;
            //enables/disable buttons based on list count
            DeleteAllEnabled = !(UsersAccounts.Count == 0);
            DeleteEnabled = !(UsersAccounts.Count == 0);
        }
        private void DeleteAll()
        {
            //makes sure you cannot delete an empty list
            if (UsersAccounts.Count == 0)
            {
                MessageBox.Show("No entries to delete!\n", "Delete All Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //confirms that user wants to commit to these changes.
            if (MessageBox.Show("Are your sure you want to delete\n" +
                "all entries? Yes to continue.",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            //clears the list of entries
            UsersAccounts.Clear();
            //updates the database list as well
            database.currentActiveUser.RegisteredAccounts = UsersAccounts;
            //diables delete buttons
            DeleteAllEnabled = !(UsersAccounts.Count == 0);
            DeleteEnabled = !(UsersAccounts.Count == 0);
        }
        //edit the user info
        private void EditUserAccount()
        {
            //opens the edit user account page
            EditUserAccountPage editUserAccountPage = new EditUserAccountPage();
            editUserAccountPage.ShowDialog();
        }
        //logs out and save their information
        private void LogOut()
        {
            //logs out and activates the logoutclose action
            LogOutClose?.Invoke();
        }
        //instructions
        private void Help()
        {
            MessageBox.Show("Username Password Bank:\n" +
                " - Listed below on this screen is the contents\n" +
                "   of your database - It contains the information for the online\n" +
                "   accounts that you have created and added to your database.\n" +
                " - To add acounts, click the Add button and a new form will \n" +
                "   pop-up for you to fill out with the account information.\n" +
                " - To edit an entry, select an entry and then click on the Edit\n" +
                "   button. A new form will pop-up with the current information \n" +
                "   that can then be edited.\n" +
                " - To delete an entry, select and highlight an entry and then \n" +
                "   click on the Delete button. It will then remove that entry \n" +
                "   from your database.\n" +
                " - To delete all entries, click on the Delete All button to empty\n" +
                "   out your database.\n" +
                " - To edit your user info, click on the Edit Account button where\n" +
                "   you will be brought to a new form where you can edit your user\n" +
                "   info.\n" +
                " - Once you exit the screen, all information will be updated in \n" +
                "   your database and will be returned to the log-in screen.",
                "Help",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        //used by event handler to update the database and then returning to the home page.
        public void UpdateDatabase()
        {
            database.currentActiveUser.RegisteredAccounts = UsersAccounts;
            database.UpdateDatabase();
        }

    }
}
