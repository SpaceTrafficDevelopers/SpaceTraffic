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
using System.Web;

namespace SpaceTraffic.Utils.Security
{
    /// <summary>
    /// Class for password hashing and validation.
    /// Algorythm used: PBKDF2 with SHA-1
    /// </summary>
    public class PasswordHasher
    {
        public const int DEF_ITERATION_COUNT = 64000;

        private const int SALT_SIZE = 128 / 8;
        private const int HASH_SIZE = 256 / 8;

        private int iterationCount;
        private RandomNumberGenerator rng;

        /// <summary>
        /// Creates instance of PasswordHasher with specific number of iterations
        /// </summary>
        /// <param name="iterationCount">Number of iterations used in PBKDF2 algorythm</param>
        public PasswordHasher(int iterationCount)
        {
            if (iterationCount < 1)
                throw new InvalidOperationException("Invalid password hasher iteration count.");

            this.iterationCount = iterationCount;
            rng = RandomNumberGenerator.Create();
        }

        /// <summary>
        /// Encrypts password with PBKDF2 algorythm and returns it encoded in Base64
        /// </summary>
        /// <param name="password">Password to encrypt</param>
        /// <returns>Ecrypted password and salt encoded in Base64</returns>
        public string HashPassword(string password)
        {
            byte[] salt = new byte[SALT_SIZE];

            rng.GetBytes(salt);

            byte[] result = HashPasswordWithSalt(password, salt);

            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// Generates random password with given length
        /// </summary>
        /// <param name="length">Password length. Minimum value is 1</param>
        /// <returns>Random password or null when length is smaller than minimum (1)</returns>
        public string GenerateRandomPassword(int length)
        {
            if (length < 1)
                return null;

            byte[] passBytes = new byte[length + 6];
            rng.GetBytes(passBytes);

            return Convert.ToBase64String(passBytes).Substring(2,length);
        }

        /// <summary>
        /// Validates password to given encrypted password
        /// </summary>
        /// <param name="password">Password to encrypt</param>
        /// <param name="pwdHash">Encrypted password to compare</param>
        /// <returns>True if password is valid else False</returns>
        public bool ValidatePassword(string password, string pwdHash)
        {
            if (password == null || pwdHash == null)
            {
                return false;
            }

            byte[] salt = new byte[SALT_SIZE];
            byte[] pwdHashBytes = Convert.FromBase64String(pwdHash);

            if (pwdHashBytes.Length != (SALT_SIZE + HASH_SIZE))
            {
                return false;
            }

            Buffer.BlockCopy(pwdHashBytes, 0, salt, 0, salt.Length);

            byte[] newPwdHash = HashPasswordWithSalt(password, salt);

            return ByteArraysEqual(pwdHashBytes, newPwdHash);
        }

        /// <summary>
        /// Compares two byte arrays
        /// </summary>
        /// <param name="a">First byte array</param>
        /// <param name="b">Second byte array</param>
        /// <returns>True if arrays are same else False</returns>
        private bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }
            var areSame = true;
            for (var i = 0; i < a.Length; i++)
            {
                areSame &= (a[i] == b[i]);
            }
            return areSame;
        }

        /// <summary>
        /// Encrypts password with PBKDF2 algorythm and given salt
        /// </summary>
        /// <param name="password">Password to encrypt</param>
        /// <param name="salt">Salt used in encryption</param>
        /// <returns>Byte array containing encrypted password and salt</returns>
        private byte[] HashPasswordWithSalt(string password, byte[] salt)
        {
            byte[] result = new byte[HASH_SIZE + SALT_SIZE];

            if (password == null && salt == null)
            {
                return null;
            }

            byte[] hash = PBKDF2(password, salt, iterationCount, HASH_SIZE);

            Buffer.BlockCopy(salt, 0, result, 0, salt.Length);
            Buffer.BlockCopy(hash, 0, result, salt.Length, hash.Length);

            return result;
        }

        /// <summary>
        /// Encrypts password with PBKDF2 algorythm
        /// </summary>
        /// <param name="password">Password to encrypt</param>
        /// <param name="salt">Salt used in encryption</param>
        /// <param name="iterations">Number of iterations in PBKDF2 algorythm</param>
        /// <param name="outputBytes">Number of output bytes</param>
        /// <returns>Byte array containing encrypted password</returns>
        private byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt))
            {
                pbkdf2.IterationCount = iterations;
                return pbkdf2.GetBytes(outputBytes);
            }
        }
    }
}