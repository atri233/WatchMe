using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace WatchMe;

public class Program
{
    public static void Main(string[] args)
    {
        var getInfo = new GetInfo(false);
        //序列化配置
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),//不转义所有字符（防止信息错乱）
            WriteIndented = true    //美化打印效果
        };
        
        Console.Write(JsonSerializer.Serialize(getInfo,jsonSerializerOptions));
        var deserialize = JsonSerializer.Deserialize<GetInfo>(JsonSerializer.Serialize(getInfo,jsonSerializerOptions));
        Console.WriteLine(deserialize.MachineName);

        // var client = new Client(9944);    //配置服务器端口
        // client.Start();     //开启服务器
    }
}