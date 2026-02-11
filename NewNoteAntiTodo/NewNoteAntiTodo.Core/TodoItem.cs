using System;

namespace NewNoteAntiTodo.Core
{
    // SOLID: SRP - This class only holds data structure for Todo items.
    public class TodoItem
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? DueDate { get; set; }
        
        public override string ToString()
        {
            return $"[{ (IsCompleted ? "X" : " ") }] {Id}: {Title} {(DueDate.HasValue ? $"(Due: {DueDate.Value:yyyy-MM-dd})" : "")}";
        }
    }
}
