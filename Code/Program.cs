using QuizStraz.Code;
using System.Text.Json;

namespace QuizStraz
{
    class QuizMain
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "Quiz MDP";
            Console.ForegroundColor = ConsoleColor.White;
            Paths.InitializeDataFolderPath();
            await QuizQuestions.AssignList();
            await Settings.HandleConfig();
            await Settings.HandleAchievements();
            QuizMenu.InitializeQuiz();
        }
    }
}
