
using System;

namespace AiTodo
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        public TodoItem(int id, string description)
        {
            Id = id;
            Description = description;
            IsCompleted = false;
        }

        public override string ToString()
        {
            string status = IsCompleted ? "[V]" : "[ ]";
            return $"{Id}. {status} {Description}";
        }
    }
}
