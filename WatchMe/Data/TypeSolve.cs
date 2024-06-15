namespace WatchMe;

public class TypeSolve
{
    /// <summary>
    /// 不同类型进行不同逻辑处理
    /// </summary>
    /// <param name="data"></param>
    /// <returns>处理后数据</returns>
    public static object? TypeGet(string[] data)
    {
        return data[0] switch
        {
            "string" => data[1],
            "GetInfo" => data[1],
            _ => null
        };
    }
}