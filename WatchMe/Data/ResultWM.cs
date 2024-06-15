namespace WatchMe;

/// <summary>
/// 通用返回值
/// </summary>
/// <typeparam name="T">泛型</typeparam>
public class ResultWM<T>
{
    private T _data;
    
    public ResultWM(T data)
    {
        this._data = data;
    }

    /// <summary>
    /// 根据传入类型动态的存储数据
    /// </summary>
    /// <param name="t">信息内容</param>
    /// <returns>通用返回值</returns>
    public static ResultWM<T> Data(T t)
    {
        return new ResultWM<T>(t);
    }
    /// <summary>
    /// 获取信息类别与信息内容
    /// </summary>
    /// <param name="data">源信息</param>
    /// <returns>字符串数组，0是类别，1是内容</returns>
    public static string[] GetDataType(string data)
    {
        var strings = data.Split("WMTYPE");
        return strings;
    }
    
    public T Data1
    {
        get => _data;
        set => _data = value;
    }
}