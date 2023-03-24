using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncryptDecryptLib;

namespace UserPasswordDatabaseLib
{
    
    public abstract class IUserType
    {
        //All information and functions every user should have
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Passkey { get; set; }
        public string[] Questions { get; set; }
        public string[] Answers { get; set; }
        public AccountEntryInformation SelectedEntry { get; set; }
        public ObservableCollection<AccountEntryInformation> RegisteredAccounts { get; set; }
        public abstract bool AddAccount(AccountEntryInformation entry);
        public abstract bool RemoveAccount(AccountEntryInformation entry);
        public abstract bool DeleteAllData();
        public abstract bool EditAccount(AccountEntryInformation original, AccountEntryInformation edited);
    }
}
