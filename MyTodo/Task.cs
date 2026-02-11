
namespace AntiTodo;
public class TodoTask
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsCompleted { get; set; }

    public TodoTask(int id, string name)
    {
        Id = id;
        Name = name;
        IsCompleted = false;
    }

    public void Complete()
    {
        IsCompleted = true;
    }
    public void Display()
    {
        Console.WriteLine($" {(IsCompleted ? "[V]" : "[ ]")} {Id}. {Name}");
    }

    public void Toggle()
    {
        IsCompleted = !IsCompleted;
    }

}