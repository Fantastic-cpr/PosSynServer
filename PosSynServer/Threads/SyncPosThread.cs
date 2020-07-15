using System.Collections.Generic;
using System.Threading;
using ConnectBridge;
using LitJson;
using Photon.SocketServer;

namespace PosSynServer.Threads {
    public class SyncPosThread {
        private Thread t;

        public void Run() {
            t = new Thread(UpdatePosition);
            t.IsBackground = true;
            t.Start();
        }

        public void Stop() {
            t.Abort();
        }

        private void UpdatePosition() {
            Thread.Sleep(5000);
            while (true) {
                Thread.Sleep(20);
                SendPosition();
            }
        }

        private void SendPosition() {
            List<PlayerPositionData> playerDataList = new List<PlayerPositionData>();
            foreach (MyClientPeer peer in PosSynServer.Instance.peerList)
            {
                if (!string.IsNullOrEmpty(peer.username))
                {
                    playerDataList.Add(new PlayerPositionData()
                    {
                        Username = peer.username,
                        Pos = new Vector3Data()
                        {
                            X = peer.x,
                            Y = peer.y,
                            Z = peer.z
                        },
                        Angle = new Vector3Data()
                        {
                            X = peer.anglex,
                            Y = peer.angley,
                            Z = peer.anglez
                        }
                    });
                }
            }

            string playerDataListJson = JsonMapper.ToJson(playerDataList);
            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add((byte)ParameterCode.PlayerDataList, playerDataListJson);
            foreach (MyClientPeer peer in PosSynServer.Instance.peerList)
            {
                if (!string.IsNullOrEmpty(peer.username))
                {
                    EventData eventData = new EventData((byte)EventCode.SyncPosition);
                    eventData.Parameters = data;
                    peer.SendEvent(eventData, new SendParameters());
                }
            }
        }
    }
}