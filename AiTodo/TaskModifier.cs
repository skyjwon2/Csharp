
using System;

namespace AiTodo
{
    public class TaskModifier
    {
        private TaskRepository _repository;

        public TaskModifier(TaskRepository repository)
        {
            _repository = repository;
        }

        public void ModifyTask()
        {
            Console.Write("수정할 할 일의 ID를 입력하세요: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Console.Write("새로운 내용을 입력하세요: ");
                string newDescription = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newDescription))
                {
                    _repository.ModifyTask(id, newDescription);
                }
                else
                {
                    Console.WriteLine("내용이 비어있습니다.");
                }
            }
            else
            {
                Console.WriteLine("유효한 ID 형식이 아닙니다.");
            }
        }
    }
}
