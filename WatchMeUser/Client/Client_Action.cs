using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace WatchMe;

public class Client_Action
{
    static string Head = "WMTYPE"; //头信息
    static string Tail = "WMOVER"; //尾信息
    
    /// <summary>
    /// 反序列化获取系统环境对象
    /// </summary>
    /// <param name="o">字节数组</param>
    public static void Get_Info(string? o)
    {
        try
        {
            if (o == null) return;
            //TODO 处理反序列化的对象
            var getInfo = JsonSerializer.Deserialize<GetInfo>(o);
            LogSelf.Success("Action_Get_Info成功");
        }
        catch (Exception e)
        {
            LogSelf.Error("序列化出错/写入流出错\n"+e);
            throw;
        }
        
    }
}