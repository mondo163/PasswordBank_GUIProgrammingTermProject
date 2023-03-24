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
    public class EditUserAccountVM : BaseVM,ICloseWindow
    {
        #region edit user account proporties
        private string enteredName;

        public string EnteredName
        {
            get { return enteredName; }
            set { enteredName = value; NotifyProperty(); }
        }

        private string enteredEmail;

        public string EnteredEmail
        {
            get { return enteredEmail; }
            set { enteredEmail = value; NotifyProperty(); }
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
        private string outputPasskey;

        public string OutputPasskey
        {
            get { return outputPasskey; }
            set { outputPasskey = value; NotifyProperty(); }
        }
        private string firstQuestion;

        public string FirstQuestion
        {
            get { return firstQuestion; }
            set { firstQuestion = value; NotifyProperty(); }
        }
        private string firstAnswer;

        public string FirstAnswer
        {
            get { return firstAnswer; }
            set { firstAnswer = value; NotifyProperty(); }
        }
        private string secondQuestion;

        public string SecondQuestion
        {
            get { return secondQuestion; }
            set { secondQuestion = value; NotifyProperty(); }
        }
        private string secondAnswer;

        public string SecondAnswer
        {
            get { return secondAnswer; }
            set { secondAnswer = value; NotifyProperty(); }
        }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand GenerateCommand { get; set; }
        public Action Close { get; set; }
        private bool generateEnabled;
        public bool GenerateEnabled
        {
            get { return generateEnabled; }
            set { generateEnabled = value; NotifyProperty(); }
        }
        private IUserType CurrentUser { get; set; }
        #endregion

        private readonly UsersDatabase database;
        public EditUserAccountVM(UsersDatabase database)
        {
            this.database = database;
            CurrentUser = database.currentActiveUser;

            //Sets all User info so user can see what information he has set
            EnteredName = CurrentUser.Name;
            EnteredEmail = CurrentUser.Email;
            EnteredUsername = CurrentUser.UserName;
            EnteredPassword = CurrentUser.Password;
            OutputPasskey = CurrentUser.Passkey;
            FirstQuestion = CurrentUser.Questions[0];
            FirstAnswer = CurrentUser.Answers[0];
            SecondQuestion = CurrentUser.Questions[1];
            SecondAnswer = CurrentUser.Answers[1];

            //generates commands for the buttons and their functions
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
            GenerateCommand = new RelayCommand(Generate);

            //sets default button for program. 
            GenerateEnabled = true;
        }
        private void Generate()
        {
            //generates the passkey and disables the button
            string pk = database.GeneratePasskey();
            OutputPasskey = pk;
            GenerateEnabled = false;
        }
        private void Save()
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

            //checks all the entered infromation criteria
            string message = "";
            if (string.IsNullOrEmpty(EnteredName))
            {
                MessageBox.Show("Don't forget your name.\nRe-enter and Try Again.", "Name Error",
                   MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!ApplicationUtlities.CheckEmail(EnteredEmail))
            {
                MessageBox.Show("Email is Incorrect.\nRe-enter and Try Again.", "Email Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!ApplicationUtlities.CheckUsername(ref message, EnteredUsername.ToLower(), database))
            {
                MessageBox.Show(message + "\nRe-enter and Try Again.", "Username Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!ApplicationUtlities.CheckPasswordCriteria(ref message, EnteredPassword))
            {
                MessageBox.Show(message, "Password Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //changes the current users information with the new info
            CurrentUser.Name = EnteredName;
            CurrentUser.Email = EnteredEmail;
            CurrentUser.UserName = EnteredUsername;
            CurrentUser.Password = EnteredPassword;
            CurrentUser.Passkey = OutputPasskey;
            CurrentUser.Questions[0] = FirstQuestion;
            CurrentUser.Answers[0] = FirstAnswer;
            CurrentUser.Questions[1] = SecondQuestion;
            CurrentUser.Answers[1] = SecondAnswer;
            //sets the new current active user to the new edited one
            database.currentActiveUser = CurrentUser;

            Close?.Invoke();
        }
        //closes window
        private void Cancel()
        {
            Close?.Invoke();
        }


    }
}
   

