using System.Net.Sockets;

namespace WatchMeUser;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // 创建一个 TcpClient 对象并连接到服务器
            var tcpClient = new TcpClient();
            tcpClient.Connect("127.0.0.1", 9944); // 服务器的 IP 地址和端口号
            using (tcpClient)
            {
                while (true)
                {
                    var networkStream = tcpClient.GetStream();
                    var reader = new BinaryReader(networkStream);
                    var writer = new BinaryWriter(networkStream);

                    writer.Write($"已连接");
                    Console.WriteLine("已传入信息");

                    while (true)
                    {
                        var readString = reader.ReadString();   //读取服务端传来的信息
                        Console.WriteLine(readString);
                    }
                }
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}