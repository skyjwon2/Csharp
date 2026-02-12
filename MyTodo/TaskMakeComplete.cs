namespace MyTodo;
public class MakeComplete
{
    public static void MakeCompleteTask(List<TodoTask> tasks)
    {
        Console.Write("완료 혹은 해제 할 할 일의 번호를 입력하세요: ");
        string? input = Console.ReadLine();

        if (string.IsNullOrEmpty(input) || !int.TryParse(input, out int id))
        {
            Console.WriteLine("유효한 번호를 입력해주세요.");
            return;
        }

        TodoTask? task = tasks.Find(t => t.Id == id);
        if (task == null)
        {
            Console.WriteLine("해당 번호의 할 일을 찾을 수 없습니다.");
            return;
        }

        task.Toggle();
        
        if (task.IsCompleted)
        {
            Console.WriteLine("할 일이 완료되었습니다.");
        }
        else
        {
            Console.WriteLine("체크가 해제되었습니다.");
        }
    }
}