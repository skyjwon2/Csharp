using System;
using System.Linq;
using System.Threading.Tasks;
using NewNoteAntiTodo.Core;

namespace NewNoteAntiTodo.Presentation
{
    // SOLID: SRP - Connects User IO (via IConsoleService) and Data (via ITodoRepository).
    // SOLID: DIP - Depends on abstractions, not concrete FileRepository or System.Console.
    public class TodoApp
    {
        private readonly ITodoRepository _repository;
        private readonly IConsoleService _console;

        // Constructor Injection
        public TodoApp(ITodoRepository repository, IConsoleService console)
        {
            _repository = repository;
            _console = console;
        }

        public async Task RunAsync()
        {
            bool exit = false;
            while (!exit)
            {
                _console.Clear();
                _console.WriteLine("=== Clean Architecture Todo App ===");
                _console.WriteLine("1. List Todos");
                _console.WriteLine("2. Add Todo");
                _console.WriteLine("3. Mark as Completed");
                _console.WriteLine("4. Delete Todo");
                _console.WriteLine("5. Exit");
                _console.Write("Select an option: ");

                var input = _console.ReadLine();
                switch (input)
                {
                    case "1":
                        await ListTodosAsync();
                        break;
                    case "2":
                        await AddTodoAsync();
                        break;
                    case "3":
                        await MarkAsCompletedAsync();
                        break;
                    case "4":
                        await DeleteTodoAsync();
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:
                        _console.WriteLine("Invalid option. Press any key...");
                        _console.ReadLine();
                        break;
                }
            }
        }

        private async Task ListTodosAsync()
        {
            _console.WriteLine("\n--- Todo List ---");
            var todos = await _repository.GetAllAsync();
            
            if (!todos.Any())
            {
                _console.WriteLine("No items found.");
            }
            else
            {
                foreach (var todo in todos)
                {
                    _console.WriteLine(todo.ToString());
                }
            }
            
            Pause();
        }

        private async Task AddTodoAsync()
        {
            _console.WriteLine("\n--- Add Todo ---");
            _console.Write("Enter Title: ");
            string title = _console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(title))
            {
                 _console.WriteLine("Title cannot be empty.");
                 Pause();
                 return;
            }

            var todo = new TodoItem 
            { 
                Title = title, 
                IsCompleted = false,
                DueDate = DateTime.Now.AddDays(1) // Default due date
            };

            await _repository.AddAsync(todo);
            _console.WriteLine("Todo added successfully.");
            Pause();
        }

        private async Task MarkAsCompletedAsync()
        {
            await ListTodosAsyncOnlyView();
            _console.Write("\nEnter ID to completion: ");
            if (int.TryParse(_console.ReadLine(), out int id))
            {
                var todos = await _repository.GetAllAsync();
                var todo = todos.FirstOrDefault(t => t.Id == id);
                if (todo != null)
                {
                    todo.IsCompleted = true;
                    await _repository.UpdateAsync(todo);
                    _console.WriteLine("Updated successfully.");
                }
                else
                {
                    _console.WriteLine("Todo not found.");
                }
            }
            else
            {
                 _console.WriteLine("Invalid ID.");
            }
            Pause();
        }

        private async Task DeleteTodoAsync()
        {
             await ListTodosAsyncOnlyView();
            _console.Write("\nEnter ID to delete: ");
            if (int.TryParse(_console.ReadLine(), out int id))
            {
                await _repository.DeleteAsync(id);
                _console.WriteLine("Deleted successfully.");
            }
            else
            {
                 _console.WriteLine("Invalid ID.");
            }
            Pause();
        }

        // Helper to list without pause, for selection
        private async Task ListTodosAsyncOnlyView()
        {
            var todos = await _repository.GetAllAsync();
             foreach (var todo in todos)
            {
                _console.WriteLine(todo.ToString());
            }
        }

        private void Pause()
        {
            _console.WriteLine("\nPress Enter to return to menu...");
            _console.ReadLine();
        }
    }
}
