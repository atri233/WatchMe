

namespace WatchMe;

public class Program
{
    public static void Main(string[] args)
    {
        var getInfo = new GetInfo(false);
        getInfo.SetInfo();
        getInfo.Get();

        // var client = new Client(9944);    //配置服务器端口
        // client.Start();     //开启服务器
    }
}