
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace AiTodo
{
    public class TaskFileManager
    {
        private const string FileName = "tasks.json";

        public void SaveTasks(List<TodoItem> tasks)
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FileName, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"파일 저장 중 오류가 발생했습니다: {ex.Message}");
            }
        }

        public List<TodoItem> LoadTasks()
        {
            if (!File.Exists(FileName))
            {
                return new List<TodoItem>();
            }

            try
            {
                string jsonString = File.ReadAllText(FileName);
                return JsonSerializer.Deserialize<List<TodoItem>>(jsonString) ?? new List<TodoItem>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"파일 불러오기 중 오류가 발생했습니다: {ex.Message}");
                return new List<TodoItem>();
            }
        }
    }
}
