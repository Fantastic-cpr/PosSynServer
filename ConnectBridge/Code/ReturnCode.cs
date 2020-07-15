namespace ConnectBridge {
    public enum ReturnCode : short {
        RegisterSuccess, // 注册成功
        RegisterInfoNotComplete, // 注册信息不全
        RegisterServerTearDown, // 注册服务器出错
        RegisterUserExist, // 注册服务器出错
        LoginSuccess, // 登陆成功
        LoginUserDontExist, // 登录用户不存在
        LoginPasswordErr, // 密码错误
        LoginServerTearDown, // 登陆服务器出错
        LoginInfoNotComplete, // 注册信息填入不全
    }
}