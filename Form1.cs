using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Speech.Synthesis;
using System.Windows.Forms;

namespace CyberBotGUI
{
    public partial class Form1 : Form
    {
        private CyberBot bot;
        private SpeechSynthesizer synth;
        private List<string> activityLog; // Task 4: Activity Log
        private string logFilePath = "activity_log.txt";
        private List<QuizQuestion> quizQuestions; // Task 2: Mini game
        private int quizScore = 0;
        private int quizIndex = 0;

        public Form1()
        {
            InitializeComponent();
            bot = new CyberBot();
            synth = new SpeechSynthesizer();
            activityLog = new List<string>();
            
            this.Text = "CyberSecurity Awareness Bot - Part 3";
            this.BackColor = Color.FromArgb(30, 30, 40);
            this.Size = new Size(900, 650);

            SetupUI();
            LoadQuizQuestions(); // Task 2
            LoadActivityLog(); // Task 4
            Form1_Load();
        }

        private void Form1_Load()
        {
            string greeting = "Hello! Welcome to the Cybersecurity Awareness Bot. What's your name?";
            AppendChat("Bot", greeting);
            synth.SpeakAsync(greeting);
            LogActivity("Bot started and greeted user");
        }

        private void SetupUI()
        {
            // Main chat area
            RichTextBox txtChat = new RichTextBox { Name = "txtChat", Location = new Point(20, 120), Size = new Size(550, 350), ReadOnly = true, BackColor = Color.FromArgb(45, 45, 60), ForeColor = Color.White, Font = new Font("Segoe UI", 10) };
            this.Controls.Add(txtChat);

            // Input + Send
            TextBox txtInput = new TextBox { Name = "txtInput", Location = new Point(20, 480), Size = new Size(450, 35), Font = new Font("Segoe UI", 11), BackColor = Color.FromArgb(60, 60, 75), ForeColor = Color.White };
            txtInput.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; ProcessInput(); } };
            this.Controls.Add(txtInput);

            Button btnSend = new Button { Name = "btnSend", Text = "Send", Location = new Point(480, 480), Size = new Size(90, 35), BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnSend.Click += (s, e) => ProcessInput();
            this.Controls.Add(btnSend);

            // Task 1: Task Assistant Panel
            GroupBox grpTasks = new GroupBox { Text = "Task Assistant", Location = new Point(590, 120), Size = new Size(270, 150), ForeColor = Color.Cyan, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            Button btnCheckPassword = new Button { Text = "Check Password Strength", Location = new Point(15, 30), Size = new Size(240, 30) };
            btnCheckPassword.Click += (s, e) => AppendChat("Task", "Enter a password in chat and type 'check password' to test strength");
            Button btnSecurityChecklist = new Button { Text = "Security Checklist", Location = new Point(15, 70), Size = new Size(240, 30) };
            btnSecurityChecklist.Click += (s, e) => ShowSecurityChecklist();
            grpTasks.Controls.Add(btnCheckPassword);
            grpTasks.Controls.Add(btnSecurityChecklist);
            this.Controls.Add(grpTasks);

            // Task 2: Mini Game Panel
            GroupBox grpQuiz = new GroupBox { Text = "Cybersecurity Quiz", Location = new Point(590, 280), Size = new Size(270, 150), ForeColor = Color.Yellow, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            Button btnStartQuiz = new Button { Name = "btnStartQuiz", Text = "Start Quiz", Location = new Point(15, 30), Size = new Size(240, 30) };
            btnStartQuiz.Click += (s, e) => StartQuiz();
            Label lblScore = new Label { Name = "lblScore", Text = "Score: 0/0", Location = new Point(15, 70), Size = new Size(240, 30), ForeColor = Color.White };
            grpQuiz.Controls.Add(btnStartQuiz);
            grpQuiz.Controls.Add(lblScore);
            this.Controls.Add(grpQuiz);

            // Task 4: Activity Log Panel
            GroupBox grpLog = new GroupBox { Text = "Activity Log", Location = new Point(590, 440), Size = new Size(270, 100), ForeColor = Color.LightGreen, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            Button btnViewLog = new Button { Text = "View Log", Location = new Point(15, 30), Size = new Size(110, 30) };
            btnViewLog.Click += (s, e) => ViewActivityLog();
            Button btnClearLog = new Button { Text = "Clear Log", Location = new Point(145, 30), Size = new Size(110, 30) };
            btnClearLog.Click += (s, e) => ClearActivityLog();
            grpLog.Controls.Add(btnViewLog);
            grpLog.Controls.Add(btnClearLog);
            this.Controls.Add(grpLog);

            // Logo
            PictureBox picLogo = new PictureBox { Location = new Point(20, 20), Size = new Size(840, 80), SizeMode = PictureBoxSizeMode.Zoom, BackColor = Color.FromArgb(20, 20, 30) };
            Label lblLogo = new Label { Text = "CYBERSECURITY AWARENESS BOT", Location = new Point(20, 35), Size = new Size(840, 50), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Color.Cyan, Font = new Font("Consolas", 18, FontStyle.Bold) };
            this.Controls.Add(picLogo);
            this.Controls.Add(lblLogo);
        }

        private void ProcessInput()
        {
            TextBox txtInput = this.Controls["txtInput"] as TextBox;
            string userInput = txtInput.Text.Trim();
            if (string.IsNullOrEmpty(userInput)) return;

            AppendChat("You", userInput);
            LogActivity($"User: {userInput}");
            txtInput.Clear();

            try
            {
                // Task 3: NLP Simulation - better intent detection
                string intent = DetectIntent(userInput);
                string sentiment = DetectSentiment(userInput);

                if (string.IsNullOrEmpty(bot.UserName))
                {
                    bot.UserName = char.ToUpper(userInput[0]) + userInput.Substring(1).ToLower();
                    string welcome = $"Nice to meet you, {bot.UserName}! I can help with password safety, phishing, and privacy.";
                    AppendChat("Bot", welcome);
                    synth.SpeakAsync(welcome);
                    LogActivity("User name captured");
                    return;
                }

                string response = bot.GenerateResponse(userInput, sentiment, intent); // Pass intent for NLP
                AppendChat("Bot", response);
                synth.SpeakAsync(response);
                LogActivity($"Bot: {response}");
            }
            catch (Exception ex)
            {
                AppendChat("Bot", "Oops, something went wrong. Please try rephrasing.");
                LogActivity($"Error: {ex.Message}");
            }
        }

        // Task 3: NLP Simulation - detect intent beyond keywords
        private string DetectIntent(string input)
        {
            input = input.ToLower();
            if (input.Contains("check") && input.Contains("password")) return "check_password";
            if (input.Contains("quiz") || input.Contains("game") || input.Contains("test")) return "start_quiz";
            if (input.Contains("log") || input.Contains("history")) return "view_log";
            if (input.Contains("remember") || input.Contains("favorite")) return "save_memory";
            return "general_chat";
        }

        private string DetectSentiment(string input)
        {
            input = input.ToLower();
            if (input.Contains("worried") || input.Contains("scared") || input.Contains("anxious")) return "worried";
            if (input.Contains("frustrated") || input.Contains("confused") || input.Contains("overwhelmed")) return "frustrated";
            if (input.Contains("curious") || input.Contains("interested")) return "curious";
            return "neutral";
        }

        // Task 1: Task Assistant - Security Checklist
        private void ShowSecurityChecklist()
        {
            string checklist = "Cybersecurity Checklist:\n1. Use unique passwords\n2. Enable 2FA\n3. Update software\n4. Backup data\n5. Check privacy settings";
            AppendChat("Task Assistant", checklist);
            LogActivity("Security checklist displayed");
        }

        // Task 2: Mini Game - Quiz
        private void LoadQuizQuestions()
        {
            quizQuestions = new List<QuizQuestion>
            {
                new QuizQuestion("What makes a strong password?", new[] {"123456", "Password1", "7x!Pq9#mL2"}, 2),
                new QuizQuestion("What is phishing?", new[] {"Fishing sport", "Fake emails to steal info", "A type of virus"}, 1),
                new QuizQuestion("2FA means?", new[] {"Two Factor Authentication", "To Fast Access", "Two File Archive"}, 0)
            };
        }

        private void StartQuiz()
        {
            quizScore = 0;
            quizIndex = 0;
            AskQuizQuestion();
        }

        private void AskQuizQuestion()
        {
            if (quizIndex >= quizQuestions.Count)
            {
                AppendChat("Quiz", $"Quiz complete! Your score: {quizScore}/{quizQuestions.Count}");
                LogActivity($"Quiz completed. Score: {quizScore}/{quizQuestions.Count}");
                return;
            }

            QuizQuestion q = quizQuestions[quizIndex];
            string options = "";
            for (int i = 0; i < q.Options.Length; i++)
                options += $"{i + 1}. {q.Options[i]}\n";

            AppendChat("Quiz", $"Q{quizIndex + 1}: {q.Question}\n{options}Type 1, 2, or 3 to answer");
        }

        // Called when user types 1, 2, 3 during quiz
        public void CheckQuizAnswer(int answer)
        {
            if (quizIndex >= quizQuestions.Count) return;

            if (answer - 1 == quizQuestions[quizIndex].CorrectIndex)
            {
                quizScore++;
                AppendChat("Quiz", "Correct!");
            }
            else
            {
                AppendChat("Quiz", $"Wrong. Correct answer: {quizQuestions[quizIndex].Options[quizQuestions[quizIndex].CorrectIndex]}");
            }

            quizIndex++;
            (this.Controls["lblScore"] as Label).Text = $"Score: {quizScore}/{quizQuestions.Count}";
            AskQuizQuestion();
        }

        // Task 4: Activity Log
        private void LogActivity(string action)
        {
            string entry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {action}";
            activityLog.Add(entry);
            File.AppendAllText(logFilePath, entry + Environment.NewLine);
        }

        private void LoadActivityLog()
        {
            if (File.Exists(logFilePath))
                activityLog = new List<string>(File.ReadAllLines(logFilePath));
        }

        private void ViewActivityLog()
        {
            string log = string.Join("\n", activityLog.TakeLast(10));
            MessageBox.Show(log, "Last 10 Activities");
        }

        private void ClearActivityLog()
        {
            activityLog.Clear();
            File.WriteAllText(logFilePath, "");
            MessageBox.Show("Activity log cleared");
        }

        private void AppendChat(string sender, string message)
        {
            RichTextBox txtChat = this.Controls["txtChat"] as RichTextBox;
            txtChat.SelectionColor = sender == "Bot"? Color.Cyan : sender == "You"? Color.Yellow : Color.LightGreen;
            txtChat.AppendText($"{sender}: ");
            txtChat.SelectionColor = Color.White;
            txtChat.AppendText(message + "\n\n");
            txtChat.ScrollToCaret();
        }
    }

    // Task 2: Quiz question class
    public class QuizQuestion
    {
        public string Question { get; set; }
        public string[] Options { get; set; }
        public int CorrectIndex { get; set; }
        public QuizQuestion(string q, string[] opts, int correct) { Question = q; Options = opts; CorrectIndex = correct; }
    }
}