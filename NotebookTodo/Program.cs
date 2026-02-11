using System;
using Microsoft.Extensions.DependencyInjection;
using NotebookTodo.Core;
using NotebookTodo.Infrastructure;
using NotebookTodo.Application;
using NotebookTodo.Presentation;

namespace NotebookTodo
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Setup DI Container
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // 2. Resolve ConsoleUI and Run
            try
            {
                var app = serviceProvider.GetRequiredService<ConsoleUI>();
                app.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Critical Error: {ex.Message}");
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Register Core/Infrastructure/Application services
            
            // Core & Infrastructure (Repository)
            services.AddSingleton<ITodoRepository, InMemoryTodoRepository>();

            // Application (Service)
            services.AddScoped<ITodoService, TodoService>();

            // Presentation (UI)
            services.AddTransient<ConsoleUI>();
        }
    }
}
