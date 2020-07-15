using System;
using System.Data;
using ConnectBridge;
using ConnectBridge.Mapper;
using LitJson;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using PosSynServer.DAO;
using PosSynServer.DAO.impl;
using PosSynServer.Util;

namespace SQLTest.test {
    public class TestUser {
        private string databaseName = "netgame";
        private static User testUser;
        private IUserDAO userDao;

        static TestUser() {
            testUser = new User();
            testUser.username = "test";
            testUser.password = "testPwd";
            testUser.phone = "10000";
        }

        [Test]
        public void TestRegister() {
            userDao = new IUserDAOImpl();
            ReturnCode returnCode = userDao.Register(testUser);
            Assert.AreEqual(ReturnCode.RegisterSuccess, returnCode);
            returnCode = userDao.Register(testUser);
            Assert.AreEqual(ReturnCode.RegisterUserExist, returnCode);
        }

        [Test]
        public void TestExistUser() {
            userDao = new IUserDAOImpl();
            User existUser = userDao.ExistUser(testUser.username);
            if (existUser == null) {
                Console.WriteLine("用户不存在,可以继续注册");
            }
            else {
                Assert.AreEqual(existUser.username, testUser.username);
            }
        }

        [Test]
        public void TestUserLogin() {
            userDao = new IUserDAOImpl();
            ReturnCode returnCode = userDao.Login(ref testUser);
            Console.WriteLine(returnCode);
            Console.WriteLine(testUser);
        }
    }
}