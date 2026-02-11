
using System;
using System.Collections.Generic;
using System.Linq;

namespace AiTodo
{
    public class TaskRepository
    {
        private List<TodoItem> _tasks;
        private int _nextId;
        private TaskFileManager _fileManager;

        public TaskRepository()
        {
            _fileManager = new TaskFileManager();
            _tasks = _fileManager.LoadTasks();
            
            if (_tasks.Any())
            {
                _nextId = _tasks.Max(t => t.Id) + 1;
            }
            else
            {
                _nextId = 1;
            }
        }

        public void AddTask(string description)
        {
            var newTask = new TodoItem(_nextId++, description);
            _tasks.Add(newTask);
            _fileManager.SaveTasks(_tasks);
            Console.WriteLine($"할 일이 추가되었습니다: {newTask}");
        }

        public bool RemoveTask(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                _tasks.Remove(task);
                _fileManager.SaveTasks(_tasks);
                Console.WriteLine($"할 일이 삭제되었습니다: {task.Description}");
                return true;
            }
            Console.WriteLine($"ID가 {id}인 할 일을 찾을 수 없습니다.");
            return false;
        }

        public bool ModifyTask(int id, string newDescription)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                string oldDescription = task.Description;
                task.Description = newDescription;
                _fileManager.SaveTasks(_tasks);
                Console.WriteLine($"할 일이 수정되었습니다: {oldDescription} -> {newDescription}");
                return true;
            }
            Console.WriteLine($"ID가 {id}인 할 일을 찾을 수 없습니다.");
            return false;
        }

        public List<TodoItem> GetAllTasks()
        {
            return _tasks;
        }

        public bool ToggleComplete(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                task.IsCompleted = !task.IsCompleted;
                _fileManager.SaveTasks(_tasks);
                string status = task.IsCompleted ? "완료" : "미완료";
                Console.WriteLine($"할 일 상태가 변경되었습니다: {task.Description} ({status})");
                return true;
            }
            Console.WriteLine($"ID가 {id}인 할 일을 찾을 수 없습니다.");
            return false;
        }
    }
}
