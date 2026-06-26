using System;
namespace CyberBotGUI
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReminderDate { get; set; }
        public bool IsCompleted { get; set; }

        public TaskItem(int id, string title, string description, DateTime reminderDate)
        {
            Id = id; Title = title; Description = description; ReminderDate = reminderDate;
        }

        public override string ToString()
        {
            string status = IsCompleted? "✓ " : "○ ";
            string reminder = ReminderDate == DateTime.MinValue? "" : $" | {ReminderDate:dd MMM}";
            return $"{status}{Title}{reminder}";
        }
    }
}