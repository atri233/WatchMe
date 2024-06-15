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
                var be = Encoding.Default.GetBytes("string" + Head + "Connet");
                networkStream.Write(be, 0, be.Length);

                var buffer = new byte[1024]; //开辟1024字节的空间维护收到的数据
                int getbytes; //获取的数组长度
                var messageStream = new MemoryStream(); // 使用内存作临时缓存

                while (true)
                {
                    try
                    {
                        getbytes = networkStream.Read(buffer, 0, buffer.Length); //阻塞


                        if (getbytes > 0)
                        {
                            // 将接收到的数据写入到缓存中
                            messageStream.Write(buffer, 0, getbytes);
                        }

                        var getBytes = messageStream.ToArray(); //收到的消息转字节数组
                        var getString = Encoding.Default.GetString(getBytes); //将字节数组编码成String字符串
                        //普通文本输出
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