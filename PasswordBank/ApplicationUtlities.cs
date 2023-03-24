using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserPasswordDatabaseLib;
using Ninject;

namespace PasswordBank
{
 /// <summary>
 /// This class has utility functions that the entire app will need
 /// This class can not be instantiated, but the methods are static
 /// so they can be invoked through out the view models. 
 /// </summary>
    public abstract class ApplicationUtlities
    {
        //checks to make sure the entry is an email
        public static bool CheckEmail(string entry)
        {
            string[] segments = entry.ToLower().Split('@');
            if (!(segments.Length == 2))
            {
                return false;
            }

            foreach (char item in segments[0])
            {
                if (!char.IsLetterOrDigit(item) && !CheckValidEmailCharacters(item))
                {
                    return false;
                }
            }

            string[] secondSegments = segments[1].Split('.');
            if (secondSegments.Length < 2)
            {
                return false;
            }
            return true;
        }
        //Checks to make sure the entry is a username
        public static bool CheckUsername(ref string message, string entry, UsersDatabase database)
        {
            if (string.IsNullOrEmpty(entry))
                return false;

            if (database.RegisteredUsernames.Contains(entry))
            {
                message = "Username already exists!";
            }

            bool IsNumberOrLetter = true;
            foreach (char item in entry)
            {
                if (!char.IsLetterOrDigit(item))
                {
                    message = "Username contains invalid characters";
                    IsNumberOrLetter = false;
                    break;
                }
            }

            if (IsNumberOrLetter && (entry.Length >= 5 && entry.Length <= 20))
                return true;
            else
                return false;
            
        }
        //Checks password entry to make sure that it 
        public static bool CheckPasswordCriteria(ref string message, string entry)
        {
            int[] characterTypeCounts = new int[4];
            bool[] conditionChecks = new bool[4];            
            
            foreach (char item in entry)
            {
                if (char.IsLower(item))
                {
                    characterTypeCounts[0]++;
                    conditionChecks[0] = (characterTypeCounts[0] > 1);
                }
                else if (char.IsUpper(item))
                {
                    characterTypeCounts[1]++;
                    conditionChecks[1] = (characterTypeCounts[1] > 1);
                }
                else if (item == '!' || item == '#' || item == '@' || item == '$')
                {
                    characterTypeCounts[2]++;
                    conditionChecks[2] = (characterTypeCounts[2] > 1);
                }
                else if (char.IsDigit(item))
                {
                    characterTypeCounts[3]++;
                    conditionChecks[3] = (characterTypeCounts[3] > 1);
                }
                else
                {
                    message = "Invalid characters in your password.\n" +
                              "Make sure that the characters are within the specified\n" +
                              "criteria.";
                    return false;
                }
            }

            for (int i = 0; i < conditionChecks.Length; i++)
            {
                if (!conditionChecks[i])
                {
                    switch (i)
                    {
                        case 0:
                            message = "Not enough lowercase characters in your password.\n" +
                                      "Please try again.";
                            return false;
                        case 1:
                            message = "Not enough uppercase characters in your password.\n" +
                                      "Please try again.";
                            return false;
                        case 2:
                            message = "Not enough special characters in your password.\n" +
                                      "Please try again.";
                            return false;
                        case 3:
                            message = "Not enough numbers in your password.\n" +
                                      "Please try again.";
                            return false;
                        default:
                            break;
                    }
                }
            }

            return true;
        }
        //checks your passkey to make sure it is a valid entry
        public static bool CheckSecurityAnswers(ref string message, string[] entrys, string[] actual)
        {
            for (int i = 0; i < entrys.Length; i++)
            {
                if (entrys[i] != actual[i])
                {
                    message = "Answers are incorrect.\nPleasy try again.";
                    return false;
                }
            }

            message = "Success!";
            return true;
        }
        //checks the email to make sure its valid
        private static bool CheckValidEmailCharacters(char item)
        {
            return (item == '+' || item == '~' || item == '_' || item == '-' || item == '.');
        }
    }
}
