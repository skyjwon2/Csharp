namespace AntiTodo;
public class Correct
{
    public static void CorrectTask(List<TodoTask> tasks)
    {
        Console.Write("수정할 일의 번호를 선택하세요: ");
        string? input = Console.ReadLine();

        if (string.IsNullOrEmpty(input) || !int.TryParse(input, out int id))
        {
            Console.WriteLine("유효한 번호를 입력해주세요.");
            Console.WriteLine();
            return;
        }

        TodoTask? task = tasks.Find(t => t.Id == id);

        if (task == null)
        {
            Console.WriteLine("해당 번호의 할 일을 찾을 수 없습니다.");
            Console.WriteLine();
            return;
        }

        Console.Write("바꿀 이름을 입력하세요: ");
        string? a = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(a)) 
        {
            task.Name = a; 
            
            Console.WriteLine("이름이 성공적으로 변경되었습니다.");
            Console.WriteLine();
        }

    }
}