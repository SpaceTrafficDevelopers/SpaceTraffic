/**
Copyright 2016 FAV ZCU

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
using Core.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceTraffic.Utils.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Utils.Security.Tests
{
    [TestClass()]
    public class PasswordHasherTests
    {
        PasswordHasher hasher;

        [TestInitialize()]
        public void TestInitialize()
        {
            this.hasher = new PasswordHasher(PasswordHasher.DEF_ITERATION_COUNT);
        }

        #region Constructor
        [TestMethod()]
        public void PasswordHasherValidTest()
        {
            int expected = 10000;
            hasher = new PasswordHasher(expected);

            int value = (int)TestUtils.GetFieldValue(typeof(PasswordHasher), hasher, "iterationCount");
            object rng = TestUtils.GetFieldValue(typeof(PasswordHasher), hasher, "rng");

            Assert.AreEqual(expected, value);
            Assert.IsNotNull(rng);
        }
        
        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PasswordHasherInvalidTest()
        {
            hasher = new PasswordHasher(0);
        }
        #endregion

        #region HashPassword
        [TestMethod()]
        public void HashPasswordValidTest()
        {
            int hashLength = (int)TestUtils.GetFieldValue(typeof(PasswordHasher), hasher, "SALT_SIZE") + (int)TestUtils.GetFieldValue(typeof(PasswordHasher), hasher, "HASH_SIZE");
            string pass = "123456789";
            string result = hasher.HashPassword(pass);

            Assert.IsNotNull(result);
            Assert.IsTrue(Convert.FromBase64String(result).Length == hashLength);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HashPasswordInvalidTest()
        {
            string result = hasher.HashPassword(null);
        }
        #endregion

        #region GenerateRandomPassword
        [TestMethod()]
        public void GenerateRandomPasswordValidLengthTest()
        {
            int length = 10;
            string result = hasher.GenerateRandomPassword(length);

            Assert.IsNotNull(result);
            Assert.AreEqual(length, result.Length);
        }

        [TestMethod()]
        public void GenerateRandomPasswordInvalidLengthTest()
        {
            int length = -1;
            string result = hasher.GenerateRandomPassword(length);

            Assert.IsNull(result);
        }
        #endregion

        #region ValidatePassword
        [TestMethod()]
        public void ValidatePasswordValidSameTest()
        {
            string passwordHash = "o8Drx+MJghpMvCN5v0oGB1AB0m0TABBWjt+p1jFsAnvQkBWaGkqFiMo2r6fPeG5+";
            string password = "user";
            bool result = hasher.ValidatePassword(password, passwordHash);

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void ValidatePasswordValidNotSameTest()
        {
            string passwordHash = "o8Drx+MJghpMvCN5v0oGB1AB0m0TABBWjt+p1jFsAnvQkBWaGkqFiMo2r6fPeG5+";
            string password = "notSame";
            bool result = hasher.ValidatePassword(password, passwordHash);

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void ValidatePasswordInvalidShortHashTest()
        {
            string passwordHash = "o8Drx+MJghpMvCN5v0AB0m0TABBWjt+p1jFsAnvQkBWaGkqFiMo2r6fPeG5+";
            string password = "user";
            bool result = hasher.ValidatePassword(password, passwordHash);

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void ValidatePasswordInvalidPasswordNullTest()
        {
            string passwordHash = "o8Drx+MJghpMvCN5v0oGB1AB0m0TABBWjt+p1jFsAnvQkBWaGkqFiMo2r6fPeG5+";
            string password = null;
            bool result = hasher.ValidatePassword(password, passwordHash);

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void ValidatePasswordInvalidHashNullTest()
        {
            string passwordHash = null;
            string password = "user";
            bool result = hasher.ValidatePassword(password, passwordHash);

            Assert.IsFalse(result);
        } 
        #endregion
    }
}