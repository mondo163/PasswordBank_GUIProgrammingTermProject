using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserPasswordDatabaseLib
{
    public class AccountEntryInformation
    {
        private string url;

        public string URL
        {
            get { return url; }
            set { url = value; }
        }

        private string nickName;

        public string NickName
        {
            get { return nickName; }
            set { nickName = value; }
        }
        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        private string dateAdded;

        public string DateAdded
        {
            get { return dateAdded; }
            set { dateAdded = value; }
        }

        public AccountEntryInformation()
        {
            URL = "";
            NickName = "";
            Username = "";
            Password = "";
            DateAdded = "";
        }

        
    }
}
