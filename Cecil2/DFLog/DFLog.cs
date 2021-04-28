using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum E_ColorType
{
    Cyan,
    Green,
    Blue,
    Red,
    Magenta,
    Yellow,
    White,
    Black,
    Gray,
    DarkCyan,
    DarkGreen,
    DarkBlue,
    DarkRed,
    DarkMagenta,
    DarkYellow,
}

public class DFLog
{

    public static void Log(object obj , E_ColorType col= E_ColorType.Gray)
    {
        Console.ForegroundColor =(ConsoleColor) Enum.Parse(typeof(ConsoleColor), col.ToString());
        Console.WriteLine(obj);
        Console.ForegroundColor = ConsoleColor.Gray;
    }

    public static void LogLine(E_ColorType col= E_ColorType.Gray)
    {
        Console.ForegroundColor =(ConsoleColor) Enum.Parse(typeof(ConsoleColor), col.ToString());
        Console.WriteLine("---------------------------------");
        Console.ForegroundColor = ConsoleColor.Gray;
    }
}
