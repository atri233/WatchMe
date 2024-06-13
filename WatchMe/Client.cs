using System.Net;
using System.Net.Sockets;

namespace WatchMe;

public class Client
{
    private readonly IPAddress _ipAddress;
    private readonly int _port;
    private readonly TcpListener _tcpListener;

    /// <summary>
    ///     有参构造，传入端口开启实例
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
        Console.WriteLine($"服务器启动——本机IP：{_ipAddress},端口：{_port}——等待用户接入...");
        ListenUser();
    }
    /// <summary>
    ///     服务器开始监听（输出监听数据）
    /// </summary>
    private void ListenUser()
    {
        while (true)
            try
            {
                var tcpClient = _tcpListener.AcceptTcpClient(); //每次接受一个用户端开启一个TcpClient
                var networkStream = tcpClient.GetStream(); //接收网络数据流
                //开启二进制流
                BinaryReader reader = new(networkStream);
                BinaryWriter writer = new(networkStream);
                //开启一个线程发心跳
                var thread = new Thread(HeartBeat);
                thread.Start(networkStream);
                //循环接受用户传来的信息
                while (true)
                {
                    try
                    {
                        lock (reader)
                        {
                            var readString = reader.ReadString();
                            Console.WriteLine("接受的网络信息流：" + readString);
                            
                        }
                        
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("连接异常关闭："+e);
                        break;
                    }
                }
                networkStream.Dispose(); //释放数据流中的数据
                tcpClient.Close();  //关闭连接实例
            }
            catch (Exception e)
            {
                Console.WriteLine("服务器异常关闭："+e);
                break;
            }
    }
    /// <summary>
    ///     心跳循环
    /// </summary>
    /// <param name="data"></param>
    private static void HeartBeat(object? data)
    {
        if (data == null) return;
        while (true)
        {
            var writer = new BinaryWriter((NetworkStream)data);
            writer.Write("Live");
            Thread.Sleep(2000);
        }
    }
}