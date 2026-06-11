using System.Reflection.Metadata;

namespace QuizStraz
{
    static class QuizLogs
    {
        const string LogsPath = "Log.txt"; 
        public static void SaveScore(int correctAnswers, int questionAmount)
        {
            if(!File.Exists(LogsPath))
            {
                using FileStream fs = File.Create(LogsPath);  
            } 
            using (StreamWriter sw = new StreamWriter(LogsPath, append: true))
            {
                sw.WriteLine("{0}: Udało ci się zdobyć {1}/{2}", DateTime.Now, correctAnswers, questionAmount);
            }
        }
        public static void ReadScore()
        {
            Console.Clear();
            if(!File.Exists(LogsPath))
            {
                Console.WriteLine("Aktualnie nie ma żadnej historii do pokazania");
            }
            else
            {    
                string? line;
                bool alternatingColors = false;
                using (StreamReader sr = new StreamReader(LogsPath))
                {
                    while((line = sr.ReadLine()) != null)
                    {
                        if(alternatingColors) Console.ForegroundColor = Settings.config.AlternatingColor1;
                        else Console.ForegroundColor = Settings.config.AlternatingColor2;
                        Console.WriteLine(line);
                        alternatingColors = !alternatingColors;
                    }       
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Wciśnij przycisk aby kontynuować");
            Console.ReadKey(true);
        }
    }
}