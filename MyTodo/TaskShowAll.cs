namespace AntiTodo;
public class ShowAll
{
    public static void ShowAllTasks(List<TodoTask> tasks)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("할 일이 없습니다.");
            Console.WriteLine();
            return;
        }
        Console.WriteLine("<<할 일 목록>>");   
        Console.WriteLine();

        foreach (TodoTask task in tasks)
        {
            task.Display();
        }
        Console.WriteLine();
    }
}
