
using System;
using System.Linq;

namespace AiTodo
{
    public class TaskViewer
    {
        private TaskRepository _repository;

        public TaskViewer(TaskRepository repository)
        {
            _repository = repository;
        }

        public void ShowAllTasks()
        {
            var tasks = _repository.GetAllTasks();
            if (tasks.Count == 0)
            {
                Console.WriteLine("등록된 할 일이 없습니다.");
                return;
            }

            Console.WriteLine("\n--- 할 일 목록 ---");
            foreach (var task in tasks)
            {
                Console.WriteLine(task);
            }
            Console.WriteLine("------------------");
        }

        public void ToggleComplete()
        {
            Console.Write("상태를 변경할 할 일의 ID를 입력하세요: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                _repository.ToggleComplete(id);
            }
            else
            {
                Console.WriteLine("유효한 ID 형식이 아닙니다.");
            }
        }
    }
}
