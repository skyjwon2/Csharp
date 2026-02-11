
using System;

namespace AiTodo
{
    public class TaskAdder
    {
        private TaskRepository _repository;

        public TaskAdder(TaskRepository repository)
        {
            _repository = repository;
        }

        public void AddTask()
        {
            Console.Write("추가할 할 일 내용을 입력하세요: ");
            string description = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(description))
            {
                _repository.AddTask(description);
            }
            else
            {
                Console.WriteLine("내용이 비어있습니다.");
            }
        }
    }
}
