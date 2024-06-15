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
        try
        {
            var networkStream = (NetworkStream)o;
            var bytes = Encoding.Default.GetBytes(JsonSerializer.Serialize(new GetInfo(Convert.ToBoolean(showEve)),JsonOptions.Option()));
            var bytes2 = Encoding.Default.GetBytes("测试");
            networkStream?.Write(bytes2, 0, bytes2.Length);
        }
        catch (Exception e)
        {
            Console.WriteLine("获取系统环境出错"+e);
            throw;
        }
        
    }
}