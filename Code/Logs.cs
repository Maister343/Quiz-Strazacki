using QuizStraz.Code;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace QuizStraz
{
    static class QuizLogs
    {
        const string LogsFileName = "Log.txt";
        static readonly string LogsPath = Paths.InitializePath(LogsFileName);
        public static void SaveScore(int correctAnswers, int questionAmount)
        {
            if(!File.Exists(LogsPath))
            {
                using FileStream fs = File.Create(LogsPath);  
            }

            bool overriteLog = IsLogFileSizeTooBig();

            if(overriteLog)
            {
                List<string> lines = new List<string>();

                lines = File.ReadAllLines(LogsPath).ToList();

                if (lines.Count > 0)
                    lines.RemoveAt(0);

                File.WriteAllLines(LogsPath, lines);
            }
            
            using (StreamWriter sw = new StreamWriter(LogsPath, append: true))
            {
                sw.WriteLine("{0}: Udało ci się zdobyć {1}/{2}", DateTime.Today.ToString("dd/MM/yy"), correctAnswers, questionAmount);
            }
            

            bool IsLogFileSizeTooBig()
            {
                if(File.Exists(LogsPath))
                {
                    FileInfo logInfo = new FileInfo(LogsPath);
                    if(logInfo.Length > 1000000)
                    {
                        return true;
                    }    
                }
                return false;
            }
        }
        public static void ReadScore()
        {
            Console.Clear();
            string lastLine = string.Empty;
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
                        lastLine = line;
                    }       
                }
            }

            if (lastLine == null || string.Equals(lastLine, string.Empty)) Console.WriteLine("Historia jest pusta");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Wciśnij przycisk aby kontynuować");
            Console.ReadKey(true);
        }
    }
}