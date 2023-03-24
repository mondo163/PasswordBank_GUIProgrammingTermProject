﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UserPasswordDatabaseLib;

namespace PasswordBank.ViewModels
{
    public class EditAccountVM : BaseVM, ICloseWindow
    {
        #region edit account properties
        private string enteredURL;

        public string EnteredURL
        {
            get { return enteredURL; }
            set { enteredURL = value; NotifyProperty(); }
        }
        private string enteredNickname;

        public string EnteredNickname
        {
            get { return enteredNickname; }
            set { enteredNickname = value; NotifyProperty(); }
        }
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

        public ICommand HelpCommand { get; set; }
        public ICommand SubmitCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public Action Close { get ; set ; }
        #endregion
        private readonly AccountEntryInformation selectedEntry; //for the entry that user wants to edit
        private readonly UsersDatabase database;
        public EditAccountVM(UsersDatabase database)
        {
            this.database = database;
            //sets the entry and fills out the form with the contents.
            this.selectedEntry = database.currentActiveUser.SelectedEntry;
            EnteredURL = this.selectedEntry.URL;
            EnteredNickname = this.selectedEntry.NickName;
            EnteredUsername = this.selectedEntry.Username;
            EnteredPassword = this.selectedEntry.Password;

            HelpCommand = new RelayCommand(Help);
            SubmitCommand = new RelayCommand(Submit);
            CancelCommand = new RelayCommand(Cancel);
        }
        //instructions on how to use the app
        private void Help()
        {
            MessageBox.Show("Edit an account: \n" +
                " - Replace the shown info with the\n" +
                "   information you wish to change within\n" +
                "   your entry. Nickname is optional.",
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
            //checks to see if the required entries contain something
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
            if (string.IsNullOrEmpty(EnteredNickname)) //sets username to nothing if empty
                EnteredNickname = "";

            //creates the new edited entry
            AccountEntryInformation newEntry = new AccountEntryInformation
            {
                URL = EnteredURL,
                NickName = EnteredNickname,
                Username = EnteredUsername,
                Password = enteredPassword,
                DateAdded = DateTime.Now.ToString("MM-dd-yyyy HH:mm")
            };

            //checks the account database for the selected entry, and changes it out for the new edited one
            database.currentActiveUser.EditAccount(selectedEntry, newEntry);

            Close?.Invoke();

        }
        //closes window
        private void Cancel()
        {
            Close?.Invoke();
        }
    }
}
