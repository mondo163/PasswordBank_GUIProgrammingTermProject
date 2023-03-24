using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using EncryptDecryptLib;

namespace UserPasswordDatabaseLib
{
    public class UsersDatabase 
    {

        private const string KEY = "A3f5!234"; //for encrypting file
        private const string FILE_NAME = "DatabaseInfo.txt"; //file name
        private readonly int maxUsers; 
        private int registeredUserID; //used to register an id number to every added user.

        //sets temporary user variables for user needing to recover information
        //or for logged in users
        public IUserType recoveryUser;
        public IUserType currentActiveUser;
        //set the method for generating passkeys
        private HillCipherMethod HillMethod { get; set; }
        //keeps track of log in attempts
        public int LogInTriesRemaining { get; set; }  
        public List<string> RegisteredUsernames { get; set; } //contains the usernames that are unavailable
        public List<IUserType> Users { get; set; } //keeps track of all the users
        public UsersDatabase()
        {
            HillMethod = new HillCipherMethod();
            recoveryUser = null;
            currentActiveUser = null;
            maxUsers = 5;
            registeredUserID = 1;
            LogInTriesRemaining = 3;
            Users = new List<IUserType>();
            RegisteredUsernames = new List<string>();
        }
        //Generates passkeys
        public string GeneratePasskey()
        { 
            HillMethod.Encrypt(RandomPassphrase());
            return HillMethod.PassKey;
        }
        //helper method that creates a random passphrase for the generator
        private string RandomPassphrase()
        {
            Random random = new Random();
            string passphrase = "";
            for (int i = 0; i < 10; i++)
            {
                passphrase += (char)((random.Next() % 26) + 97);
            }

            return passphrase;
        }
        //creates a new user if said user does not exist. If they do exist, false is returned
        public bool AddUser(IUserType newUser)
        {
            if (Users.Count == maxUsers)
                return false;
         
            foreach (IUserType user in Users)
            {
                if (user.Email == newUser.Email)
                    return false;
            }

            newUser.ID = registeredUserID++;
            LogInTriesRemaining = 3;
            Users.Add(newUser);
            RegisteredUsernames.Add(newUser.UserName);
            return true;
        }
        //returns true or false if the database if empty
        public bool IsEmpty()
        {
            return (Users.Count == 0);
        }
        //checks if username is available
        public bool UsernameAvailable(string username)
        {
            return RegisteredUsernames.Contains(username);
        }
        //returns true if the user is found and sets currentUser, otherwise false
        public bool LogInUser(ref string message, string username, string password, string passkey)
        {
            if (LogInTriesRemaining <= 0)
            {
                message = "Out of Log-In tries.\nPlease reset your passkey and try again.";
                return false;
            }


            bool wrongUsername = true;
            bool wrongPassword = true;
            bool wrongPasskey = true;

            foreach (IUserType user in Users)
            {
                wrongUsername = !(user.UserName == username || user.Email == username);
                wrongPassword = !(user.Password == password);
                wrongPasskey = !(user.Passkey == passkey);

                if (!wrongUsername && !wrongPassword && !wrongPasskey)
                {
                    message = "Login Succesfull!";
                    currentActiveUser = user;
                    LogInTriesRemaining = 3;
                    return true;
                }
            }

            LogInTriesRemaining--;
            if (wrongUsername)
            {
                message = "User Does Not Exist\nLog-in tries remaining: " + LogInTriesRemaining; 
                return false;
            }
            else if (wrongPassword && wrongPasskey)
            {
                message = "Wrong Passkey and Password\nLog-in tries remaining: " + LogInTriesRemaining; 
                return false;
            }
            else if (wrongPassword)
            {
                message = "Wrong Password\nLog-in tries remaining: " + LogInTriesRemaining;
                return false;
            }
            else
            {
                message = "Wrong Passkey\nLog-in tries remaining: " + LogInTriesRemaining;
                return false;
            }
        }
        //Checks to see if user exists for passkey recovery
        public bool UserExistsUsernamePassword(string username, string password)
        {
            foreach (IUserType user in Users)
            {
                if (user.UserName == username && user.Password == password)
                {
                    recoveryUser = user;
                    return true;
                }
            }
            return false;
        }
        //checks to see if user exists from their username
        public bool UserExistsUsername(string username)
        {
            foreach (IUserType user in Users)
            {
                if (user.UserName == username)
                {
                    recoveryUser = user;
                    return true;
                }
            }
            return false;
        }
        //checks to see if user exists from their email
        public bool UserExistsEmail(string email)
        {
            foreach (IUserType user in Users)
            {
                if (user.Email == email)
                {
                    recoveryUser = user;
                    return true;
                }
            }
            return false;
        }
        //updates the users list depending on if the user recovery user and 
        //logged in user
        public bool UpdateDatabase()
        {
            bool activeUser = currentActiveUser == null;
            bool recoverUser = recoveryUser == null;
            if (activeUser && recoverUser)
                return false;

            if (!activeUser)
            {
                for (int i = 0; i < Users.Count; i++)
                {
                    if (Users[i].ID == currentActiveUser.ID)
                    {
                        Users[i] = currentActiveUser;
                        currentActiveUser = null;
                        break;
                    }
                }
            }
            if (!recoverUser)
            {
                for (int i = 0; i < Users.Count; i++)
                {
                    if (Users[i].ID == recoveryUser.ID)
                    {
                        Users[i] = recoveryUser;
                        recoveryUser = null;
                        break;
                    }
                }
            }

            return true;
        }
        //saves application database information
        public void SaveDatabase()
        {
            //cretes the file where the Users contents will be 
            //json serialized
            using (StreamWriter sw = new StreamWriter(FILE_NAME,false))
            {
                var json = JsonConvert.SerializeObject(Users, Formatting.Indented);
                sw.Write(json);
            }
            
            //Encrypts that same file so it cant be read. 
            FileInfo fi = new FileInfo(FILE_NAME);
           EncryptFile(fi.FullName, KEY);
        }
        //Help from https://www.youtube.com/watch?v=Xna-5XBXck8 on how to encrypt files. switched it to AES
        private void EncryptFile(string path, string key)
        {
            byte[] plaintext = File.ReadAllBytes(path);
            using (var DES = new DESCryptoServiceProvider())
            {
                DES.IV = Encoding.UTF8.GetBytes(key);
                DES.Key = Encoding.UTF8.GetBytes(key);
                DES.Mode = CipherMode.CBC;
                DES.Padding = PaddingMode.PKCS7;

                using (var memStream = new MemoryStream())
                {
                    CryptoStream cryptoStream = new CryptoStream(memStream, DES.CreateEncryptor(), CryptoStreamMode.Write);

                    cryptoStream.Write(plaintext, 0, plaintext.Length);
                    cryptoStream.FlushFinalBlock();
                    File.WriteAllBytes(path, memStream.ToArray());
                }
            }
        }
        //loads application database information
        public void LoadDatabase()
        {
            if (!File.Exists(FILE_NAME))
                return;

            //finds file and decrypts
            FileInfo fi = new FileInfo(FILE_NAME);
            DecryptFile(fi.FullName, KEY);

            //creates a json converter for the abstract user class
            JsonConverter[] converters = { new Converters.JsonRegUserConverter() };

            //deserializes the decrypted file
            var test = JsonConvert.DeserializeObject<List<IUserType>>(File.ReadAllText(fi.FullName),
                new JsonSerializerSettings() { Converters = converters });

            //assigns the contents to the Users database
            Users = test;

            //adds usernames from list to the registered username list
            foreach (IUserType user in Users)
            {
                RegisteredUsernames.Add(user.UserName);
            }
            //deletes the files, so it cannot be accessed
            File.Delete(FILE_NAME);
            
        }
        //Decrypts File using AES. help from: https://www.youtube.com/watch?v=Xna-5XBXck8
        private void DecryptFile(string path, string key)
        {
            byte[] encrypted = File.ReadAllBytes(path);
            using (var DES = new DESCryptoServiceProvider())
            {
                DES.IV = Encoding.UTF8.GetBytes(key);
                DES.Key = Encoding.UTF8.GetBytes(key);
                DES.Mode = CipherMode.CBC;
                DES.Padding = PaddingMode.PKCS7;


                using (var memStream = new MemoryStream())
                {
                    CryptoStream cryptoStream = new CryptoStream(memStream, DES.CreateDecryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(encrypted, 0,encrypted.Length);
                    cryptoStream.FlushFinalBlock();
                    File.WriteAllBytes(path, memStream.ToArray());
                }
            }
        }
        

    }
}
