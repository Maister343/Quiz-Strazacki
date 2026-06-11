using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace QuizStraz
{
    static class QuizLogs
    {
        static readonly string LogsPath = InitializeLogsPath(); 
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

        static string InitializeLogsPath()
        {
            try
            {
                string current = Directory.GetCurrentDirectory();
                DirectoryInfo? dir = new DirectoryInfo(current);
                while (dir != null)
                {
                    // if this directory contains a "Code" folder, assume it's the project root
                    if (Directory.Exists(Path.Combine(dir.FullName, "Code")))
                    {
                        string dataDir = Path.Combine(dir.FullName, "Data");
                        Directory.CreateDirectory(dataDir);
                        return Path.Combine(dataDir, "Log.txt");
                    }
                    dir = dir.Parent;
                }
            }
            catch
            {
                // fall through to fallback
            }

            // fallback: create Data folder in current directory
            string fallback = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            Directory.CreateDirectory(fallback);
            return Path.Combine(fallback, "Log.txt");
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