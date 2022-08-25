using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Globalization;
using System.IO;
namespace CBP_EMS_SP.Common
{



    public class CBPTokenGeneration : NameValueCollection
    {
        //        private const string cryptoKey = "typedef(node*)";
        //private readonly byte[] IV = new byte[8] { 240, 3, 45, 29, 0, 76, 173, 59 };
        //private const string cryptoKey = "M8tKmt177K";
        //private readonly byte[] IV = new byte[8] { 240, 3, 45, 29, 0, 76, 173, 59 };
        private static string cryptoKey = "b8dJk044gfy4jQ44U1Pkw5k8HPRyoipJ";
        private static readonly byte[] IV = new byte[16] { 70, 76, 78, 109, 89, 72, 103, 80, 110, 69, 89, 75, 69, 66, 75, 113 };


        private bool IsTried = false;
        private const string timeStampKey = "__TimeStamp__";
        /// <summary>
        /// Returns the encrypted query string.
        /// </summary>
        public string EncryptedString
        {
            get
            {
                return HttpUtility.UrlEncode(encrypt(serialize()));
            }
        }

        private DateTime _expireTime = DateTime.MaxValue;
        /// <summary>
        /// The timestamp in which the EncryptedString should expire
        /// </summary>
        public DateTime ExpireTime
        {
            get
            {
                return _expireTime;
            }
            set
            {
                _expireTime = value;
            }
        }


        public CBPTokenGeneration() : base() { }

        public CBPTokenGeneration(string encryptedString)
        {
            if (encryptedString.Contains("?culture=es") && (!string.IsNullOrEmpty(encryptedString)))
            {
                encryptedString = encryptedString.Replace("?culture=es", "");
            }
            deserialize(decrypt(encryptedString));

            // Compare the Expiration Time with the current Time to ensure
            // that the queryString has not expired.
            /* if (DateTime.Compare(ExpireTime, DateTime.Now) < 0)
             {
                 throw new ExpiredQueryStringException();
             }*/
        }




        #region [Methods]
        /// <summary>
        /// Returns the EncryptedString property.
        /// </summary>
        public override string ToString()
        {
            return EncryptedString;
        }

        /// <summary>
        /// Encrypts a serialized query string 
        /// </summary>
        public string encrypt(string serializedQueryString)
        {
            /* byte[] buffer = Encoding.ASCII.GetBytes(serializedQueryString);
             TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
             MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
             des.Key = MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(cryptoKey));
             des.IV = IV;
             return Convert.ToBase64String(
                 des.CreateEncryptor().TransformFinalBlock(
                     buffer,
                     0,
                     buffer.Length
                 )
             );*/
            using (AesCryptoServiceProvider myAes = new AesCryptoServiceProvider())
            {
                return Convert.ToBase64String(EncryptStringToBytes_Aes(serializedQueryString, ASCIIEncoding.ASCII.GetBytes(cryptoKey), IV));
            }
        }

        /// <summary>
        /// Decrypts a serialized query string
        /// </summary>
        public string decrypt(string encryptedQueryString)
        {

            try
            {
                /*byte[] buffer = Convert.FromBase64String(encryptedQueryString);
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
                des.Key = MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(cryptoKey));
                des.IV = IV;
                return Encoding.ASCII.GetString(
                    des.CreateDecryptor().TransformFinalBlock(
                        buffer,
                        0,
                        buffer.Length
                    )
                );*/
                using (AesCryptoServiceProvider myAes = new AesCryptoServiceProvider())
                {
                    return DecryptStringFromBytes_Aes(Convert.FromBase64String(encryptedQueryString), ASCIIEncoding.ASCII.GetBytes(cryptoKey), IV);
                }
            }
            catch (CryptographicException ex)
            {
                return "";
            }
            catch (FormatException ex1)
            {
                if (!IsTried)
                {
                    IsTried = true;
                    // Some softwares automatically encodes the url
                    return decrypt(HttpUtility.UrlDecode(encryptedQueryString).Replace(" ", "+"));

                }
                else
                {
                    throw new InvalidQueryStringException();
                }
            }

        }

        /// <summary>
        /// Deserializes a decrypted query string and stores it
        /// as name/value pairs.
        /// </summary>
        private void deserialize(string decryptedQueryString)
        {


                string[] nameValuePairs = decryptedQueryString.Split('&');
                for (int i = 0; i < nameValuePairs.Length; i++)
                {
                    string[] nameValue = nameValuePairs[i].Split('=');
                    if (nameValue.Length == 2)
                    {
                        base.Add(nameValue[0], nameValue[1]);
                    }
                }
            // Ensure that timeStampKey exists and update the expiration time.
            /* if (base[timeStampKey] != null)
                 _expireTime = DateTime.Parse(base[timeStampKey],null,System.Globalization.DateTimeStyles.);*/
        }

        /// <summary>
        /// Serializes the underlying NameValueCollection as a QueryString
        /// </summary>
        private string serialize()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string key in base.AllKeys)
            {
                sb.Append(key);
                sb.Append('=');
                sb.Append(base[key]);
                sb.Append('&');
            }

            // Append timestamp
            sb.Append(timeStampKey);
            sb.Append('=');
            //try
            //{
            //    sb.Append(_expireTime);
            //    alWLog.WriteLog("", "No Exception for _expireTime ", "_expireTime " + _expireTime);
            //}
            //catch (ArgumentOutOfRangeException ex)
            //{                
            _expireTime = CultureInfo.CurrentCulture.DateTimeFormat.Calendar.MaxSupportedDateTime;
            sb.Append(_expireTime);
            //    alWLog.WriteLog(ex, "Exception caught for _expireTime ", "New _expireTime " + _expireTime);
            //}


            return sb.ToString();
        }

        static public byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            // Create an AesCryptoServiceProvider object 
            // with the specified key and IV. 
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt =
                            new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream. 
            return encrypted;
        }

        static public string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;
            // Create an AesCryptoServiceProvider object 
            // with the specified key and IV. 
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt =
                            new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream 
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
        #endregion


    }


    #region [Exception Classes]

    /// <summary>
    /// Thrown when attempting to decrypt or deserialize an invalid encrypted queryString.
    /// </summary>
    public class InvalidQueryStringException : System.Exception
    {
        public InvalidQueryStringException() : base() { }
    }


    /// <summary>
    /// Thrown when a queryString has expired and is therefore no longer valid.
    /// </summary>
    public class ExpiredQueryStringException : System.Exception
    {
        public ExpiredQueryStringException() : base() { }
    }
    #endregion
}

