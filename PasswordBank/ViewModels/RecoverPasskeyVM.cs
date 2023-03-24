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
    public class RecoverPasskeyVM : BaseVM, ICloseWindow
    {
        #region passkey recovery properties
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
        public RecoverPasskeyVM(UsersDatabase database)
        {
            this.database = database;
            EnterCom = new RelayCommand(Enter);
            SubmitCom = new RelayCommand(Submit);
            CancelCom = new RelayCommand(Cancel);
            HelpCom = new RelayCommand(Help);
            //sets default button
            EnterButtonDefault = true;
            SubmitButtonDefault = false;
            EnterButtonEnabled = true;
        }
        //checks to user credentials and makes sure they exist
        private void Enter()
        {
            if (string.IsNullOrEmpty(EnteredUsername) ||
                string.IsNullOrEmpty(EnteredPassword) ||
                !database.UserExistsUsernamePassword(EnteredUsername.ToLower(), EnteredPassword))
            {
                MessageBox.Show("User does not exist.\nPlease Re-enter credentials.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //gets questions and sets them on screen from the user recovery is happening on
            string[] questions = database.recoveryUser.Questions;
            FirstQuestion = questions[0];
            SecondQuestion = questions[1];
            //changes default button
            EnterButtonDefault = false;
            SubmitButtonDefault = true;
            EnterButtonEnabled = false;
        }
        //Once user is found, answers to security questions will be checked and a new passkey will show up
        private void Submit()
        {
            //checks if user has been found and verified
            if (EnterButtonDefault)
            {
                MessageBox.Show("You have not entered your credentials.\n" +
                                "Please enter your username and password,\n" +
                                "So we can verify your identity.\n", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //gets user entrys from form and checks if the match the users actual answers
            string[] answerEntrys = new string[2] { FirstAnswer, SecondAnswer };
            string message = null;

            if (!ApplicationUtlities.CheckSecurityAnswers(ref message,answerEntrys,database.recoveryUser.Answers))
            {
                MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //if passed, a new passkey will be generated and outputted to the screen. will then return to homepage. 
            string passkey = database.GeneratePasskey();
            database.recoveryUser.Passkey = passkey;

            MessageBox.Show("Your new passkey is:\n\n" +
                             passkey,"New Passkey", MessageBoxButton.OK, MessageBoxImage.Information);
            //updates the dabase with the updated user passkey
            if (!database.UpdateDatabase())
            {
                MessageBox.Show("Could not save. Sorry!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //resets login tries
            database.LogInTriesRemaining = 3;

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
            MessageBox.Show("Recover your passkey\n" +
                            " - Enter your username and password\n" +
                            " - If you do not remember these, click Cancel\n" +
                            "   and click on the appropriate button.\n" +
                            " - Otherwise, hit enter after your credentials\n" +
                            "   have been inputted.\n" +
                            " - Once you have been found, your security questions\n" +
                            "   will appear and responses will need to be entered\n" +
                            "   before hitting Submit.\n" +
                            " - When you have responded to your security questions,\n" +
                            "   click on submit and your new passkey will appear.\n" +
                            " - You will then be redirected back to the home page.\n", "Help", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
