using System.Buffers;
using System.Net.Sockets;
using System.Text;
using WatchMe;

namespace WatchMeUser;

class Program
{
    static string Head = "WMTYPE"; //头信息

    static void Main(string[] args)
    {
        // // 手动输入IP
        //  LogSelf.Receive("输入监听IP：");
        //  var consoleKeyIP = (Console.ReadLine());
        // // 手动输入端口
        //  LogSelf.Receive("输入监听端口：");
        // // 输入ip地址
        //  var consoleKeyPort =Convert.ToInt32(Console.ReadLine());
        //  var client = new Client(consoleKeyIP,consoleKeyPort); //配置服务器端口
        
        var client = new Client("127.0.0.1",9944);
        // Console.ReadKey();
    }
}