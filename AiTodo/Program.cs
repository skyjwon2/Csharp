
using System;

namespace AiTodo
{
    class Program
    {
        static void Main(string[] args)
        {
            TaskRepository repository = new TaskRepository();
            TaskAdder adder = new TaskAdder(repository);
            TaskRemover remover = new TaskRemover(repository);
            TaskModifier modifier = new TaskModifier(repository);
            TaskViewer viewer = new TaskViewer(repository);

            while (true)
            {
                Console.WriteLine("\n=== AiTodo ===");
                Console.WriteLine("1. 할 일 추가");
                Console.WriteLine("2. 할 일 목록 확인");
                Console.WriteLine("3. 할 일 완료/미완료 처리");
                Console.WriteLine("4. 할 일 수정");
                Console.WriteLine("5. 할 일 삭제");
                Console.WriteLine("0. 종료");
                Console.Write("메뉴를 선택하세요: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        adder.AddTask();
                        break;
                    case "2":
                        viewer.ShowAllTasks();
                        break;
                    case "3":
                        viewer.ToggleComplete();
                        break;
                    case "4":
                        modifier.ModifyTask();
                        break;
                    case "5":
                        remover.RemoveTask();
                        break;
                    case "0":
                        Console.WriteLine("프로그램을 종료합니다.");
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다. 다시 선택해주세요.");
                        break;
                }
            }
        }
    }
}