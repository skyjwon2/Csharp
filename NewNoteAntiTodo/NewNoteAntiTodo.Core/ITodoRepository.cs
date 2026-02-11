using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewNoteAntiTodo.Core
{
    // SOLID: ISP - Interface is specific to Todo data operations.
    // SOLID: DIP - High-level modules will depend on this abstraction, not implementation.
    public interface ITodoRepository
    {
        Task<IEnumerable<TodoItem>> GetAllAsync();
        Task AddAsync(TodoItem todo);
        Task UpdateAsync(TodoItem todo);
        Task DeleteAsync(int id);
    }
}
