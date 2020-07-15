using System.Collections.Generic;
using ConnectBridge;
using LitJson;
using Photon.SocketServer;

namespace PosSynServer.Handler
{
    public class SyncPlayerHandler : BaseHandler
    {
        public SyncPlayerHandler()
        {
            OpCode = OperationCode.SyncPlayer;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters,
            ClientPeer clientPeer)
        {
            //取得在线玩家列表
            List<string> usernameList = new List<string>();
            foreach (MyClientPeer peer in PosSynServer.Instance.peerList)
            {
                if (!string.IsNullOrEmpty(peer.username) && peer != clientPeer)
                {
                    usernameList.Add(peer.username);
                }
            }

            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add((byte)ParameterCode.UsernameList, JsonMapper.ToJson(usernameList));
            OperationResponse resp = new OperationResponse(operationRequest.OperationCode);
            resp.Parameters = data;
            clientPeer.SendOperationResponse(resp, sendParameters);

            //通知其他客户端有新的客户端加入
            foreach (MyClientPeer peer in PosSynServer.Instance.peerList)
            {
                if (!string.IsNullOrEmpty(peer.username) && peer != clientPeer)
                {
                    EventData eventData = new EventData((byte)EventCode.NewPlayer);
                    Dictionary<byte, object> eData = new Dictionary<byte, object>();
                    eData.Add((byte)ParameterCode.Username, (clientPeer as MyClientPeer)?.username);
                    eventData.SetParameters(eData);
                    peer.SendEvent(eventData, sendParameters);
                }
            }
        }
    }
}