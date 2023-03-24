using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using UserPasswordDatabaseLib;

namespace UserDatabaseUT
{
    [TestClass]
    public class UsersDatabaseUT
    {
        [TestMethod]
        public void AddUser_NewMemberCreatedAndAdded_ReturnsTrue()
        {
            //Arrange
            IUserType user = new RegUser
            {
                Name = "Stanley",
                Email = "asdfa@gmail.com",
                UserName = "hello",
                Password = "help",
                Passkey = "!!!!",
                Questions = new string[2],
                Answers = new string[2],
            };
            UsersDatabase data = new UsersDatabase();
            bool result;

            //Act
            result = data.AddUser(user);

            //Assert
            Assert.AreEqual(result, true);
        }
        [TestMethod]
        public void AddUser_MemberAlreadyExists_ReturnsFalse()
        {
            //Arrange
            IUserType user = new RegUser
            {
                Name = "Stanley",
                Email = "asdfa@gmail.com",
                UserName = "hello",
                Password = "help",
                Passkey = "!!!!",
                Questions = new string[2],
                Answers = new string[2],
            };
            IUserType user2 = new RegUser
            {
                Name = "Stanley",
                Email = "asdfa@gmail.com",
                UserName = "hello",
                Password = "help",
                Passkey = "!!!!",
                Questions = new string[2],
                Answers = new string[2],
            };
            UsersDatabase data = new UsersDatabase();
            bool result;

            //Act
            data.AddUser(user);
            result = data.AddUser(user2);

            //Assert
            Assert.AreEqual(result, false);
        }
        [TestMethod]
        public void AddUser_MaxMembersReached_ReturnsFalse()
        {
            //Arrange
            IUserType user = new RegUser
            {
                Name = "Stanley",
                Email = "asdfa@gmail.com",
                UserName = "hello",
                Password = "help",
                Passkey = "!!!!",
                Questions = new string[2],
                Answers = new string[2],
            };
            IUserType user2 = new RegUser
            {
                Name = "Stanley",
                Email = "asdf@gmail.com",
                UserName = "hello",
                Password = "help",
                Passkey = "!!!!",
                Questions = new string[2],
                Answers = new string[2],
            };
            IUserType user3 = new RegUser
            {
                Name = "Stanley",
                Email = "asd@gmail.com",
                UserName = "hello",
                Password = "help",
                Passkey = "!!!!",
                Questions = new string[2],
                Answers = new string[2],
            }; 
            IUserType user4 = new RegUser
            {
                Name = "Stanley",
                Email = "as@gmail.com",
                UserName = "hello",
                Password = "help",
                Passkey = "!!!!",
                Questions = new string[2],
                Answers = new string[2],
            }; 
            IUserType user5 = new RegUser
            {
                Name = "Stanley",
                Email = "a@gmail.com",
                UserName = "hello",
                Password = "help",
                Passkey = "!!!!",
                Questions = new string[2],
                Answers = new string[2],
            };
            IUserType user6 = new RegUser
            {
                Name = "Stanley",
                Email = "1234@gmail.com",
                UserName = "hello",
                Password = "help",
                Passkey = "!!!!",
                Questions = new string[2],
                Answers = new string[2],
            };
            UsersDatabase data = new UsersDatabase();
            bool result;

            //Act
            data.AddUser(user);
            data.AddUser(user2);
            data.AddUser(user3);
            data.AddUser(user4);
            data.AddUser(user5);
            result = data.AddUser(user6);

            //Assert
            Assert.AreEqual(result, false);
        }
        [TestMethod]
        public void LogInUser_MakeSureUserExistsWithUsername_ReturnsTrueMessage()
        {
            //Arrange
            IUserType user = new RegUser
            {
                Name = "Stanley",
                Email = "asdfa@gmail.com",
                UserName = "hello",
                Password = "help",
                Passkey = "!!!!",
                Questions = new string[2],
                Answers = new string[2],
            };
            UsersDatabase data = new UsersDatabase();
            bool result;
            string message = "";

            //Act
            data.AddUser(user);
            result = data.LogInUser(ref message, user.UserName, user.Password, user.Passkey);

            //Assert
            Assert.AreEqual(result, true);
            Assert.AreEqual(message, "Login Succesfull!");
        }
        [TestMethod]
        public void LogInUser_MakeSureUserExistsWithEmail_ReturnsTrueWithMessage()
        {
            //Arrange
            IUserType user = new RegUser
            {
                Name = "Stanley",
                Email = "asdfa@gmail.com",
                UserName = "hello",
                Password = "help",
                Passkey = "!!!!",
                Questions = new string[2],
                Answers = new string[2],
            };
            UsersDatabase data = new UsersDatabase();
            bool result;
            string message = "";

            //Act
            data.AddUser(user);
            result = data.LogInUser(ref message, user.Email, user.Password, user.Passkey);

            //Assert
            Assert.AreEqual(result, true);
            Assert.AreEqual(message, "Login Succesfull!");
        }
        [TestMethod]
        public void LogInUser_UserDoesNotExist_ReturnsFalseWithMessage()
        {
            //Arrange
            IUserType user = new RegUser
            {
                Name = "Stanley",
                Email = "asdfa@gmail.com",
                UserName = "hello",
                Password = "help",
                Passkey = "!!!!",
                Questions = new string[2],
                Answers = new string[2],
            };
            UsersDatabase data = new UsersDatabase();
            bool result;
            string message = "";

            //Act
            data.AddUser(user);
            result = data.LogInUser(ref message, "aloz@gmail.com", user.Password, user.Passkey);
            

            //Assert
            Assert.AreEqual(result, false);
            Assert.AreEqual(message, "User Does Not Exist");
        }
        [TestMethod]
        public void LogInUser_PasswordIncorrect_ReturnsFalseWithMessage()
        {
            //Arrange
            IUserType user = new RegUser
            {
                Name = "Stanley",
                Email = "asdfa@gmail.com",
                UserName = "hello",
                Password = "help",
                Passkey = "!!!!",
                Questions = new string[2],
                Answers = new string[2],
            };
            UsersDatabase data = new UsersDatabase();
            bool result;
            string message = "";

            //Act
            data.AddUser(user);
            result = data.LogInUser(ref message, user.UserName, "asdgggf", user.Passkey);


            //Assert
            Assert.AreEqual(result, false);
            Assert.AreEqual(message, "Wrong Password");
        }
        [TestMethod]
        public void LogInUser_PasskeyIncorrect_ReturnsFalseWithMessage()
        {
            //Arrange
            IUserType user = new RegUser
            {
                Name = "Stanley",
                Email = "asdfa@gmail.com",
                UserName = "hello",
                Password = "help",
                Passkey = "!!!!",
                Questions = new string[2],
                Answers = new string[2],
            };
            UsersDatabase data = new UsersDatabase();
            bool result;
            string message = "";

            //Act
            data.AddUser(user);
            result = data.LogInUser(ref message, user.UserName, user.Password, "asdgggf");


            //Assert
            Assert.AreEqual(result, false);
            Assert.AreEqual(message, "Wrong Passkey");
        }
        [TestMethod]
        public void LogInUser_PasskeyAndPasswordIncorrect_ReturnsFalseWithMessage()
        {
            //Arrange
            IUserType user = new RegUser
            {
                Name = "Stanley",
                Email = "asdfa@gmail.com",
                UserName = "hello",
                Password = "help",
                Passkey = "!!!!",
                Questions = new string[2],
                Answers = new string[2],
            };
            UsersDatabase data = new UsersDatabase();
            bool result;
            string message = "";

            //Act
            data.AddUser(user);
            result = data.LogInUser(ref message, user.UserName, "asdgggf", "asdgggf");


            //Assert
            Assert.AreEqual(result, false);
            Assert.AreEqual(message, "Wrong Passkey and Password");
        }
        [TestMethod]
        public void SaveLoadDatabase_MakeSureDataBaseCanSave_FileExists()
        {
            //Arrange
            RegUser user = new RegUser
            {
                Name = "Stanley",
                Email = "asdfa@gmail.com",
                UserName = "hello",
                Password = "help",
                Passkey = "!!!!",
                Questions = new string[2],
                Answers = new string[2],
            };
            RegUser user2 = new RegUser
            {
                Name = "Stanley",
                Email = "asdf@gmail.com",
                UserName = "hello",
                Password = "help",
                Passkey = "!!!!",
                Questions = new string[2],
                Answers = new string[2],
            };
            RegUser user3 = new RegUser
            {
                Name = "Stanley",
                Email = "asd@gmail.com",
                UserName = "hello",
                Password = "help",
                Passkey = "!!!!",
                Questions = new string[2],
                Answers = new string[2],
            };
            RegUser user4 = new RegUser
            {
                Name = "Stanley",
                Email = "as@gmail.com",
                UserName = "hello",
                Password = "help",
                Passkey = "!!!!",
                Questions = new string[2],
                Answers = new string[2],
            };
            RegUser user5 = new RegUser
            {
                Name = "Stanley",
                Email = "a@gmail.com",
                UserName = "hello",
                Password = "help",
                Passkey = "!!!!",
                Questions = new string[2],
                Answers = new string[2],
            };
            
            UsersDatabase data = new UsersDatabase();

            //Act
            data.AddUser(user);
            data.AddUser(user2);
            data.AddUser(user3);
            data.AddUser(user4);
            data.AddUser(user5);
            data.SaveDatabase();

            data.LoadDatabase();
        }
    }
}
