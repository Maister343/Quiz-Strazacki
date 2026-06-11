using System.Drawing;

namespace QuizStraz
{
    static class QuizMenu
    {           
        public static void InitializeQuiz()
        {
            ConsoleKeyInfo input;
            QuizExec.onChallengeCompleted += Settings.ChallengeCompletion;
            bool printHowToGetAchievements = false;

            while(true)
            {
                Console.Clear();
                PrintMenu();
                input = Console.ReadKey(true);
                
                if(input.Key == ConsoleKey.Q || input.Key == ConsoleKey.Escape) 
                {                   
                    break;
                }
                if(input.Key == ConsoleKey.D1 || input.Key == ConsoleKey.NumPad1)
                {
                    QuizExec.QuizExecution();
                }
                if(input.Key == ConsoleKey.D2 || input.Key == ConsoleKey.NumPad2)
                {
                    QuizLogs.ReadScore();
                }
                if(input.Key == ConsoleKey.D3 || input.Key == ConsoleKey.NumPad3)
                {
                    Settings.SetingsExec();
                }
                if(input.Key == ConsoleKey.D4 || input.Key == ConsoleKey.NumPad4)
                {
                    QuizExec.QuizExecution(true);
                }
                if(input.Key == ConsoleKey.D5 || input.Key == ConsoleKey.NumPad5)
                {
                    printHowToGetAchievements = true;
                }
            }
            
            QuizExec.onChallengeCompleted -= Settings.ChallengeCompletion;


            void PrintMenu()
            {
                Console.WriteLine("<--- Menu główne Quizu --->");
                Console.WriteLine("");
                Console.WriteLine("1. Zacznij quiz");
                Console.WriteLine("2. Historia wyników");
                Console.WriteLine("3. Ustawienia");
                Console.WriteLine("4. Wszystkie pytania challenge");
                Console.WriteLine("5. Możliwe odznaki do zdobycia");
                Console.WriteLine("Q lub ESC aby wyjść z programu lub cofnąć się");
                Console.WriteLine();

                if(Settings.achievements.HasCompletedChallenge == true)
                {
                    string msg = string.Empty;
                    int cursorTop = Console.CursorTop;
                    int baseStarLocation = 0;
                    int nextStarLocation = 20;
                    switch(Settings.achievements.achievementLevel)
                    {
                        case StarColor.gold:
                            PrintStar(Settings.starColorsToTxtCode.GetValueOrDefault(StarColor.gold)!, StarColorAnswerRequirements.GoldStarRequirement, baseStarLocation, cursorTop);
                            PrintStar(Settings.starColorsToTxtCode.GetValueOrDefault(StarColor.silver)!, StarColorAnswerRequirements.SilverStarRequirement, nextStarLocation, cursorTop);
                            PrintStar(Settings.starColorsToTxtCode.GetValueOrDefault(StarColor.bronze)!, StarColorAnswerRequirements.BronzeStarRequirement, nextStarLocation*2, cursorTop);
                            break;
                        case StarColor.silver:
                            PrintStar(Settings.starColorsToTxtCode.GetValueOrDefault(StarColor.silver)!, StarColorAnswerRequirements.SilverStarRequirement, baseStarLocation, cursorTop);
                            PrintStar(Settings.starColorsToTxtCode.GetValueOrDefault(StarColor.bronze)!, StarColorAnswerRequirements.BronzeStarRequirement, nextStarLocation, cursorTop);
                            break;
                        case StarColor.bronze:
                            PrintStar(Settings.starColorsToTxtCode.GetValueOrDefault(StarColor.bronze)!, StarColorAnswerRequirements.BronzeStarRequirement, baseStarLocation, cursorTop);
                            break;
                    }
                    
                    void PrintStar(string colorCode, int scoreNumber,int whereXAxis, int whereYAxis) //Star height: 9, Width: 17
                    {  
                        try
                        {
                            Console.Write(colorCode);
                            Console.SetCursorPosition(whereXAxis, whereYAxis++);
                            Console.Write("        *        "); Console.SetCursorPosition(whereXAxis, whereYAxis++);
                            Console.Write("       ***       "); Console.SetCursorPosition(whereXAxis, whereYAxis++);
                            Console.Write("      *****      "); Console.SetCursorPosition(whereXAxis, whereYAxis++);
                            Console.Write("*****************"); Console.SetCursorPosition(whereXAxis, whereYAxis++);
                            Console.Write("  *****{0}*****  ", scoreNumber); Console.SetCursorPosition(whereXAxis, whereYAxis++);
                            Console.Write("    *********    "); Console.SetCursorPosition(whereXAxis, whereYAxis++);
                            Console.Write("   ***********   "); Console.SetCursorPosition(whereXAxis, whereYAxis++);
                            Console.Write("  ****     ****  "); Console.SetCursorPosition(whereXAxis, whereYAxis++);
                            Console.Write(" ***         *** "); Console.SetCursorPosition(whereXAxis, whereYAxis++);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            Console.Write("Błąd w drukowaniu gwiazdek bo okno konsoli jest za małe");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;

                if(printHowToGetAchievements)
                {
                    PrintHowToGetAchievements();
                    printHowToGetAchievements = false;
                }

                void PrintHowToGetAchievements()
                {
                    Console.WriteLine();
                    Console.WriteLine("Brązowa gwiazdka jest za poprawne odpowiedzenie na {0} pytań w jednym quizie", StarColorAnswerRequirements.BronzeStarRequirement);
                    Console.WriteLine("Srebrna gwiazdka jest za poprawne odpowiedzenie na {0} pytań w jednym quizie", StarColorAnswerRequirements.SilverStarRequirement);
                    Console.WriteLine("Złota gwiazdka jest za poprawne odpowiedzenie na wszystkie pytania w jednym quizie");
                    Console.WriteLine("Aby dostać więcej pytań na jeden quiz można zmienić ilość zadawanych pytań w ustawieniach lub podjąć się wszystkich pytań challenge");
                    Console.WriteLine();
                }
            }
        }
    }
}