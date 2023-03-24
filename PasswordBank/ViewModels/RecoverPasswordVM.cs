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
    public class RecoverPasswordVM : BaseVM, ICloseWindow
    {
        #region recover username properties
        private string enteredUsername;

        public string EnteredUsername
        {
            get { return enteredUsername; }
            set { enteredUsername = value; }
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
        public Action Close { get ; set ; }
        #endregion

        private readonly UsersDatabase database;

        public RecoverPasswordVM(UsersDatabase database)
        {
            this.database = database;
            EnterCom = new RelayCommand(Enter);
            SubmitCom = new RelayCommand(Submit);
            CancelCom = new RelayCommand(Cancel);
            HelpCom = new RelayCommand(Help);
            EnterButtonEnabled = true;
            EnterButtonDefault = true;
            SubmitButtonDefault = false;
        }
        //verifys the user and outputs the question for the user
        private void Enter()
        {
            //checks to see if the user exist by his username
            if (string.IsNullOrEmpty(EnteredUsername) || !database.UserExistsUsername(EnteredUsername.ToLower()))
            {
                MessageBox.Show("User does not exist.\nPlease Re-enter correct username.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //sets the questions and disables enter
            string[] questions = database.recoveryUser.Questions;
            FirstQuestion = questions[0];
            SecondQuestion = questions[1];
            EnterButtonEnabled = false;
            EnterButtonDefault = false;
            SubmitButtonDefault = true;
        }
        //will check security question answers and output password
        private void Submit()
        {
            //checks if user was verifed
            if (EnterButtonDefault)
            {
                MessageBox.Show("You have not entered your credentials.\n" +
                                "Please enter your username,\n" +
                                "So we can verify your identity.\n", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //checks answers to data answers
            string[] answerEntrys = new string[2] { FirstAnswer, SecondAnswer };
            string message = null;

            if (!ApplicationUtlities.CheckSecurityAnswers(ref message, answerEntrys, database.recoveryUser.Answers))
            {
                MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //retrieves the users password and outputs it to the screen. 
            string password = database.recoveryUser.Password;

            MessageBox.Show("Your new password is:\n\n" +
                             password, database.recoveryUser.Name + " Password", MessageBoxButton.OK, MessageBoxImage.Information);
            //updates the database
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
        //instructions. 
        private void Help()
        {
            MessageBox.Show("Recover your password\n" +
                           " - Enter your username\n" +
                           " - If you do not remember this, click Cancel\n" +
                           "   and click on the Username button.\n" +
                           " - Otherwise, hit enter after your credentials\n" +
                           "   have been inputted.\n" +
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
