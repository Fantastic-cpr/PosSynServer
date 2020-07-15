namespace ConnectBridge {
    public enum ParameterCode : byte { //传递字典数据的key
        RegisterUser, // 注册用户数据
        LoginUser, // 登录用户数据
        Username,
        Password,
        PositionX,
        PositionY,
        PositionZ,
        UsernameList,
        PlayerDataList,
        EulerAnglesX,
        EulerAnglesY,
        EulerAnglesZ,
    }
}