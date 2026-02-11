using System;
using System.Collections.Generic;
using System.Linq;
using NotebookTodo.Core;

namespace NotebookTodo.Infrastructure
{
    public class InMemoryTodoRepository : ITodoRepository
    {
        private readonly List<TodoItem> _items = new List<TodoItem>();
        private int _nextId = 1;

        public IEnumerable<TodoItem> GetAll()
        {
            return _items;
        }

        public TodoItem? GetById(int id)
        {
            return _items.FirstOrDefault(t => t.Id == id);
        }

        public void Add(TodoItem entity)
        {
            entity.Id = _nextId++;
            _items.Add(entity);
        }

        public void Update(TodoItem entity)
        {
            var existing = GetById(entity.Id);
            if (existing != null)
            {
                // In a real DB, update logic. 
                // In memory, object reference might be same, but let's be explicit.
                existing.Title = entity.Title;
                existing.IsCompleted = entity.IsCompleted;
            }
        }

        public void Delete(int id)
        {
            var item = GetById(id);
            if (item != null)
            {
                _items.Remove(item);
            }
        }
    }
}
