namespace CyberBotGUI
{
    public class Question
    {
        public string Text { get; set; }
        public string[] Options { get; set; }
        public int CorrectIndex { get; set; }
        public string Explanation { get; set; }
        public bool IsTrueFalse { get; set; }

        public Question(string text, string[] options, int correctIndex, string explanation, bool isTrueFalse = false)
        {
            Text = text; Options = options; CorrectIndex = correctIndex; Explanation = explanation; IsTrueFalse = isTrueFalse;
        }
    }
}