using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using UserPasswordDatabaseLib;


namespace UserDatabaseUT
{
    [TestClass]
    public class RegUserTests
    {
        [TestMethod]
        public void RegularConstructor_NoArguments_CreatesUserObject()
        {
            //arrange
            IUserType user = new RegUser();

            //assert
            Assert.AreEqual(typeof(RegUser), user.GetType());
        }
        [TestMethod]
        public void RegularConstructor_ArgumentsForVariables_CreatesUserObject()
        {
            //arrange
            string[] questions = new string[2];
            string[] answers = new string[2];
            IUserType user = new RegUser("Stanley","asdfa@gmail.com" ,"farter01", "Herrrro","qwer", questions, answers);


            //assert
            Assert.AreEqual(typeof(RegUser), user.GetType());
            Assert.AreEqual(user.Name, "Stanley");
            Assert.AreEqual(user.Email, "asdfa@gmail.com");
            Assert.AreEqual(user.UserName, "farter01");
            Assert.AreEqual(user.Password, "Herrrro");
            Assert.AreEqual(user.Passkey, "qwer");
            Assert.AreEqual(user.Questions.Length, 2);
            Assert.AreEqual(user.Answers.Length, 2);
        }
        [TestMethod]
        public void AddAccount_AddingNewAccounteEntry_ResultIsTrueEntryIsAdded()
        {
            //arrange
            AccountEntryInformation entry = new AccountEntryInformation
            {
                URL = "www.google.com",
                NickName = "Barnie",
                Username = "owl",
                Password = "hello",
                DateAdded = DateTime.Now.ToString("h/m")
            };
            IUserType user = new RegUser();

            //act
            bool result = user.AddAccount(entry);

            //assert
            Assert.AreEqual(true, result);
            Assert.AreEqual(user.RegisteredAccounts.Count, 1);
        }
        [TestMethod]
        public void AddAccount_AddingCopy_ResultIsFalseEntryIsNotAdded()
        {
            //arrange
            AccountEntryInformation entry1 = new AccountEntryInformation
            {
                URL = "www.google.com",
                NickName = "Barnie",
                Username = "owl",
                Password = "hello",
                DateAdded = DateTime.Now.ToString("h/m")
            };
            
            IUserType user = new RegUser();

            //act
            bool result = user.AddAccount(entry1);
            result = user.AddAccount(entry1);


            //assert
            Assert.AreEqual(false, result);
            Assert.AreEqual(user.RegisteredAccounts.Count, 1);
        }
        [TestMethod]
        public void RemoveAccount_RemovingNonExistent_ResultOfRemoveIsFalse()
        {
            //arrange
            AccountEntryInformation entry1 = new AccountEntryInformation
            {
                URL = "www.google.com",
                NickName = "Barnie",
                Username = "owl",
                Password = "hello",
                DateAdded = DateTime.Now.ToString("h/m")
            };
            AccountEntryInformation entry2 = new AccountEntryInformation
            {
                URL = "www.google.com",
                NickName = "Barnie",
                Username = "owl",
                Password = "hello",
                DateAdded = DateTime.Now.ToString("h/m")
            };
            IUserType user = new RegUser();
            bool result;

            //act
            user.AddAccount(entry1);
            result = user.RemoveAccount(entry2);

            //Assert
            Assert.AreEqual(result, false);
        }
        [TestMethod]
        public void RemoveAccount_RemovingExistent_ResultOfRemoveIsTrue()
        {
            //arrange
            AccountEntryInformation entry1 = new AccountEntryInformation
            {
                URL = "www.google.com",
                NickName = "Barnie",
                Username = "owl",
                Password = "hello",
                DateAdded = DateTime.Now.ToString("h/m")
            };
            AccountEntryInformation entry2 = new AccountEntryInformation
            {
                URL = "www.google.com",
                NickName = "Barnie",
                Username = "owl",
                Password = "hello",
                DateAdded = DateTime.Now.ToString("h/m")
            };
            IUserType user = new RegUser();
            bool result;

            //act
            user.AddAccount(entry1);
            user.AddAccount(entry2);
            result = user.RemoveAccount(entry2);

            //Assert
            Assert.AreEqual(result, true);
        }
        [TestMethod]
        public void DeleteAllData_DeleteAllThatDontExist_ReturnsFalseForRemove()
        {
            //Arrange
            IUserType user = new RegUser();
            bool result;

            //Act
            result = user.DeleteAllData();

            //Arrange
            Assert.AreEqual(result, false);
            Assert.AreEqual(user.RegisteredAccounts.Count, 0);
        }
        [TestMethod]
        public void DeleteAllData_DeleteAllThatExists_ReturnsTrueForRemove()
        {
            //Arrange
            AccountEntryInformation entry1 = new AccountEntryInformation
            {
                URL = "www.google.com",
                NickName = "Barnie",
                Username = "owl",
                Password = "hello",
                DateAdded = DateTime.Now.ToString("h/m")
            };
            AccountEntryInformation entry2 = new AccountEntryInformation
            {
                URL = "www.google.com",
                NickName = "Barnie",
                Username = "owl",
                Password = "hello",
                DateAdded = DateTime.Now.ToString("h/m")
            };
            IUserType user = new RegUser();
            bool result;

            //Act
            user.AddAccount(entry1);
            user.AddAccount(entry2);
            result = user.DeleteAllData();

            //Assert
            Assert.AreEqual(result, true);
            Assert.AreEqual(user.RegisteredAccounts.Count, 0);
        }
        [TestMethod]
        public void EditAccount_SelectedEntryDoesNotExist_ReturnsFalseForEdit()
        {
            //Arrange
            AccountEntryInformation entry1 = new AccountEntryInformation
            {
                URL = "www.google.com",
                NickName = "Barnie",
                Username = "owl",
                Password = "hello",
                DateAdded = DateTime.Now.ToString("h/m")
            };
            AccountEntryInformation entry2 = new AccountEntryInformation
            {
                URL = "www.google.com",
                NickName = "Barnie",
                Username = "owl",
                Password = "hello",
                DateAdded = DateTime.Now.ToString("h/m")
            };
            IUserType user = new RegUser();
            bool result;

            //Act
            user.AddAccount(entry1);
            result = user.EditAccount(entry2, entry1);

            //Assert
            Assert.AreEqual(result, false);
        }
        [TestMethod]
        public void EditAccount_SelectedEntryDoesExist_ReturnsTrueForEdit()
        {
            //Arrange
            AccountEntryInformation entry1 = new AccountEntryInformation
            {
                URL = "www.google.com",
                NickName = "Barnie",
                Username = "owl",
                Password = "hello",
                DateAdded = DateTime.Now.ToString("h/m")
            };
            AccountEntryInformation entry2 = new AccountEntryInformation
            {
                URL = "www.google.com",
                NickName = "Barnie",
                Username = "owl",
                Password = "hello",
                DateAdded = DateTime.Now.ToString("h/m")
            };
            IUserType user = new RegUser();
            bool result;

            //Act
            user.AddAccount(entry1);
            result = user.EditAccount(entry1, entry2);

            //Assert
            Assert.AreEqual(result, true);
            Assert.AreEqual(user.RegisteredAccounts.Contains(entry2), true); //makes sure entry was actually edited
        }
    }
}
