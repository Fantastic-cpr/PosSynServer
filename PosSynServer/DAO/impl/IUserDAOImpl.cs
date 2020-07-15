using System;
using ConnectBridge;
using ConnectBridge.Mapper;
using MySql.Data.MySqlClient;
using PosSynServer.Util;

namespace PosSynServer.DAO.impl {
    public class IUserDAOImpl : IUserDAO {
        private const string databaseName = "netgame";

        public ReturnCode Register(User u) {
            // 鉴别用户是否存在
            User existUser = ExistUser(u.username);
            if (existUser != null) {
                return ReturnCode.RegisterUserExist;
            }

            // 注册逻辑
            MySqlConnection conn = null;
            MySqlCommand cmd = null;
            MySqlTransaction transaction = null;
            string sql =
                $"insert into user (username, password, phone) values " +
                $"('{u.username}', '{u.password}', '{u.phone}')";
            try {
                conn = MySQLConnectUtil.GetConnection(databaseName);
                cmd = new MySqlCommand(sql, conn);
                transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;
                cmd.ExecuteNonQuery();
                transaction.Commit();
                return ReturnCode.RegisterSuccess;
            }
            catch (Exception e) {
                transaction?.Rollback();
                Console.WriteLine(e);
                return ReturnCode.RegisterServerTearDown;
            }
            finally {
                MySQLConnectUtil.CloseConnection(conn, cmd);
            }
        }

        public User ExistUser(string username) {
            MySqlConnection conn = null;
            MySqlCommand cmd = null;
            MySqlDataReader reader = null;
            string sql = $"select * from user where username = '{username}'";
            try {
                conn = MySQLConnectUtil.GetConnection(databaseName);
                cmd = new MySqlCommand(sql, conn);
                reader = cmd.ExecuteReader();
                if (reader.Read()) {
                    User queryUser = new User();
                    queryUser.id = reader.GetInt64("id");
                    queryUser.username = reader.GetString("username");
                    return queryUser;
                }
                else {
                    return null;
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
            finally {
                MySQLConnectUtil.CloseConnection(conn, cmd, reader);
            }
        }

        public ReturnCode Login(ref User u) {
            MySqlConnection conn = null;
            MySqlCommand cmd = null;
            MySqlDataReader reader = null;
            string sql = $"select * from user where username = '{u.username}'";
            try {
                conn = MySQLConnectUtil.GetConnection(databaseName);
                cmd = new MySqlCommand(sql, conn);
                reader = cmd.ExecuteReader();
                if (reader.Read()) {
                    if (reader.GetString("password").Equals(u.password)) {
                        u.id = reader.GetInt64("id");
                        u.phone = reader.GetString("phone");
                        return ReturnCode.LoginSuccess;
                    }

                    return ReturnCode.LoginPasswordErr;
                }

                return ReturnCode.LoginUserDontExist;
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return ReturnCode.LoginServerTearDown;
            }
            finally {
                MySQLConnectUtil.CloseConnection(conn, cmd, reader);
            }
        }
    }
}