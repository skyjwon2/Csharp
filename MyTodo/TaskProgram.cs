using System;
using System.Collections.Generic;

namespace AntiTodo;

public class Program
{
    public static void Main(string[] args)
    {
        var (tasks, nextId) = SaveLoad.Load();

        while (true)
        {
            Console.WriteLine("<< 할 일 관리 프로그램 >>");
            Console.WriteLine("1. 추가 2. 목록 3. 완료/해제 4. 삭제 5. 변경 6. 종료");   
            Console.Write("메뉴 선택: ");
            string? input = Console.ReadLine();
            Console.WriteLine();

            var (updatedNextId, keepRunning) = MenuHandler.HandleMenu(input, tasks, nextId);
            nextId = updatedNextId;

            if (!keepRunning)
            {
                return;
            }

            SaveLoad.Save(tasks, nextId);
        }
    }
}