using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

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
                string path = GetQuestionsFilePath();
                using FileStream stream = File.OpenRead(path);
                List<AQuestion>? questions = await JsonSerializer.DeserializeAsync<List<AQuestion>>(stream);
                return questions ?? new List<AQuestion>();
            }

            QuestionsList = await ReadQuestions();
        }

        static string GetQuestionsFilePath()
        {
            try
            {
                string current = Directory.GetCurrentDirectory();
                DirectoryInfo? dir = new DirectoryInfo(current);
                while (dir != null)
                {
                    if (Directory.Exists(Path.Combine(dir.FullName, "Code")))
                    {
                        string dataDir = Path.Combine(dir.FullName, "Data");
                        Directory.CreateDirectory(dataDir);
                        return Path.Combine(dataDir, "Questions.json");
                    }
                    dir = dir.Parent;
                }
            }
            catch{ }

            string fallback = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            Directory.CreateDirectory(fallback);
            return Path.Combine(fallback, "Questions.json");
        }
    }
}