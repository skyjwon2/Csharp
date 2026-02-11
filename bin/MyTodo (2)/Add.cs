namespace AntiTodo;
public class Add
{
    public static int AddTask(List<TodoTask> tasks, int nextId)
    {
        Console.Write("할 일 입력: ");
        string? name = Console.ReadLine();
        int id = nextId;

        if (string.IsNullOrEmpty(name))
        {
            Console.WriteLine("할 일을 입력해주세요.");
            Console.WriteLine();
            return nextId;
        }

        if (tasks.Find(t => t.Name == name) != null)
        {
            Console.WriteLine("이미 존재하는 할 일입니다.");
            Console.WriteLine();
            return nextId;
        }

        tasks.Add(new TodoTask(id, name));
        Console.WriteLine("할 일이 추가되었습니다.");
        Console.WriteLine();
        return nextId + 1;
    }
}