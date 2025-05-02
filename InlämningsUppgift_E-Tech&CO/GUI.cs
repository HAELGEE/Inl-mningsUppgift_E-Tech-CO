using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO;

class GUI
{
    public static void DrawWindow(string header, int fromLeft, int fromTop, List<string> graphic)
    {
        string[] graphics = graphic.ToArray();
        int width = 0;
        for (int i = 0; i < graphics.Length; i++)
        {
            if (graphics[i] != null && graphics[i].Length > width)
            {
                width = graphics[i].Length;
            }
        }
        if (width < header.Length + 4)
        { width = header.Length + 4; }
        ;

        Console.SetCursorPosition(fromLeft, fromTop);
        if (header != "")
        {
            Console.Write('┌' + " ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(header);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" " + new String('─', width - header.Length) + '┐');
        }
        else
        {
            Console.Write('┌' + new String('─', width + 2) + '┐');
        }
        Console.WriteLine();
        int maxRows = 0;
        for (int j = 0; j < graphics.Length; j++)
        {
            Console.SetCursorPosition(fromLeft, fromTop + j + 1);
            Console.WriteLine('│' + " " + graphics[j] + new String(' ', width - graphics[j].Length + 1) + '│');
            maxRows = j;
        }
        Console.SetCursorPosition(fromLeft, fromTop + maxRows + 2);
        Console.Write('└' + new String('─', width + 2) + '┘');

    }
    public static void DrawWindowForShop(string header, int fromLeft, int fromTop, List<string> graphic)
    {
        string[] graphics = graphic.ToArray();
        int width = 0;
        for (int i = 0; i < graphics.Length; i++)
        {
            if (graphics[i] != null && graphics[i].Length > width)
            {
                width = graphics[i].Length;
            }
        }
        if (width < header.Length + 4)
        { width = header.Length + 4; }
        ;

        Console.SetCursorPosition(fromLeft, fromTop);
        if (header != "")
        {
            Console.Write('┌' + " ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(header);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" " + new String('─', width - header.Length) + '┐');
        }
        else
        {
            Console.Write('┌' + new String('─', width + 2) + '┐');
        }
        Console.WriteLine();
        int maxRows = 0;
        for (int j = 0; j < graphics.Length; j++)
        {
            Console.SetCursorPosition(fromLeft, fromTop + j + 1);
            Console.WriteLine('│' + " " + graphics[j] + new String(' ', width - graphics[j].Length + 1) + '│');
            maxRows = j;
        }
        Console.SetCursorPosition(fromLeft, fromTop + maxRows + 2);
        Console.Write('└' + new String('─', width + 2) + '┘');

    }
}

