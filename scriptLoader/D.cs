using System;

namespace scriptLoader
{
    public class D
    {
        public static void Log(string msg)
        {
            Log(msg, "");
        }

        public static void Log(object msg)
        {
            if (msg == null) Log("NULL", "");
            else Log(msg.ToString(), "");
        }

        public static void Log(string msg, params object[] p)
        {
            if (msg == null || p == null) Console.WriteLine("NULL");
            else Console.WriteLine(msg, p);
        }
    }
}