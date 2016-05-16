/**
Copyright 2010 FAV ZCU

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

**/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SpaceTraffic.Utils.Security
{
    /// <summary>
    /// Password hasher for Minigames. It used AES with PBKDF2 with HMAC-SHA1.
    /// </summary>
    public class MinigamePasswordHasher
    {
        /// <summary>
        /// Initialization vector for crypter. Length has to be 16 bytes.
        /// </summary>
        private const string INIT_VECTOR = "05yngXV9RCNhmsmt";

        /// <summary>
        /// PDKBF2 password.
        /// </summary>
        private const string PDKBF2_PASSWORD = "RrapN54SFry37HXkQuym0AOa5x4HrAeKBJQGgM6E";
        
        /// <summary>
        /// Salt.
        /// </summary>
        private const string SALT = "3Z1OH3m3CpUzGIA6E3vZkPTNMtatK9C5knnhMfaa";

        /// <summary>
        /// Method for encrypt password.
        /// </summary>
        /// <param name="password">password</param>
        /// <returns>encrypted password</returns>
        public string getEncryptedPassword(string password)
        {
            using (AesCryptoServiceProvider aesCryptServiceProvider = new AesCryptoServiceProvider())
            {
                ICryptoTransform transformation = getCryptographicTransformation(aesCryptServiceProvider, false);
                
                byte[] bytePassword = Encoding.UTF8.GetBytes(password);
                byte[] encryptedPassword = transformation.TransformFinalBlock(bytePassword, 0, bytePassword.Length);

                return Convert.ToBase64String(encryptedPassword);
            }
        }

        /// <summary>
        /// Method for decrypt password.
        /// </summary>
        /// <param name="encryptedPassword">encrypted password</param>
        /// <returns>decrypted password</returns>
        public string getOriginalPassword(string encryptedPassword)
        {
            using (AesCryptoServiceProvider aesCryptServiceProvider = new AesCryptoServiceProvider())
            {
                ICryptoTransform transformation = getCryptographicTransformation(aesCryptServiceProvider, true);

                byte[] fromBase = Convert.FromBase64String(encryptedPassword);
                byte[] decryptedPassword = transformation.TransformFinalBlock(fromBase, 0, fromBase.Length);
                
                return Encoding.UTF8.GetString(decryptedPassword);
            }
        }

        /// <summary>
        /// Method for AES creating decryptor or encrptor.
        /// </summary>
        /// <param name="aesCryptServiceProvider">aes provider</param>
        /// <param name="decrypt">true for decryptor, false for encrypter</param>
        /// <returns>crypter or decrypter instance</returns>
        private ICryptoTransform getCryptographicTransformation(AesCryptoServiceProvider aesCryptServiceProvider, bool decrypt)
        {
            aesCryptServiceProvider.Mode = CipherMode.CBC;
            aesCryptServiceProvider.Padding = PaddingMode.PKCS7;
            
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(PDKBF2_PASSWORD), Encoding.UTF8.GetBytes(SALT), 65536);

            byte[] key = rfc.GetBytes(INIT_VECTOR.Length);

            aesCryptServiceProvider.IV = Encoding.UTF8.GetBytes(INIT_VECTOR);
            aesCryptServiceProvider.Key = key;
            
            return decrypt ? aesCryptServiceProvider.CreateDecryptor() : aesCryptServiceProvider.CreateEncryptor();
        }
    }
}
