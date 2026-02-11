using System;
using NewNoteAntiTodo.Core;

namespace NewNoteAntiTodo.Infrastructure
{
    // SOLID: SRP - Sole implementation detail of wrapping System.Console.
    public class ConsoleService : IConsoleService
    {
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public void Write(string message)
        {
            Console.Write(message);
        }

        public string ReadLine()
        {
            return Console.ReadLine() ?? string.Empty;
        }

        public void Clear()
        {
            Console.Clear();
        }
    }
}
