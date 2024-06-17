using System.Buffers;
using System.Net.Sockets;
using System.Text;
using WatchMe;

namespace WatchMeUser;

class Program
{
    static string Head = "WMTYPE"; //头信息

    static void Main(string[] args)
    {
        var client = new Client("127.0.0.1",9944);
        
    }
}