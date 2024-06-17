using System.Text.Json;
using WatchMe.application;

namespace WatchMe;

public class Program
{
    public static void Main(string[] args)
    {
        // var getInfo = new GetInfo(false);
        // Console.Write("本机环境：\n"+JsonSerializer.Serialize(getInfo,JsonOptions.Option()));
        
        //手动输入端口
        // LogSelf.Receive("输入监听端口：");
        // var consoleKeyInfo =Convert.ToInt32(Console.ReadLine());
        // var client = new Client(consoleKeyInfo); //配置服务器端口
        
        //开发期自动端口
        var client = new Client(9944); //配置服务器端口
        
        client.Start(); //开启服务器
    }
}