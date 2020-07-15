using System.Collections.Generic;
using ConnectBridge;
using ConnectBridge.Mapper;
using ConnectBridge.Util;
using LitJson;
using Photon.SocketServer;
using PosSynServer.DAO;
using PosSynServer.DAO.impl;

namespace PosSynServer.Handler {
    public class LoginHandler : BaseHandler {
        private IUserDAO userDao;

        public LoginHandler() {
            OpCode = OperationCode.Login;
            userDao = new IUserDAOImpl();
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters,
            ClientPeer clientPeer) {
            OperationResponse resp = new OperationResponse(operationRequest.OperationCode);
            Dictionary<byte, object> loginInfo = operationRequest.Parameters;
            string loginUserJson = DictUtil.GetValue(loginInfo, (byte) ParameterCode.LoginUser) as string;
            User loginUser = JsonMapper.ToObject<User>(loginUserJson);
            if (loginUser != null && loginUser.LoginInfoComplete()) { //进行请求处理
                resp.ReturnCode = (short) userDao.Login(ref loginUser);
                resp.Parameters = new Dictionary<byte, object>();
                resp[(byte) ParameterCode.LoginUser] = JsonMapper.ToJson(loginUser);
            }
            else { //注册信息不全
                resp.ReturnCode = (short) ReturnCode.LoginInfoNotComplete;
            }
            if (resp.ReturnCode == (short)ReturnCode.LoginSuccess)
            {
                MyClientPeer peer = clientPeer as MyClientPeer;
                if (peer != null) peer.username = loginUser.username;
            }
            clientPeer.SendOperationResponse(resp, sendParameters);
        }
    }
}