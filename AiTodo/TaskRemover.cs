
using System;

namespace AiTodo
{
    public class TaskRemover
    {
        private TaskRepository _repository;

        public TaskRemover(TaskRepository repository)
        {
            _repository = repository;
        }

        public void RemoveTask()
        {
            Console.Write("삭제할 할 일의 ID를 입력하세요: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                _repository.RemoveTask(id);
            }
            else
            {
                Console.WriteLine("유효한 ID 형식이 아닙니다.");
            }
        }
    }
}
