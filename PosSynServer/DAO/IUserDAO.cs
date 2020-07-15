using ConnectBridge;
using ConnectBridge.Mapper;

namespace PosSynServer.DAO {
    public interface IUserDAO {
        ReturnCode Register(User u);
        User ExistUser(string username);
        ReturnCode Login(ref User u);
    }
}