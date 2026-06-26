using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberBotGUI
{
    public class CyberBot
    {
        public string UserName { get; set; }
        private Dictionary<string, List<string>> topicKeywords;

        public CyberBot()
        {
            topicKeywords = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                { "password", new List<string> { "password", "passcode", "strong password" }},
                { "phishing", new List<string> { "phishing", "scam", "fake email" }},
                { "privacy", new List<string> { "privacy", "data", "cookies" }}
            };
        }

        public string ProcessInput(string input, string sentiment)
        {
            string namePrefix = string.IsNullOrEmpty(UserName)? "" : $"{UserName}, ";
            string sentimentPrefix = sentiment == "worried"? "It's understandable to feel that way. " : "";

            foreach (var topic in topicKeywords)
            {
                if (topic.Value.Any(k => input.ToLower().Contains(k)))
                {
                    return sentimentPrefix + $"{namePrefix}Tip for {topic.Key} safety: Ask me 'tips' for details!";
                }
            }
            return $"I can help with tasks, quiz, or cybersecurity topics {namePrefix}Try 'add task' or 'start quiz'.";
        }
    }
}