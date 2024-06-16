using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WatchMe;

/// <summary>
/// 服务端
/// </summary>
public class Client
{
    static string Head = "WMTYPE"; //头信息
    static string Tail = "WMOVER"; //尾信息
    private readonly IPAddress _ipAddress;
    private readonly int _port;
    private readonly TcpListener _tcpListener;

    /// <summary>
    /// 有参构造，传入端口开启实例
    /// </summary>
    /// <param name="port"></param>
    public Client(int port)
    {
        //获取服务端本机所有IP与端口
        _ipAddress = IPAddress.Any;
        _port = port;
        //创建实例监控IP与端口（端口调用时传入）
        _tcpListener = new TcpListener(_ipAddress, _port);
    }

    /// <summary>
    ///     服务器启动
    /// </summary>
    public void Start()
    {
        _tcpListener.Start();
        Console.WriteLine($"\n服务器启动——本机IP：{_ipAddress},端口：{_port}——等待用户接入...");
        WaitAndStart(); //开始监听
    }

    /// <summary>
    /// 监听是否有用户连接
    /// 开启多线程进行服务
    /// </summary>
    private void WaitAndStart()
    {
        while (true)
        {
            try
            {
                var acceptTcpClient = _tcpListener.AcceptTcpClient(); //循环监听是否有用户连入（阻塞方法）||此处唯一对象
                
                var thread = new Thread(ListenUser); //创建监听线程
                thread.Name = ((IPEndPoint)acceptTcpClient.Client.RemoteEndPoint).Address.ToString(); //用户ip为线程名
                thread.Start(acceptTcpClient); //运行监听线程
                
                var threadout = new Thread(OutUser);//闯将输出线程
                threadout.Name = ((IPEndPoint)acceptTcpClient.Client.RemoteEndPoint).Address.ToString(); //用户ip为线程名
                threadout.Start(acceptTcpClient);//运行输出线程
                
                Console.WriteLine("连接用户ip：" + thread.Name);
            }
            catch (Exception e)
            {
                Console.WriteLine("服务器出错：" + e);
                break;
            }
        }
    }

    /// <summary>
    ///     服务器开始监听（接收数据并处理）
    /// </summary>
    private static void ListenUser(object? acceptTcpClient)
    {
        try
        {
            var tcpClient = (TcpClient)acceptTcpClient;
            var networkStream = tcpClient.GetStream(); //接收网络数据流(自动释放)(阻塞式的方式)
            using (networkStream)
            {
                
                
                int getbytes; //存储读取数据的长度
                var arrayPool = ArrayPool<byte>.Shared; //字节数组作读取缓存
                // var memoryPool = MemoryPool<byte>.Shared; //内存池作动态缓存
                // var messageStream = new MemoryStream(); // 内存流作中间缓存
                while (true)
                {
                    try
                    {
                        var bytes = arrayPool.Rent(2048);//借用2048字节的数组空间维护收到的数据
                        
                        getbytes =  networkStream.Read(bytes, 0, bytes.Length); //阻塞
                        if (getbytes <= 0) continue;//无数据跳过

                        var data = bytes.Take(getbytes);
                        
                        var getString = Encoding.Default.GetString(data.ToArray()); //先转字节再转字符串
                        var dataType = ResultWM<string>.GetDataType(getString); //获取处理后数据
                        var typeGet = TypeSolve.TypeGet(dataType); //获取相对应类型的返回值
                        //根据命令类别触发不同方法
                        switch (typeGet)
                        {
                            case "String":
                                Console.WriteLine(dataType[1]);
                                break;
                            case "Action_Get_Info":
                                Client_Action.Get_Info(networkStream, dataType[1]);
                                break;
                        }
                        arrayPool.Return(bytes);//返还字节数组占用
                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(Thread.CurrentThread.Name + "：连接非主动断开：\n" + e);
                        Console.ResetColor();
                        break;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("服务器异常关闭：\n" + e);
            Console.ResetColor();
        }
    }
    /// <summary>
    /// 输出方法（包含心跳检测）
    /// </summary>
    /// <param name="acceptTcpClient"></param>
    private static void OutUser(object? acceptTcpClient)
    {
        var tcpClient = (TcpClient)acceptTcpClient;
        var networkStream = tcpClient.GetStream(); //接收网络数据流(自动释放)(阻塞式的方式)
        
        //输入欢迎消息
        var bytess = Encoding.Default.GetBytes("welcome");
        networkStream.Write(bytess, 0, bytess.Length);
        
        //开启一个线程发心跳
        var thread = new Thread(HeartBeat);
        thread.Name = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString() + "heart";
        thread.Start(tcpClient);
        
    }
    /// <summary>
    /// 心跳循环(使用TcpClient.Available判断用户是否断开连接)
    /// </summary>
    /// <param name="data"></param>
    private static void HeartBeat(object? networkStream)
    {
        if (networkStream == null) return;
        var tcpClient = (TcpClient)networkStream;
        while (true)
        {
            try
            {
                // var writer = new BinaryWriter((NetworkStream)data);
                // writer.Write("W");
                var tcpClientAvailable = tcpClient.Available;//用数字表示有多少数据在阻塞
                // if (tcpClientAvailable == 0)
                // {
                //     Console.WriteLine(Thread.CurrentThread.Name + "：心跳循环停止");
                //     Console.WriteLine(Thread.CurrentThread.Name + "：用户断开连接");
                //     break;
                // }
                //
                // Thread.Sleep(1000);
            }
            catch (ObjectDisposedException e)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                // Console.WriteLine(Thread.CurrentThread.Name + "：心跳循环停止:\n" + e);
                Console.WriteLine(Thread.CurrentThread.Name + "：用户断开连接/对象释放\n"+e);
                Console.ResetColor();
                break;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(Thread.CurrentThread.Name + "：心跳循环停止:\n" + e);
                Console.WriteLine(Thread.CurrentThread.Name + "服务端出错");
                Console.ResetColor();
                break;
            }
        }
    }
}