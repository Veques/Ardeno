namespace Ardeno.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        public string QuestionDifficulty { get; set; }
        public string QuestionTitle { get; set; }
        public string FirstAnswer { get; set; }
        public string SecondAnswer { get; set; }
        public string ThirdAnswer { get; set; }
        public string FourthAnswer { get; set; }
        public string CorrectAnswer { get; set; }
        public double Done { get; set; }

    }
}
