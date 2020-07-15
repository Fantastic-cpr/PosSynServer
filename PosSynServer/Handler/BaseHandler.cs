using ConnectBridge;
using Photon.SocketServer;

namespace PosSynServer.Handler {
    public abstract class BaseHandler {
        public OperationCode OpCode;

        public abstract void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters,ClientPeer clientPeer);
    }
}