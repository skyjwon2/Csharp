using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewNoteAntiTodo.Core;
using NewNoteAntiTodo.Infrastructure;

namespace NewNoteAntiTodo.Presentation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // 1. Create a Host Application Builder
            var builder = Host.CreateApplicationBuilder(args);

            // 2. Register Services (Dependency Injection Container)
            // Application Layer
            builder.Services.AddSingleton<TodoApp>();

            // Infrastructure Layer (Mapping Interfaces to Concrete Implementations)
            // SOLID: DIP is achieved here. The implementation details are injected.
            builder.Services.AddSingleton<ITodoRepository, FileTodoRepository>();
            builder.Services.AddSingleton<IConsoleService, ConsoleService>();

            // 3. Build the Host
            using IHost host = builder.Build();

            // 4. Resolve the Main App Entry point and run
            // Create a scope if needed, but for a simple console app, accessing Singleton is fine.
            var app = host.Services.GetRequiredService<TodoApp>();
            
            try 
            {
                await app.RunAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An critical error occurred: {ex.Message}");
            }
        }
    }
}
