using System.Text.Json;
using WatchMe.application;

namespace WatchMe;

public class Program
{
    public static void Main(string[] args)
    {
        var getInfo = new GetInfo(false);

        // Console.Write("本机环境：\n"+JsonSerializer.Serialize(getInfo,JsonOptions.Option()));

        var client = new Client(9944); //配置服务器端口
        client.Start(); //开启服务器
    }
}