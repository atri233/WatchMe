using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace WatchMe.application;

public class JsonOptions
{
    /// <summary>
    /// json序列化配置
    /// </summary>
    /// <returns></returns>
    public static JsonSerializerOptions Option()
    {
        return new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), //不转义所有字符（防止信息错乱）
            WriteIndented = true //美化打印效果
        };
    }
}