using System.Buffers;
using System.Net.Sockets;
using System.Text;

namespace WatchMeUser;

class Program
{
    static string Head = "WMTYPE"; //头信息

    static void Main(string[] args)
    {
        try
        {
            // 创建一个 TcpClient 对象并连接到服务器
            var tcpClient = new TcpClient();
            tcpClient.Connect("127.0.0.1", 9944); // 服务器的 IP 地址和端口号

            using (tcpClient)
            {
                var networkStream = tcpClient.GetStream(); //接收网络数据流(自动释放)(阻塞式的方式)

                //欢迎消息
                var be = Encoding.Default.GetBytes("GetInfo" + Head + "false");
                networkStream.Write(be, 0, be.Length);

                var arrayPool = ArrayPool<byte>.Shared;
                int getbytes; //获取的数组长度
                while (true)
                {
                    try
                    {
                        var buffer = arrayPool.Rent(2048);//借用1024字节的数组空间维护收到的数据
                        getbytes = networkStream.Read(buffer, 0, buffer.Length); //阻塞(接受单次传输的所有包)
                        // arrayPool.Return(buffer);//归还数组缓存
                        if (getbytes <= 0) continue;
                        
                        var enumerable = buffer.Take(getbytes);
                        var getBytes = enumerable.ToArray(); //收到的消息转字节数组
                        var getString = Encoding.Default.GetString(getBytes); //将字节数组编码成String字符串
                        arrayPool.Return(buffer);//归还数组缓存
                        
                        Console.WriteLine(getString);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        break;
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