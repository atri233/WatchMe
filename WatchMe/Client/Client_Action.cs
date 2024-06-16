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
        LogSelf.Receive($"接收到请求:Get_Info({showEve})");
        try
        {
            var networkStream = (NetworkStream)o;
            var bytes = Encoding.Default.GetBytes(JsonSerializer.Serialize(new GetInfo(Convert.ToBoolean(showEve)),JsonOptions.Option()));
            networkStream?.Write(bytes, 0, bytes.Length);
            LogSelf.Success("成功"); 
        }
        catch (Exception e)
        {
            LogSelf.Error("获取系统环境出错"+e);
            throw;
        }
        
    }
}