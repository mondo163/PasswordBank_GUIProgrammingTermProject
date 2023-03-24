using EncryptDecryptLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UserPasswordDatabaseLib
{
    
    public class RegUser : IUserType
    {
        public RegUser()
        {
            Name = "";
            Email = "";
            UserName = "";
            Password = "";
            Passkey = "";
            Questions = new string[2];
            Answers = new string[2];
            RegisteredAccounts = new ObservableCollection<AccountEntryInformation>();
        }
        public RegUser(string newName, string newEmail,string newUsername, string newPassword, 
            string passkey, string[] newQuestions, string[] newResponses)
        {
            Passkey = passkey;
            Name = newName;
            Email = newEmail;
            UserName = newUsername; 
            Password = newPassword;
            Questions = newQuestions;
            Answers = newResponses;
            RegisteredAccounts = new ObservableCollection<AccountEntryInformation>();
        }
        //adds entry to the list of accounts
        public override bool AddAccount(AccountEntryInformation entry)
        {
            if (RegisteredAccounts.Contains(entry))
            {
                return false;
            }

            RegisteredAccounts.Add(entry);
            return true;
        }
        //removes the entry if the entry is contained in the list
        public override bool RemoveAccount(AccountEntryInformation entry)
        {
            if (RegisteredAccounts.Contains(entry))
            {
                RegisteredAccounts.Remove(entry);
                return true;
            }

            return false;
        }
        //clears the list
        public override bool DeleteAllData()
        {
            if (RegisteredAccounts == null || RegisteredAccounts.Count == 0)
            {
                return false;
            }

            RegisteredAccounts.Clear();
            return true;
        }
        //edits original account with the new one. 
        public override bool EditAccount(AccountEntryInformation original, AccountEntryInformation edited)
        {
            if (!RegisteredAccounts.Contains(original))
            {
                return false;
            }

            int index = RegisteredAccounts.IndexOf(original);
            RegisteredAccounts[index] = edited;

            return true;
        }
    }
}
