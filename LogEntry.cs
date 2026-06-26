using System;
namespace CyberBotGUI
{
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string ActionType { get; set; }
        public string Description { get; set; }

        public LogEntry(string actionType, string description)
        {
            Timestamp = DateTime.Now;
            ActionType = actionType;
            Description = description;
        }
    }
}