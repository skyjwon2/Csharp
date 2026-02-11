using System.Collections.Generic;

namespace NotebookTodo.Core
{
    public interface ITodoService
    {
        void AddTodo(string title);
        IEnumerable<TodoItem> GetAllTodos();
        IEnumerable<TodoItem> GetCompletedTodos();
        IEnumerable<TodoItem> GetIncompleteTodos();
        void CompleteTodo(int id);
        void DeleteTodo(int id);
    }
}
