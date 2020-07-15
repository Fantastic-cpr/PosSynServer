using System;

namespace ConnectBridge {
    [Serializable]
    public class PlayerPositionData {
        public Vector3Data Pos { get; set; }
        public Vector3Data Angle { get; set; }
        public string Username{ get; set; }
    }
}