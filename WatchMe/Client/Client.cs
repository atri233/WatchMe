using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

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
                var acceptTcpClient = _tcpListener.AcceptTcpClient(); //循环监听是否有用户连入（阻塞方法）
                var thread = new Thread(ListenUser); //单独为不同用户开启线程进行服务
                thread.Name = ((IPEndPoint)acceptTcpClient.Client.RemoteEndPoint).Address.ToString(); //用户ip为线程名
                thread.Start(acceptTcpClient); //运行线程
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
    ///     服务器开始监听（输出监听数据）
    /// </summary>
    private static void ListenUser(object? acceptTcpClient)
    {
        try
        {
            var tcpClient = (TcpClient)acceptTcpClient;
            var networkStream = tcpClient.GetStream(); //接收网络数据流(自动释放)(阻塞式的方式)
            using (networkStream)
            {
                //输入欢迎消息
                var bytes = Encoding.Default.GetBytes("welcome");
                networkStream.Write(bytes, 0, bytes.Length);
                
                //开启一个线程发心跳
                var thread = new Thread(HeartBeat);
                thread.Name = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString() + "heart";
                thread.Start(tcpClient);

                var buffer = new byte[10240]; //开辟10240字节的空间维护收到的数据(10MB)/开启细节占用时占用比较多
                int getbytes; //获取的数组长度
                var messageStream = new MemoryStream(); // 使用内存作临时缓存
                // 读取数据并存储到字节数组中
                while (true)
                {
                    try
                    {
                        //以下为数据读取逻辑
                        getbytes = networkStream.Read(buffer, 0, buffer.Length); //阻塞
                        if (getbytes > 0)
                        {
                            // 将接收到的数据写入到缓存中
                            messageStream.Write(buffer, 0, getbytes);
                        }

                        var getString = Encoding.Default.GetString(messageStream.ToArray()); //先转字节再转字符串
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

                        Console.WriteLine(typeGet);
                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(thread.Name + "：连接非主动断开：\n" + e);
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
                var tcpClientAvailable = tcpClient.Available;
                if (tcpClientAvailable != 0)
                {
                    Console.WriteLine(Thread.CurrentThread.Name + "：心跳循环停止:\n");
                    Console.WriteLine(Thread.CurrentThread.Name + "：用户断开连接");
                    break;
                }

                Thread.Sleep(1000);
            }
            catch (ObjectDisposedException e)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                // Console.WriteLine(Thread.CurrentThread.Name + "：心跳循环停止:\n" + e);
                Console.WriteLine(Thread.CurrentThread.Name + "：用户断开连接/对象释放");
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