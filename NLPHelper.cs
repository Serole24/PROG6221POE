using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CyberBotGUI
{
    public static class NLPHelper
    {
        public static List<UserAction> ActionHistory = new List<UserAction>();

        private static Dictionary<string, List<string>> intentKeywords = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
        {
            { "add_task", new List<string> { "add task", "remind me", "set reminder", "remember to", "i need to" }},
            { "view_log", new List<string> { "show log", "activity log", "what have you done", "summary" }},
            { "show_more", new List<string> { "show more", "full log", "all actions" }},
            { "start_quiz", new List<string> { "quiz", "start quiz", "test me" }}
        };

        public static (string intent, string description, DateTime? dueDate) ProcessCommand(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput)) return ("unknown", "", null);
            string normalized = Regex.Replace(userInput.ToLower().Trim(), @"[^\w\s]", " ");

            foreach (var intent in intentKeywords)
                if (intent.Value.Any(k => normalized.Contains(k)))
                {
                    if (intent.Key == "add_task")
                    {
                        string desc = ExtractTaskDescription(userInput);
                        DateTime? date = ExtractDate(userInput);
                        return ("add_task", desc, date);
                    }
                    return (intent.Key, "", null);
                }
            return ("unknown", "", null);
        }

        private static string ExtractTaskDescription(string input)
        {
            string task = input;
            foreach (var list in intentKeywords.Values)
                foreach (string k in list) task = Regex.Replace(task, k, "", RegexOptions.IgnoreCase);
            task = Regex.Replace(task, @"\b(tomorrow|today|on|to|a|the)\b", "", RegexOptions.IgnoreCase).Trim();
            task = Regex.Replace(task, @"\s+", " ");
            return string.IsNullOrEmpty(task)? "New cybersecurity task" : char.ToUpper(task[0]) + task.Substring(1);
        }

        private static DateTime? ExtractDate(string input)
        {
            input = input.ToLower();
            if (input.Contains("tomorrow")) return DateTime.Today.AddDays(1);
            if (input.Contains("today")) return DateTime.Today;
            return null;
        }
    }
}