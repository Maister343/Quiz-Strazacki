using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuizStraz
{
    enum CorrectAnswerEnum
    {
        A,B,C
    }
    class AQuestion
    {
        public string QuestionTitle {get; set;} = string.Empty;
        public string AnswerA {get; set;} = string.Empty;
        public string AnswerB {get; set;} = string.Empty;
        public string AnswerC {get; set;} = string.Empty;
        public CorrectAnswerEnum CorrectAnswer {get; set;}
    }
    static class QuizQuestions
    {
        public static List<AQuestion> QuestionsList = new List<AQuestion>();
        public static async Task AssignList()
        {
            async Task<List<AQuestion>> ReadQuestions()
            {
                using FileStream stream = File.OpenRead("Questions.json");
                List<AQuestion>? questions = await JsonSerializer.DeserializeAsync<List<AQuestion>>(stream);
                return questions ?? new List<AQuestion>();
            }

            QuestionsList = await ReadQuestions();
        }
    }
}