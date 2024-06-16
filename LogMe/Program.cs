namespace LogMe
{
    public class LogMe
    {
        /// <summary>
        /// 严重
        /// </summary>
        /// <param name="data"></param>
        public static void Error(string data)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(data);
            Console.ResetColor();
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="data"></param>
        public static void Warning(string data)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(data);
            Console.ResetColor();
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="data"></param>
        public static void Success(string data)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(data);
            Console.ResetColor();
        }

        /// <summary>
        /// 接收
        /// </summary>
        /// <param name="data"></param>
        public static void Receive(string data)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(data);
            Console.ResetColor();
        }

        /// <summary>
        /// 普通结果
        /// </summary>
        /// <param name="data"></param>
        public static void Result(string data)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(data);
            Console.ResetColor();
        }
    }
}