using System;
namespace CyberBotGUI
{
    public class UserAction
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }

        public UserAction(string type, string description, DateTime? dueDate)
        {
            Type = type; Description = description; DueDate = dueDate;
        }
    }
}