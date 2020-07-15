using ConnectBridge;
using ConnectBridge.Util;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using PosSynServer.Handler;

namespace PosSynServer {
    //客户端连接后，使用ClientPeer代表客户端的一个链接
    public class MyClientPeer : ClientPeer {
        public double x, y, z;
        public double anglex, angley, anglez;
        public string username;

        public MyClientPeer(InitRequest initRequest) : base(initRequest) { }

        //处理客户端请求
        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters) {
            BaseHandler handler = DictUtil.GetValue(PosSynServer.Instance.handlerDict,
                (OperationCode) operationRequest.OperationCode);
            if (handler != null) {
                handler.OnOperationRequest(operationRequest,sendParameters,this);
            }
            else {
                handler = DictUtil.GetValue(PosSynServer.Instance.handlerDict, OperationCode.Default);
                handler.OnOperationRequest(operationRequest, sendParameters, this);
            }
        }

        //断开连接
        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail) {
            PosSynServer.Instance.peerList.Remove(this);
        }
    }
}