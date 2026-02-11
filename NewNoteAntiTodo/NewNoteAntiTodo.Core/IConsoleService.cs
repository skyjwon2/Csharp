namespace NewNoteAntiTodo.Core
{
    // SOLID: DIP - Application will depend on this interface instead of System.Console directly.
    // This allows mocking for unit testing.
    public interface IConsoleService
    {
        void WriteLine(string message);
        void Write(string message);
        string ReadLine();
        void Clear();
    }
}
