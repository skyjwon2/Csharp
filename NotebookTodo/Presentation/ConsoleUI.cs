using System;
using System.Linq;
using NotebookTodo.Core;

namespace NotebookTodo.Presentation
{
    public class ConsoleUI
    {
        private readonly ITodoService _todoService;

        public ConsoleUI(ITodoService todoService)
        {
            _todoService = todoService;
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Modular TodoList ===");
                Console.WriteLine("1. Add Todo");
                Console.WriteLine("2. List All Todos");
                Console.WriteLine("3. List Completed Todos");
                Console.WriteLine("4. List Incomplete Todos");
                Console.WriteLine("5. Complete Todo");
                Console.WriteLine("6. Delete Todo");
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");

                var input = Console.ReadLine();

                try
                {
                    switch (input)
                    {
                        case "1":
                            AddTodoAndShow();
                            break;
                        case "2":
                            ListTodosAndPause(_todoService.GetAllTodos(), "All Todos");
                            break;
                        case "3":
                            ListTodosAndPause(_todoService.GetCompletedTodos(), "Completed Todos");
                            break;
                        case "4":
                            ListTodosAndPause(_todoService.GetIncompleteTodos(), "Incomplete Todos");
                            break;
                        case "5":
                            CompleteTodoAndShow();
                            break;
                        case "6":
                            DeleteTodoAndShow();
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Invalid option. Press any key to try again.");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        private void AddTodoAndShow()
        {
            Console.Write("Enter Todo Title: ");
            var title = Console.ReadLine();
            if (title != null) 
            {
                _todoService.AddTodo(title);
                Console.WriteLine("Todo added successfully!");
            }
            Pause();
        }

        private void ListTodosAndPause(System.Collections.Generic.IEnumerable<TodoItem> items, string header)
        {
            Console.WriteLine($"\n--- {header} ---");
            if (!items.Any())
            {
                Console.WriteLine("No items found.");
            }
            else
            {
                foreach (var item in items)
                {
                    var status = item.IsCompleted ? "[X]" : "[ ]";
                    Console.WriteLine($"{item.Id}. {status} {item.Title} (Created: {item.CreatedAt})");
                }
            }
            Pause();
        }

        private void CompleteTodoAndShow()
        {
            Console.Write("Enter Todo ID to complete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                _todoService.CompleteTodo(id);
                Console.WriteLine("Todo marked as completed.");
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
            Pause();
        }

        private void DeleteTodoAndShow()
        {
            Console.Write("Enter Todo ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                _todoService.DeleteTodo(id);
                Console.WriteLine("Todo deleted successfully.");
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
            Pause();
        }

        private void Pause()
        {
            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }
    }
}
