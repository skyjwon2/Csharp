using System.IO;
using System.Collections.Generic;

namespace MyTodo;

public class SaveLoad
{
    private const string Path = "tasks.txt";

    public static void Save(List<TodoTask> tasks, int nextId)
    {
        List<string> lines = new List<string>();
        lines.Add(nextId.ToString()); 

        foreach (var t in tasks)
        {
            lines.Add($"{t.Id}|{t.Name}|{t.IsCompleted}");
        }
        File.WriteAllLines(Path, lines);
    }

    public static (List<TodoTask>, int) Load()
    {
        if (!File.Exists(Path)) return (new List<TodoTask>(), 1);

        string[] lines = File.ReadAllLines(Path);
        if (lines.Length == 0) return (new List<TodoTask>(), 1);

        if (!int.TryParse(lines[0], out int nextId))
        {
            nextId = 1; 
        }
        List<TodoTask> tasks = new List<TodoTask>();

        for (int i = 1; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split('|');
            if (parts.Length < 3) continue;

            var task = new TodoTask(int.Parse(parts[0]), parts[1])
            {
                IsCompleted = bool.Parse(parts[2])
            };
            tasks.Add(task);
        }
        return (tasks, nextId);
    }
}