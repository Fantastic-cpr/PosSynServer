using System;
using MySql.Data.MySqlClient;

namespace PosSynServer.Util {
    public class MySQLConnectUtil {
        private MySQLConnectUtil() { }

        public static MySqlConnection GetConnection(string database, string user = "root", string password = "",
            string server = "127.0.0.1", string port = "3306") {
            try {
                string connectConfig =
                    $"server={server};port={port};database={database};user={user};password={password}";
                MySqlConnection conn = new MySqlConnection(connectConfig);
                conn.Open();
                return conn;
            }
            catch (Exception e) {
                Console.WriteLine("Sql连接打开失败");
                throw;
            }
        }

        public static void CloseConnection(MySqlConnection conn, MySqlCommand cmd = null, MySqlDataReader reader = null) {
            try {
                reader?.Close();
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
            finally {
                try {
                    cmd?.Dispose();
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
                finally {
                    try {
                        if (conn != null) {
                            conn.Close();
                            conn.Dispose();
                        }
                    }
                    catch (Exception e) {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }
        }
    }
}