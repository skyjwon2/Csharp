using System;
using System.Collections.Generic;

namespace AntiTodo;

public class MenuHandler
{
    public static (int, bool) HandleMenu(string? input, List<TodoTask> tasks, int nextId)
    {
        switch (input)
        {
            case "1":
                nextId = Add.AddTask(tasks, nextId);
                break;
            case "2":
                ShowAll.ShowAllTasks(tasks);
                break;
            case "3":
                MakeComplete.MakeCompleteTask(tasks);
                break;
            case "4":
                Delete.DeleteTask(tasks);
                break;
            case "5":
                Correct.CorrectTask(tasks);
                break;
            case "6":
                SaveLoad.Save(tasks, nextId);
                Console.WriteLine("데이터를 저장하고 종료합니다.");
                return (nextId, false); // false means stop running
            default:
                Console.WriteLine("잘못된 입력입니다.");
                break;
        }

        return (nextId, true); // true means keep running
    }
}
