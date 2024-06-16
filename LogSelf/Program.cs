namespace WatchMe;

public static class LogSelf
{
    /// <summary>
    /// 严重
    /// </summary>
    public static void Error(string data)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(data);
        Console.ResetColor();
    }

    /// <summary>
    /// 警告
    /// </summary>
    public static void Warning(string data)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(data);
        Console.ResetColor();
    }
    /// <summary>
    /// 成功
    /// </summary>
    public static void Success(string data)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(data);
        Console.ResetColor();
    }
    /// <summary>
    /// 接收
    /// </summary>
    public static void Receive(string data)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(data);
        Console.ResetColor();
    }
    /// <summary>
    /// 普通结果
    /// </summary>
    public static void Result(string data)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(data);
        Console.ResetColor();
    }
    
}