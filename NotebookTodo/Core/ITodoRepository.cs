using System.Collections.Generic;

namespace NotebookTodo.Core
{
    public interface ITodoRepository : IRepository<TodoItem>
    {
        // Add specific methods if needed, e.g., GetCompleted()
        // For now, standard repository methods + LINQ in service might suffice, 
        // but we can add specific queries here for performance if backed by DB.
        // Keeping it simple as per requirements.
    }
}
