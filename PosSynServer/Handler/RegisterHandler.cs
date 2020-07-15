using System.Collections.Generic;
using ConnectBridge;
using ConnectBridge.Mapper;
using ConnectBridge.Util;
using LitJson;
using Photon.SocketServer;
using PosSynServer.DAO;
using PosSynServer.DAO.impl;

namespace PosSynServer.Handler {
    public class RegisterHandler : BaseHandler {
        private IUserDAO userDao;

        public RegisterHandler() {
            userDao = new IUserDAOImpl();
            OpCode = OperationCode.Register;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters,
            ClientPeer clientPeer) {
            OperationResponse resp = new OperationResponse(operationRequest.OperationCode);
            Dictionary<byte, object> registerInfo = operationRequest.Parameters;
            string registerUserJson =
                DictUtil.GetValue(registerInfo, (byte) ParameterCode.RegisterUser) as string;
            User registerUser = JsonMapper.ToObject<User>(registerUserJson);
            if (registerUser != null && registerUser.RegisterInfoComplete()) { //进行请求处理
                resp.ReturnCode = (short) userDao.Register(registerUser);
            }
            else { //注册信息不全
                resp.ReturnCode = (short) ReturnCode.RegisterInfoNotComplete;
            }

            clientPeer.SendOperationResponse(resp, sendParameters);
        }
    }
}