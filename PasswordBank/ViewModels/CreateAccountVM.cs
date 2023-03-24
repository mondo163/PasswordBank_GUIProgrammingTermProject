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
    public class CreateAccountVM : BaseVM,ICloseWindow
    {
        #region Create account proporties
        private string enteredName;

        public string EnteredName
        {
            get { return enteredName; }
            set { enteredName = value; }
        }

        private string enteredEmail;

        public string EnteredEmail
        {
            get { return enteredEmail; }
            set { enteredEmail = value; }
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
            set { firstQuestion = value; }
        }
        private string firstAnswer;

        public string FirstAnswer
        {
            get { return firstAnswer; }
            set { firstAnswer = value; }
        }
        private string secondQuestion;

        public string SecondQuestion
        {
            get { return secondQuestion; }
            set { secondQuestion = value; }
        }
        private string secondAnswer;

        public string SecondAnswer
        {
            get { return secondAnswer; }
            set { secondAnswer = value; }
        }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand GenerateCommand { get; set; }
        public Action Close { get; set; }

        private bool submitDefaultButton;
        public bool SubmitDefaultButton
        {
            get { return submitDefaultButton; }
            set { submitDefaultButton = value; NotifyProperty(); }
        }

        private bool generateDefaultButton;

        public bool GenerateDefaultButton
        {
            get { return generateDefaultButton; }
            set { generateDefaultButton = value; NotifyProperty(); }
        }

        private bool generateEnabled;
        public bool GenerateEnabled
        {
            get { return generateEnabled; }
            set { generateEnabled = value; NotifyProperty(); }
        }

        #endregion

        private readonly UsersDatabase database;
        public CreateAccountVM(UsersDatabase database)
        {
            this.database = database;
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
            GenerateCommand = new RelayCommand(Generate);
            GenerateDefaultButton = true;
            SubmitDefaultButton = false;
            GenerateEnabled = true;
        }
        //generates a random passkey for the output from the database generate function
        private void Generate()
        {
            string pk = database.GeneratePasskey();
            OutputPasskey = pk;
            //sets new default and disables the generate button
            GenerateDefaultButton = false;
            SubmitDefaultButton = true;
            GenerateEnabled = false;
        }
        private void Save()
        {
            //confirms that user wants to commit to these changes.
            if (MessageBox.Show("Are your sure all information\n" +
                "entered is correct? Yes to continue.",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            //Checks all entered info to make sure it fits the criteria for each entry
            string message = "";
            if (GenerateDefaultButton) // makes sure you generated your passkey before submitting.
            {
                MessageBox.Show("You have not generated your passkey.\nGenerate your passkey then hit save", "Passkey Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
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
            
            //creates a new user and adds them to the database of users using the database add function
            RegUser newUser = new RegUser
            {
                Name = EnteredName,
                Email = EnteredEmail.ToLower(),
                UserName = EnteredUsername.ToLower(),
                Password = EnteredPassword,
                Passkey = OutputPasskey,
                Answers = new string[2],
                Questions = new string[2]
            };

            newUser.Answers[0] = FirstAnswer.ToLower();
            newUser.Answers[1] = SecondAnswer.ToLower();

            newUser.Questions[0] = FirstQuestion;
            newUser.Questions[1] = SecondQuestion;
            //if user exists, they will not be added
            bool result = database.AddUser(newUser);
            if (!result)
            {
                MessageBox.Show("User Could Not Be Added", "Save User Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Close?.Invoke();
        }
        //closes window without making changes. 
        private void Cancel()
        {
            Close?.Invoke();
        }


    }
}
   

