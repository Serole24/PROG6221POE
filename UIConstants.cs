using System;
using System.Threading;

namespace CyberSecurityBot
{
    public static class UIConstants
    {
        // Console colors
        public static readonly ConsoleColor Cyan = ConsoleColor.Cyan;
        public static readonly ConsoleColor Green = ConsoleColor.Green;
        public static readonly ConsoleColor Yellow = ConsoleColor.Yellow;
        public static readonly ConsoleColor Red = ConsoleColor.Red;
        public static readonly ConsoleColor White = ConsoleColor.White;

        // Reset color helper
        public static string Reset => "";

        // Section headers and dividers - Task 6
        public static string Divider => "═══════════════";
        public static string HeaderBorder => "╔═══════════════════════════════════════╗";
        public static string FooterBorder => "╚═══════════════╝";

        // Typing effect to simulate conversation - Task 6
        public static void TypePrint(string text, int delayMs = 30)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delayMs);
            }
            Console.WriteLine();
        }

        // ASCII logo with borders
        public static void DisplayLogo()
        {
            Console.ForegroundColor = Cyan;
            Console.WriteLine(HeaderBorder);
            Console.WriteLine("║ ██████╗██╗ ██╗██████╗ ███████╗██████╗ ║");
            Console.WriteLine("║ ██╔════╝██║ ██║██╔══██╗██╔════╝██╔══██╗ ║");
            Console.WriteLine("║ ██║ ██║ ██║██████╔╝█████╗ ██████╔╝ ║");
            Console.WriteLine("║ ██║ ██║ ██║██╔══██╗██╔══╝ ██╔══██╗ ║");
            Console.WriteEquals("║ ╚██████╗╚██████╔╝██████╔╝███████╗██║ ██║ ║");
            Console.WriteLine("║ AWARENESS BOT - STAY SAFE ONLINE ║");
            Console.WriteLine(FooterBorder + "\n");
            Console.ResetColor();
        }
    }
}