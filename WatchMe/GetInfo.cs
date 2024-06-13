using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using NickStrupat;

namespace WatchMe;

public class GetInfo
{
    private bool ShowEve;

    /// <summary>
    /// 有参，是否显示具体进程占用
    /// </summary>
    /// <param name="showEve"></param>
    public GetInfo(bool showEve)
    {
        ShowEve = showEve;
    }
    /// <summary>
    /// 填入系统环境到文件中
    /// </summary>
    public void SetInfo()
    {
         //控制台输出到文本中
        var streamWriter = new StreamWriter("DataMe.txt");
        Console.SetOut(streamWriter);
        
        var p = Process.GetProcesses(); //获取所以正在运行的进程信息

        double totalMem = 0;//总占用内存

        string info = "";//存储进程名称
        
        /*
         * 获取所有进程占用内存
         */
        foreach (Process pr in p)
        
        {
            totalMem += (double)pr.WorkingSet64 / 1024/1024;//自增总内存
            var prWorkingSet64 = ((double)pr.WorkingSet64 / (1024 *1024) );
            prWorkingSet64 = Math.Round(prWorkingSet64, 2);
            
            info += pr.ProcessName + "内存：——" + prWorkingSet64 + "MB\n"; //得到进程内存,三位小数(win是\r\n,lin是\n)

        }
        var computerInfo = new ComputerInfo();
        Console.WriteLine("本机使用操作系统:"+computerInfo.OSFullName);
        Console.WriteLine("本机物理内存:"+Math.Round((double)computerInfo.TotalPhysicalMemory/1024/1024,2)+"MB");
        Console.WriteLine("本机虚拟内存:"+Math.Round((double)computerInfo.TotalVirtualMemory/1024/1024,2)+"MB");
        Console.WriteLine("本机物理内存剩余:"+Math.Round((double)computerInfo.AvailablePhysicalMemory/1024/1024,2)+"MB");
        
        Console.WriteLine("总占用内存:" + Math.Round(totalMem,2) + "M");
        
        // Console.WriteLine("判断是否为Windows Linux OSX");
        //
        // Console.WriteLine($"Linux:{RuntimeInformation.IsOSPlatform(OSPlatform.Linux)}");
        //
        // Console.WriteLine($"OSX:{RuntimeInformation.IsOSPlatform(OSPlatform.OSX)}");
        //
        // Console.WriteLine($"Windows:{RuntimeInformation.IsOSPlatform(OSPlatform.Windows)}");

        Console.WriteLine($"系统架构：{RuntimeInformation.OSArchitecture}");

        // Console.WriteLine($"系统名称：{RuntimeInformation.OSDescription}");

        Console.WriteLine($"进程架构：{RuntimeInformation.ProcessArchitecture}");

        Console.WriteLine($"是否64位操作系统：{Environment.Is64BitOperatingSystem}");

        Console.WriteLine("CPU 核心:" + Environment.ProcessorCount);

        Console.WriteLine("主机名称:" + Environment.MachineName);

        // Console.WriteLine("Version:" + Environment.OSVersion);
        
        Console.WriteLine("当前程序使用内存:" + Math.Round((double)Environment.WorkingSet/1024/1024,2));
        if (ShowEve)
        {
            Console.WriteLine(info);//显示所有进程的字符串
        }

/*// Console.ReadLine();

//创建一个ProcessStartInfo对象 使用系统shell 指定命令和参数 设置标准输出

        var psi = new ProcessStartInfo("top", " -b -n 1") { RedirectStandardOutput = true };

//启动

        var proc = Process.Start(psi);

//   psi = new ProcessStartInfo("", "1") { RedirectStandardOutput = true };

//启动

// proc = Process.Start(psi);

        if (proc == null)

        {
            Console.WriteLine("Can not exec.");
        }

        else

        {
            Console.WriteLine("-------------Start read standard output-------cagy-------");

//开始读取

            using (var sr = proc.StandardOutput)

            {
                while (!sr.EndOfStream)

                {
                    Console.WriteLine(sr.ReadLine());
                }

                if (!proc.HasExited)

                {
                    proc.Kill();
                }
            }

            Console.WriteLine("---------------Read end-----------cagy-------");

            Console.WriteLine($"Total execute time :{(proc.ExitTime - proc.StartTime).TotalMilliseconds} ms");

            Console.WriteLine($"Exited Code ： {proc.ExitCode}");*/

            streamWriter.Flush();
            // streamWriter.Close();
    }
    
    public void Get()
    {
        var streamReader = new StreamReader("DataMe.txt",Encoding.UTF8);
        using (streamReader)
        {
            while (!streamReader.EndOfStream)
            {
                Console.WriteLine(streamReader.ReadLine());
            }
        }
    }
}