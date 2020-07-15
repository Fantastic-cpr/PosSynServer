using System;

namespace ConnectBridge.Mapper {
    [Serializable]
    public class User {
        public long id;
        public string username;
        public string password;
        public string phone;

        public bool RegisterInfoComplete() {
            return !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) &&
                   !string.IsNullOrEmpty(phone);
        }

        public bool LoginInfoComplete() {
            return !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password);
        }

        public override string ToString() {
            return $"{GetType()} : id = {id}, username = {username}, password = {password}, phone = {phone}";
        }
    }
}