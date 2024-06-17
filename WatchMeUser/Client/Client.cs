using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Text;
using WatchMe;

namespace WatchMeUser;

public class Client
{
    static string Head = "WMTYPE"; //头信息
    static string Tail = "WMOVER"; //尾信息
    private TcpClient _tcpClient;
    private string _ipConnect;
    private int _port;

    /// <summary>
    /// 传入连接ip与端口
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    public Client(string ip, int port)
    {
        _ipConnect = ip;
        _port = port;
        //创建实例连接（IP与端口）（端口调用时传入）
        try
        {
            _tcpClient = new TcpClient(_ipConnect, _port);

            var thread = new Thread(Start);
            thread.Start(_tcpClient);
        }
        catch (Exception e)
        {
            LogSelf.Error("连接对应服务器失败：\n" + e);
            throw;
        }
    }

    /// <summary>
    /// 获取并处理服务器信息
    /// </summary>
    /// <param name="_tcpClient">链接实体</param>
    private void Start(Object? _tcpClient)
    {
        LogSelf.Success("成功连接服务器：" + $"ip:{_ipConnect}" + $"端口:{_port}");
        try
        {
            var tcpClient = (TcpClient)_tcpClient;
            var networkStream = tcpClient.GetStream(); //接收网络数据流(阻塞式的方式)

            using (networkStream)
            {
                //输入测试消息
                var bytess = Encoding.Default.GetBytes("GetInfo" + Head + "false");
                networkStream.Write(bytess, 0, bytess.Length);


                int getbytes; //存储读取数据的长度
                var arrayPool = ArrayPool<byte>.Shared; //字节数组作读取缓存
                // var memoryPool = MemoryPool<byte>.Shared; //内存池作动态缓存
                // var messageStream = new MemoryStream(); // 内存流作中间缓存
                while (true)
                {
                    try
                    {
                        var bytes = arrayPool.Rent(2048); //借用2048字节的数组空间维护收到的数据

                        getbytes = networkStream.Read(bytes, 0, bytes.Length); //阻塞

                        if (getbytes <= 0) continue; //无数据跳过

                        var data = bytes.Take(getbytes); //获取已知长度的数据

                        var getString = Encoding.Default.GetString(data.ToArray()); //转字符串（先转字节再转字符串）

                        var dataType = ResultWM<string>.GetDataType(getString); //获取处理后数据

                        var typeGet = TypeSolve.TypeGet(dataType); //获取相对应类型的命令
                        //根据命令类别触发不同方法
                        switch (typeGet)
                        {
                            case "String":
                                LogSelf.Result("服务器:"+dataType[1]);
                                break;
                            case "Action_Get_Info":
                                Client_Action.Get_Info(dataType[1]);
                                break;
                        }

                        arrayPool.Return(bytes); //返还字节数组占用
                    }
                    catch (Exception e)
                    {
                        LogSelf.Warning(Thread.CurrentThread.Name + "：连接非主动断开：\n" + e);
                        break;
                    }
                }
            }
        }
        catch (Exception e)
        {
            LogSelf.Error("服务器连接异常：\n" + e);
            throw;
        }
    }
}