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
    public class RecoverUsernameVM : BaseVM, ICloseWindow
    {
        #region username recovery Properties
        private string enteredEmail;

        public string EnteredEmail
        {
            get { return enteredEmail; }
            set { enteredEmail = value; }
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
            set { firstAnswer = value; }
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
            set { secondAnswer = value; }
        }
        public ICommand EnterCom { get; set; }
        public ICommand SubmitCom { get; set; }
        public ICommand CancelCom { get; set; }
        public ICommand HelpCom { get; set; }
        private bool enterButtonEnabled;

        public bool EnterButtonEnabled
        {
            get { return enterButtonEnabled; }
            set { enterButtonEnabled = value; NotifyProperty(); }
        }

        private bool enterButtonDefault;

        public bool EnterButtonDefault
        {
            get { return enterButtonDefault; }
            set { enterButtonDefault = value; NotifyProperty(); }
        }
        private bool submitButtonDefault;

        public bool SubmitButtonDefault
        {
            get { return submitButtonDefault; }
            set { submitButtonDefault = value; NotifyProperty(); }
        }

 
        public Action Close { get; set; }
        #endregion
        private readonly UsersDatabase database;
        public RecoverUsernameVM(UsersDatabase database)
        {
            this.database = database;
            EnterCom = new RelayCommand(Enter);
            SubmitCom = new RelayCommand(Submit);
            CancelCom = new RelayCommand(Cancel);
            HelpCom = new RelayCommand(Help);
            SubmitButtonDefault = false;
            EnterButtonDefault = true;
            EnterButtonEnabled = true;
        }
        private void Enter()
        {
            //checks if the user exists
            if (string.IsNullOrEmpty(EnteredEmail) || !database.UserExistsEmail(EnteredEmail.ToLower()))
            {
                MessageBox.Show("User does not exist.\nPlease Re-enter credentials.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //if they exist question are displayed on screen. disables buttons as needed
            string[] questions = database.recoveryUser.Questions;
            FirstQuestion = questions[0];
            SecondQuestion = questions[1];
            EnterButtonEnabled = false;
            EnterButtonDefault = false;
            SubmitButtonDefault = false;
        }
        //checks security answers and outputs username
        private void Submit()
        {
            //checks if the user was verified
            if (EnterButtonDefault)
            {
                MessageBox.Show("You have not entered your credentials.\n" +
                                "Please enter your email,\n" +
                                "So we can verify your identity.\n", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //checks the security answers to the actual user answers
            string[] answerEntrys = new string[2] { FirstAnswer, SecondAnswer };
            string message = null;

            if (!ApplicationUtlities.CheckSecurityAnswers(ref message, answerEntrys, database.recoveryUser.Answers))
            {
                MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //outputs the username to the screen and updates the database
            string username = database.recoveryUser.UserName;

            MessageBox.Show("Your username is:\n\n" +
                             username, "New Passkey", MessageBoxButton.OK, MessageBoxImage.Information);

            if (!database.UpdateDatabase())
            {
                MessageBox.Show("Could not save. Sorry!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            Close?.Invoke();
        }
        //closes form
        private void Cancel()
        {
            Close?.Invoke();
        }
        //instructions
        private void Help()
        {
            MessageBox.Show("Recover your username\n" +
                           " - Enter your email\n" +
                           " - Otherwise, hit enter after your credentials\n" +
                           " - Once you have been found, your security questions\n" +
                           "   will appear and responses will need to be entered\n" +
                           "   before hitting Submit.\n" +
                           " - When you have responded to your security questions,\n" +
                           "   click on submit and your password will appear.\n" +
                           " - You will then be redirected back to the home page.\n", 
                           "Help", 
                           MessageBoxButton.OK, 
                           MessageBoxImage.Information);
        }
    }
}
