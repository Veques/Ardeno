using Ardeno.Helpers.Enums;

namespace Ardeno.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        public QuestionDifficulty QuestionDifficulty { get; set; }
        public string QuestionTitle { get; set; }
        public string FirstAnswer { get; set; }
        public string SecondAnswer { get; set; }
        public string ThirdAnswer { get; set; }
        public string FourthAnswer { get; set; }
        public string CorrectAnswer { get; set; }

    }
}
