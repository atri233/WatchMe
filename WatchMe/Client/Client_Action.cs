using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using WatchMe.application;

namespace WatchMe;

public class Client_Action
{
    /// <summary>
    /// 获取系统环境
    /// </summary>
    /// <param name="o">数据流</param>
    /// <param name="showEve">是否显示细节</param>
    public static void Get_Info(Object? o,string showEve)
    {
        LogMe.Receive($"收到请求:"+$"GetInfo({showEve})");
        try
        {
            var networkStream = (NetworkStream)o;
            var bytes = Encoding.Default.GetBytes(JsonSerializer.Serialize(new GetInfo(Convert.ToBoolean(showEve)),JsonOptions.Option()));
            networkStream?.Write(bytes, 0, bytes.Length);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"成功");
            Console.ResetColor();
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("获取系统环境出错"+e);
            Console.ResetColor();
            throw;
        }
        
    }
}