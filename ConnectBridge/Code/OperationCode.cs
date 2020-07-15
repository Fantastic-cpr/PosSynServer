namespace ConnectBridge {
    public enum OperationCode : byte { //区分操作类型的枚举对象
        Default,
        Login,
        Register,
        SyncPos,
        SyncPlayer,
    }
}