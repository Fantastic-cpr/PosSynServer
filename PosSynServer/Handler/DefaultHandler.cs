using ConnectBridge;
using Photon.SocketServer;

namespace PosSynServer.Handler {
    public class DefaultHandler:BaseHandler {
        public DefaultHandler() {
            OpCode = OperationCode.Default;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer clientPeer) {
            
        }
    }
}