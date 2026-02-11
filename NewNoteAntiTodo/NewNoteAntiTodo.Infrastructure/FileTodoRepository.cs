using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using NewNoteAntiTodo.Core;

namespace NewNoteAntiTodo.Infrastructure
{
    // SOLID: SRP - Handles only file storage operations.
    public class FileTodoRepository : ITodoRepository
    {
        private const string FilePath = "todos.json";
        private readonly JsonSerializerOptions _jsonOptions;

        public FileTodoRepository()
        {
            _jsonOptions = new JsonSerializerOptions { WriteIndented = true };
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            if (!File.Exists(FilePath))
            {
                return new List<TodoItem>();
            }

            try
            {
                using var stream = File.OpenRead(FilePath);
                return await JsonSerializer.DeserializeAsync<List<TodoItem>>(stream) 
                       ?? new List<TodoItem>();
            }
            catch
            {
                return new List<TodoItem>();
            }
        }

        public async Task AddAsync(TodoItem todo)
        {
            var items = (await GetAllAsync()).ToList();
            
            // Auto-increment ID strategy
            int nextId = items.Any() ? items.Max(t => t.Id) + 1 : 1;
            todo.Id = nextId;
            
            items.Add(todo);
            await SaveToFileAsync(items);
        }

        public async Task UpdateAsync(TodoItem todo)
        {
            var items = (await GetAllAsync()).ToList();
            var existingItem = items.FirstOrDefault(i => i.Id == todo.Id);
            
            if (existingItem != null)
            {
                existingItem.Title = todo.Title;
                existingItem.IsCompleted = todo.IsCompleted;
                existingItem.DueDate = todo.DueDate;
                await SaveToFileAsync(items);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var items = (await GetAllAsync()).ToList();
            var itemToRemove = items.FirstOrDefault(i => i.Id == id);
            
            if (itemToRemove != null)
            {
                items.Remove(itemToRemove);
                await SaveToFileAsync(items);
            }
        }

        private async Task SaveToFileAsync(IEnumerable<TodoItem> items)
        {
            using var stream = File.Create(FilePath);
            await JsonSerializer.SerializeAsync(stream, items, _jsonOptions);
        }
    }
}
