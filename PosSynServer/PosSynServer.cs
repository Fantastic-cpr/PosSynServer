using System.Collections.Generic;
using System.IO;
using ConnectBridge;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net;
using log4net.Config;
using Photon.SocketServer;
using PosSynServer.Handler;
using PosSynServer.Threads;
using LogManager = ExitGames.Logging.LogManager;

namespace PosSynServer {
    //Server端主类,服务器需要使用继承ApplicationBase类的主类进行逻辑编写
    //主类负责与客户端的连接
    public class PosSynServer : ApplicationBase {
        //Logger对象，用于输出日志
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public static ILogger Log => log;

        //将服务类改为单例模式
        public static PosSynServer Instance { get; private set; }

        //所有的Handler集合
        public Dictionary<OperationCode, BaseHandler> handlerDict = new Dictionary<OperationCode, BaseHandler>();

        //所有ClientPeer
        public List<MyClientPeer> peerList = new List<MyClientPeer>();

        //同步线程
        private SyncPosThread syncPosThread = new SyncPosThread();

        //初始化
        protected override void Setup() {
            Instance = this;
            //日志初始化
            InitLog4Net();

            //Handler初始化
            InitHandler();
            syncPosThread.Run();
        }

        private void InitLog4Net() {
            GlobalContext.Properties["Photon:ApplicationLogPath"] =
                Path.Combine(ApplicationRootPath, "bin_Win64", "log"); //设置属性{Photon:ApplicationLogPath}
            FileInfo configFileInfo = new FileInfo(Path.Combine(BinaryPath, "log4net.config"));
            if (configFileInfo.Exists) {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance); //Log工厂设置插件
                XmlConfigurator.ConfigureAndWatch(configFileInfo); //读取xml配置文件
            }
        }

        private void InitHandler() {
            handlerDict.Add(OperationCode.Default, new DefaultHandler());
            handlerDict.Add(OperationCode.Login, new LoginHandler());
            handlerDict.Add(OperationCode.Register, new RegisterHandler());
            handlerDict.Add(OperationCode.SyncPos, new SyncPosHandler());
            handlerDict.Add(OperationCode.SyncPlayer, new SyncPlayerHandler());
        }

        //客户端与服务器创建连接
        protected override PeerBase CreatePeer(InitRequest initRequest) {
            MyClientPeer peer = new MyClientPeer(initRequest);
            peerList.Add(peer);
            return peer;
        }

        //服务关闭
        protected override void TearDown() {
            syncPosThread.Stop();
        }
    }
}