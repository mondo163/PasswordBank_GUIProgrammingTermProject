using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptDecryptLib
{
    public class HillCipherMethod : IStringEncryptDecrypt
    {
        public bool IsActive { get; set; }
        private string OriginalPhrase { get; set; }
        public string PassKey { get; set; }
        private int[,] KeyMatrix { get; set; }
        public HillCipherMethod()
        {
            IsActive = false;
            KeyMatrix = null;
        }
        //returns the comparison result to the stored private string
        //Had bugs with this method and ended up not being able to use it..
        public void Decrypt(string Entry)
        {
            throw new NotImplementedException();
        }

        public void Encrypt(string entry)
        {
            //sets original phrase for the encryption
            OriginalPhrase = entry;
            // create matrixs from the entry received

            CreateKeyMatrix(entry.Length);

            //using the key matrix created, encrypt the phrase passed in
            //by the user
            //convert every entry into a int array
            int [] encryptedEntryInts = new int[entry.Length];
            for (int i = 0; i < entry.Length; i++)
            {
                for (int j = 0; j < entry.Length; j++)
                {
                    encryptedEntryInts[i] += KeyMatrix[i, j] * ((int)entry[j] - 97);  
                }
                
            }
            //create encrypted phrase and store in property for the passphrase;
            string encryptedPhrase = "";
            for (int i = 0; i < encryptedEntryInts.Length; i++)
            {
                encryptedPhrase += (char)((encryptedEntryInts[i]%42) + 48);
            }

            PassKey = encryptedPhrase;
            IsActive = true;
        }
        //Creates a random key matrix dimensional based on the length of the string entry.
        //Makes sure that the key matrix is invertible so that an inverse matrix
        //can be created to decryp the user's string entry.
        private void CreateKeyMatrix(int wordLength)
        {
            Random rand = new Random();
   
            KeyMatrix = new int[wordLength, wordLength];
            for (int i = 0; i < wordLength; i++)
            {
                for (int j = 0; j < wordLength; j++)
                {
                    KeyMatrix[i, j] = rand.Next() % 42;
                }
            }
              
        }
        //recursive method that determines the determinant of a matrix. 
        //would have been used if it werent for the decrypt not working...
        private int Determinant(int det, int length, int [,] matrix)
        {
            //base case for finding the determinant of a matrix
            int count1, count2;
            int[,] subMatrix = new int[length, length];
            if (length == 2)
            {
                return (matrix[0, 0] * matrix[1, 1]) - (matrix[1, 0] * matrix[0, 1]);
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    count1 = 0;
                    for (int j = 1; j < length; j++)
                    {
                        count2 = 0;
                        for (int k = 0; k < length; k++)
                        {
                            if (k == i)
                            {
                                continue;
                            }
                            subMatrix[count1, count2] = matrix[j, k];
                            count2++;
                        }
                        count1++;
                    }
                    det += ((int)System.Math.Pow(-1, i) * matrix[0, i] * Determinant(det, length - 1, subMatrix));
                }
            }
            return det;
        }
        
    }
}
