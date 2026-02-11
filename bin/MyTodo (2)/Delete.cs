namespace AntiTodo;

public class Delete
{
    public static void DeleteTask(List<TodoTask> tasks)
    {
        Console.Write("삭제할 ID 입력: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var task = tasks.Find(t => t.Id == id);
            if (task != null)
            {
                tasks.Remove(task);
                Console.WriteLine($"{id}번 삭제 완료.");
            }
            else Console.WriteLine("해당 ID를 찾을 수 없습니다.");
        }
        else Console.WriteLine("숫자를 입력해주세요.");
    }
}