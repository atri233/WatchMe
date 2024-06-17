using System.Diagnostics;
using System.Runtime.InteropServices;
using NickStrupat;

namespace WatchMe;

public class GetInfo
{
    private string Name;
    private bool showEve; //是否显示每个进程的具体占用
    private string systemArchitecture; //系统架构
    private string operatingSystem; //操作系统
    public double totalPhysicalMemory; //总物理内存
    private double totalVirtualMemory; //总虚拟内存
    private double availablePhysicalMemory; //剩余物理内存
    private double totalMem; //总占用内存
    private bool is_64; //是否为64位操作系统
    private int processorCount; //cpu核心数
    private string machineName; //主机名称
    private double workingSet; //本应用占用内存
    private string? info; //每个进程的具体占用

    
    /// <summary>
    /// 有参，是否显示具体占用
    /// </summary>
    /// <param name="showEve"></param>
    public GetInfo(bool showEve)
    {
        ShowEve = showEve;
        SetInfo();
    }

    /// <summary>
    /// 填入系统环境到属性中
    /// </summary>
    public void SetInfo()
    {
        try
        {
            Name = "GetInfo";
            var p = Process.GetProcesses(); //获取所以正在运行的进程信息

            totalMem = 0; //总占用内存
            /*
             * 获取所有进程占用内存
             */
            foreach (var pr in p)
            {
                totalMem += (double)pr.WorkingSet64 / 1024 / 1024; //自增总内存
                var prWorkingSet64 = ((double)pr.WorkingSet64 / (1024 * 1024));
                prWorkingSet64 = Math.Round(prWorkingSet64, 2);
                if (ShowEve)
                {
                    info += pr.ProcessName + "内存:" + prWorkingSet64 + "MB"; //得到进程内存,三位小数(win是\r\n,lin是\n)
                }
            }

            var computerInfo = new ComputerInfo(); //创建查询环境实例

            operatingSystem = computerInfo.OSFullName;

            totalPhysicalMemory = Math.Round((double)computerInfo.TotalPhysicalMemory / 1024 / 1024, 2);

            totalVirtualMemory = Math.Round((double)computerInfo.TotalVirtualMemory / 1024 / 1024, 2);

            availablePhysicalMemory = Math.Round((double)computerInfo.AvailablePhysicalMemory / 1024 / 1024, 2);

            totalMem = Math.Round(totalMem, 2);

            systemArchitecture = RuntimeInformation.OSArchitecture.ToString();

            is_64 = Environment.Is64BitOperatingSystem;

            processorCount = Environment.ProcessorCount;

            machineName = Environment.MachineName;

            workingSet = Math.Round((double)Environment.WorkingSet / 1024 / 1024, 2);
        }
        catch (Exception e)
        {
            LogSelf.Error("获取系统环境出错：\n" + e);
            throw;
        }
    }

    public string Name1
    {
        get => Name;
        set => Name = value ?? throw new ArgumentNullException(nameof(value));
    }

    public bool ShowEve
    {
        get => showEve;
        set => showEve = value;
    }

    public string SystemArchitecture
    {
        get => systemArchitecture;
        set => systemArchitecture = value;
    }

    public string OperatingSystem
    {
        get => operatingSystem;
        set => operatingSystem = value ?? throw new ArgumentNullException(nameof(value));
    }

    public double TotalPhysicalMemory
    {
        get => totalPhysicalMemory;
        set => totalPhysicalMemory = value;
    }

    public double TotalVirtualMemory
    {
        get => totalVirtualMemory;
        set => totalVirtualMemory = value;
    }

    public double AvailablePhysicalMemory
    {
        get => availablePhysicalMemory;
        set => availablePhysicalMemory = value;
    }

    public double TotalMem
    {
        get => totalMem;
        set => totalMem = value;
    }

    public bool Is64
    {
        get => is_64;
        set => is_64 = value;
    }

    public int ProcessorCount
    {
        get => processorCount;
        set => processorCount = value;
    }

    public string MachineName
    {
        get => machineName;
        set => machineName = value ?? throw new ArgumentNullException(nameof(value));
    }

    public double WorkingSet
    {
        get => workingSet;
        set => workingSet = value;
    }

    public string? Info
    {
        get => info;
        set => info = value;
    }
}