using System;
using System.Collections.Generic;
using System.Linq;
using NotebookTodo.Core;

namespace NotebookTodo.Application
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;

        // Constructor Injection
        public TodoService(ITodoRepository repository)
        {
            _repository = repository;
        }

        public void AddTodo(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be empty.", nameof(title));
            }

            var newItem = new TodoItem
            {
                Title = title,
                IsCompleted = false
            };
            _repository.Add(newItem);
        }

        public IEnumerable<TodoItem> GetAllTodos()
        {
            return _repository.GetAll();
        }

        public IEnumerable<TodoItem> GetCompletedTodos()
        {
            return _repository.GetAll().Where(t => t.IsCompleted);
        }

        public IEnumerable<TodoItem> GetIncompleteTodos()
        {
            return _repository.GetAll().Where(t => !t.IsCompleted);
        }

        public void CompleteTodo(int id)
        {
            try
            {
                var item = _repository.GetById(id);
                if (item == null)
                {
                    throw new KeyNotFoundException($"Todo item with ID {id} not found.");
                }

                item.IsCompleted = true;
                _repository.Update(item);
            }
            catch
            {
                // Log error if logging was available
                throw; // Re-throw preserving stack trace
            }
        }

        public void DeleteTodo(int id)
        {
            try
            {
                var item = _repository.GetById(id);
                if (item == null)
                {
                    throw new KeyNotFoundException($"Todo item with ID {id} not found.");
                }
                 _repository.Delete(id);
            }
            catch
            {
                throw;
            }
        }
    }
}
