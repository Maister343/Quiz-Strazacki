using QuizStraz.Code;
using System.Diagnostics.Tracing;
using System.Reflection.PortableExecutable;
using System.Xml.Serialization;

namespace QuizStraz
{
    public class Config : IEquatable<Config>
    {
        public int HowManyQuestionsToAsk { get; set; }
        public ConsoleColor QuestionColor { get; set; }
        public ConsoleColor GivenAnswerColor { get; set; }
        public ConsoleColor CorrectAnswerColor { get; set; }
        public ConsoleColor WrongAnswerColor { get; set; }
        public ConsoleColor AlternatingColor1 { get; set; }
        public ConsoleColor AlternatingColor2 { get; set; }

        public bool Equals(Config? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return HowManyQuestionsToAsk == other.HowManyQuestionsToAsk
                && QuestionColor == other.QuestionColor
                && GivenAnswerColor == other.GivenAnswerColor
                && CorrectAnswerColor == other.CorrectAnswerColor
                && WrongAnswerColor == other.WrongAnswerColor
                && AlternatingColor1 == other.AlternatingColor1
                && AlternatingColor2 == other.AlternatingColor2;
        }
        public override bool Equals(object? obj) => Equals(obj as Config);
        public override int GetHashCode()
        {
            return HashCode.Combine(
                HowManyQuestionsToAsk,
                QuestionColor,
                GivenAnswerColor,
                CorrectAnswerColor,
                WrongAnswerColor,
                AlternatingColor1,
                AlternatingColor2);
        }
    }
    public class Achievements
    {
        public bool HasCompletedChallenge {get; set;}
        public StarColor achievementLevel {get; set;}
    }
    public static class StarColorAnswerRequirements
    {
        public const int GoldStarRequirement = 516;
        public const int SilverStarRequirement = 350;
        public const int BronzeStarRequirement = 200;
    }
    public enum StarColor
    {
        none,bronze,silver,gold
    }
    static class Settings
    {
        public static Config config = new Config();
        public static Achievements achievements = new Achievements();
        public static Dictionary<StarColor, string> starColorsToTxtCode = new Dictionary<StarColor, string>
        {
            //Console.Write($"\x1b[38;2;{r};{g};{b}m");
            {StarColor.none, string.Empty},
            {StarColor.bronze, $"\x1b[38;2;60;40;0m"},
            {StarColor.silver, $"\x1b[38;2;100;100;100m"},
            {StarColor.gold, $"\x1b[38;2;135;110;55m"}  
        };
        static Dictionary<ConsoleColor, string> polishColors = new Dictionary<ConsoleColor, string>
        {
            {ConsoleColor.Black, "Czarny"},
            {ConsoleColor.DarkBlue, "Ciemny Niebieski"},
            {ConsoleColor.DarkGreen, "Ciemny Zielony"},
            {ConsoleColor.DarkCyan, "Ciemny Jasny Niebieski"},
            {ConsoleColor.DarkRed, "Ciemny Czerwony"},
            {ConsoleColor.DarkMagenta, "Ciemny Różowy"},
            {ConsoleColor.DarkYellow, "Ciemny Żólty"},
            {ConsoleColor.Gray, "Szary"},
            {ConsoleColor.DarkGray, "Ciemny Szary"},
            {ConsoleColor.Blue, "Niebieski"},
            {ConsoleColor.Green, "Zielony"},
            {ConsoleColor.Cyan, "Jasny Niebieski"},
            {ConsoleColor.Red, "Czerwony"},
            {ConsoleColor.Magenta, "Różowy"},
            {ConsoleColor.Yellow, "Żółty"},
            {ConsoleColor.White, "Biały"}

        };
        static string menuMessage = string.Empty;
        public static async void SetingsExec()
        {
            ConsoleKeyInfo input;

            while(true)
            {
                Console.Clear();
                SettingsMenu(menuMessage);
                input = Console.ReadKey(true);

                if(input.Key == ConsoleKey.Q || input.Key == ConsoleKey.Escape) 
                {
                    Config checkIfSame = LoadConfig();
                    if(!checkIfSame.Equals(config))
                    {
                        Console.WriteLine("Chesz wyjść z ustawień bez zapisywania? (Y/N) lub (1/2)");
                        if(YNConfirmation())
                        {
                            config = LoadConfig();
                            break;
                        } 
                        else continue;
                    }
                    config = LoadConfig();
                    break;
                }
                if(input.Key == ConsoleKey.D1 || input.Key == ConsoleKey.NumPad1) //Change amount of questions to be asked
                {
                    config.HowManyQuestionsToAsk = ChangeAmountOfQuestionsToAsk();
                }
                if(input.Key == ConsoleKey.D2 || input.Key == ConsoleKey.NumPad2) //Change color of something
                {
                    ColorChanging();
                }
                if(input.Key == ConsoleKey.D3 || input.Key == ConsoleKey.NumPad3) //Print questions
                {
                    Console.Clear();
                    int questionCounter = 1;
                    foreach(AQuestion q in QuizQuestions.QuestionsList)
                    {
                        Console.ForegroundColor = config.QuestionColor;
                        Console.WriteLine(questionCounter + ". " + q.QuestionTitle);
                        if(q.CorrectAnswer == CorrectAnswerEnum.A) Console.ForegroundColor = config.CorrectAnswerColor;
                        else Console.ForegroundColor = config.GivenAnswerColor;
                        Console.WriteLine("A. " + q.AnswerA);
                        if(q.CorrectAnswer == CorrectAnswerEnum.B) Console.ForegroundColor = config.CorrectAnswerColor;
                        else Console.ForegroundColor = config.GivenAnswerColor;
                        Console.WriteLine("B. " + q.AnswerB);
                        if(q.CorrectAnswer == CorrectAnswerEnum.C) Console.ForegroundColor = config.CorrectAnswerColor;
                        else Console.ForegroundColor = config.GivenAnswerColor;
                        Console.WriteLine("C. " + q.AnswerC);
                        Console.WriteLine();
                        questionCounter++;
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("W razie wątpliwości w folderze instalacji Quizu znajduje się oficjalna lista pytań z odpowiedziami");
                    Console.WriteLine("Kliknij przycisk by wrócić");
                    Console.ReadKey(true);
                }
                if(input.Key == ConsoleKey.D7 || input.Key == ConsoleKey.NumPad7 || input.Key == ConsoleKey.S) //Save config
                {
                    SaveConfig();
                    menuMessage = "Zapisano ustawienia";
                }
                if(input.Key == ConsoleKey.D8 || input.Key == ConsoleKey.NumPad8 || input.Key == ConsoleKey.R) //Reset config
                {
                    Console.WriteLine("Na pewno chcesz przywrócić ustawienia fabryczne? (Y/N) lub (1/2)");
                    if(YNConfirmation())
                    {
                        await HandleConfig(true);
                        menuMessage = "Przywrócono ustawienia fabryczne";
                    }
                    else menuMessage = "Akcja została anulowana";
                }
                if(input.Key == ConsoleKey.D9 || input.Key == ConsoleKey.NumPad9)
                {
                    Console.WriteLine("Na pewno chcesz zapomnieć o osiągnięciach? (Y/N) lub (1/2)");
                    if(YNConfirmation())
                    {
                        await HandleAchievements(true);
                        menuMessage = "Zapomniano o zdobytych osiągnięciach";
                    }
                    else menuMessage = "Akcja została anulowana";
                }
            }
            void SettingsMenu(string lastMessage = "")
            {
                Console.ForegroundColor = Settings.config.AlternatingColor1;
                Console.WriteLine("<--- Ustawienia --->");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("");
                Console.WriteLine("1. Ilość pytań do zadania, aktualnie: {0}", config.HowManyQuestionsToAsk);
                Console.WriteLine("2. Ustawienia Kolorów");
                Console.WriteLine("3. Lista pytań");
                Console.WriteLine("");
                Console.WriteLine("7. - Zapisz ustawienia");
                Console.WriteLine("8. - Przywróć ustawienia fabryczne");
                Console.WriteLine("9. - Zapomnij o zdobytych osiągnięciach");
                Console.WriteLine();
                Console.WriteLine("Przywracanie ustawień fabryczych nie usuwa zdobytych osiągnięć");
                Console.WriteLine();
                int cursorYPlacement = Console.CursorTop;
                if(!string.IsNullOrEmpty(lastMessage))
                {
                    Console.WriteLine("Ostatnia wiadomość: " + lastMessage);
                    cursorYPlacement = Console.CursorTop;
                } 
                string watermarkMessage = "Program zrobiony przez Oliver Szturc, v1.0";
                ConsoleManipulation.ReplaceText(watermarkMessage, Console.BufferWidth-watermarkMessage.Length, Console.BufferHeight-1);
                Console.SetCursorPosition(0, cursorYPlacement);
            }
            int ChangeAmountOfQuestionsToAsk()
            {
                Console.Clear();
                string errorMessage;
                while(true)
                {
                    ConsoleManipulation.EmptyLineAndSetCursor(Console.CursorTop);
                    Console.Write("Wpisz nową ilość pytań do zdania: ");
                    int errorMessageYLocation = Console.CursorTop+1;
                    int xCursorLocation = Console.CursorLeft;
                    string inputString = Console.ReadLine() ?? "q";
                    if(string.Equals(inputString,"q")) return config.HowManyQuestionsToAsk;
                    try
                    {
                        int newAmount = Convert.ToInt32(inputString);
                        if(newAmount > QuizQuestions.QuestionsList.Count())
                        {
                            errorMessage = $"Ilość zadanych pytań nie może być większa niż pula pytań ({QuizQuestions.QuestionsList.Count()})";
                        }
                        else if(newAmount <= 0)
                        {
                            errorMessage = "Wpisana liczba nie może być mniejsza niż 1";
                        }
                        else return newAmount;
                    }
                    catch (FormatException)
                    {
                        errorMessage = "Nowa ilość musi być cyfrą";
                    }
                    catch (OverflowException)
                    {
                        errorMessage = "Wpisana liczba jest za duża";
                    }
                    ConsoleManipulation.EmptyLineAndReplaceText(errorMessage, errorMessageYLocation);
                    Console.SetCursorPosition(xCursorLocation, errorMessageYLocation-1);
                }
            }
        }
        static void ColorChanging()
        {   
            Console.Clear();
            ConsoleKeyInfo input;

            while(true)
            {
                ColorChangingMenu();
                input = Console.ReadKey(true);
                
                if(input.Key == ConsoleKey.Q || input.Key == ConsoleKey.Escape) break;
                if(input.Key == ConsoleKey.D1 || input.Key == ConsoleKey.NumPad1)
                {
                    config.QuestionColor = ColorPickMenu("Pytania", config.QuestionColor);
                }
                if(input.Key == ConsoleKey.D2 || input.Key == ConsoleKey.NumPad2)
                {
                    config.GivenAnswerColor = ColorPickMenu("Odpowiedzi", config.GivenAnswerColor);
                }
                if(input.Key == ConsoleKey.D3 || input.Key == ConsoleKey.NumPad3)
                {
                    config.CorrectAnswerColor = ColorPickMenu("Poprawnej odpowiedzi", config.CorrectAnswerColor);
                }
                if(input.Key == ConsoleKey.D4 || input.Key == ConsoleKey.NumPad4)
                {
                    config.WrongAnswerColor = ColorPickMenu("Złej odpowiedzi", config.WrongAnswerColor);
                }
                if(input.Key == ConsoleKey.D5 || input.Key == ConsoleKey.NumPad5)
                {
                    config.AlternatingColor1 = ColorPickMenu("Alternatywy1", config.AlternatingColor1);
                }
                if(input.Key == ConsoleKey.D6 || input.Key == ConsoleKey.NumPad6)
                {
                    config.AlternatingColor2 = ColorPickMenu("Alternatywy2", config.AlternatingColor2);
                }
                Console.Clear();
            }

            void ColorChangingMenu()
            {
                Console.WriteLine("<--- Ustawienia Kolorów --->");
                Console.WriteLine("");
                Console.WriteLine("1. Kolor pytania: " + polishColors.GetValueOrDefault(config.QuestionColor));
                Console.WriteLine("2. Kolor odpowiedzi: " + polishColors.GetValueOrDefault(config.GivenAnswerColor));
                Console.WriteLine("3. Kolor poprawnej odpowiedzi: " + polishColors.GetValueOrDefault(config.CorrectAnswerColor));
                Console.WriteLine("4. Kolor złej odpowiedzi: " + polishColors.GetValueOrDefault(config.WrongAnswerColor));
                Console.WriteLine("5. Kolor alternatywny1: " + polishColors.GetValueOrDefault(config.AlternatingColor1));
                Console.WriteLine("6. Kolor alternatywny2: " + polishColors.GetValueOrDefault(config.AlternatingColor2));
                Console.WriteLine("");
            }
        }
        static ConsoleColor ColorPickMenu(string pickMenuTitleText, ConsoleColor color)
        {
            Console.Clear();
            Console.WriteLine("Wybierz kolor dla: " + pickMenuTitleText);
            Console.WriteLine();
            int colorNumber = 0;
            foreach(var e in polishColors)
            {
                Console.WriteLine("{0:X}. {1}", colorNumber, e.Value);
                colorNumber++;
            }

            ConsoleKeyInfo input;

            while(true)
            {
                input = Console.ReadKey(true);
            
                if(input.Key == ConsoleKey.Q || input.Key == ConsoleKey.Escape) break;
                if(input.Key == ConsoleKey.D0 || input.Key == ConsoleKey.NumPad0) return ConsoleColor.Black;
                if(input.Key == ConsoleKey.D1 || input.Key == ConsoleKey.NumPad1) return ConsoleColor.DarkBlue;
                if(input.Key == ConsoleKey.D2 || input.Key == ConsoleKey.NumPad2) return ConsoleColor.DarkGreen;
                if(input.Key == ConsoleKey.D3 || input.Key == ConsoleKey.NumPad3) return ConsoleColor.DarkCyan;
                if(input.Key == ConsoleKey.D4 || input.Key == ConsoleKey.NumPad4) return ConsoleColor.DarkRed;
                if(input.Key == ConsoleKey.D5 || input.Key == ConsoleKey.NumPad5) return ConsoleColor.DarkMagenta;
                if(input.Key == ConsoleKey.D6 || input.Key == ConsoleKey.NumPad6) return ConsoleColor.DarkYellow;
                if(input.Key == ConsoleKey.D7 || input.Key == ConsoleKey.NumPad7) return ConsoleColor.Gray;
                if(input.Key == ConsoleKey.D8 || input.Key == ConsoleKey.NumPad8) return ConsoleColor.DarkGray;
                if(input.Key == ConsoleKey.D9 || input.Key == ConsoleKey.NumPad9) return ConsoleColor.Blue;
                if(input.Key == ConsoleKey.A) return ConsoleColor.Green;
                if(input.Key == ConsoleKey.B) return ConsoleColor.Cyan;
                if(input.Key == ConsoleKey.C) return ConsoleColor.Red;
                if(input.Key == ConsoleKey.D) return ConsoleColor.Magenta;
                if(input.Key == ConsoleKey.E) return ConsoleColor.Yellow;
                if(input.Key == ConsoleKey.F) return ConsoleColor.White;
            }
            return color;
        }
        const string configXMLFileName = "config.xml";
        static readonly string configXMLFilePath = Paths.InitializePath(configXMLFileName);
        public static async Task HandleConfig(bool returnDefault = false)
        {
            if(returnDefault)
            {
                config = DefaultConfig();
                SaveConfig();
                return;
            }
            if(!File.Exists(configXMLFilePath))
            {
                config = DefaultConfig();
                SaveConfig();
            }
            else
            {
                config = LoadConfig();    
            }

            Config DefaultConfig()
            {
                return new Config
                {
                    HowManyQuestionsToAsk = 30,
                    QuestionColor = ConsoleColor.DarkYellow,
                    GivenAnswerColor = ConsoleColor.Cyan,
                    CorrectAnswerColor = ConsoleColor.Green,
                    WrongAnswerColor = ConsoleColor.Red,
                    AlternatingColor1 = ConsoleColor.Magenta,
                    AlternatingColor2 = ConsoleColor.DarkMagenta
                };
            }
        }
        static Config LoadConfig()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Config));
            
            using(FileStream fs = new FileStream(configXMLFilePath, FileMode.Open, FileAccess.Read))
            {
                return (Config)xmlSerializer.Deserialize(fs)!;
            }
        }
        static void SaveConfig()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Config));

            using (TextWriter sw = new StreamWriter(configXMLFilePath))
            {
                xmlSerializer.Serialize(sw, config);
            }
        }
        const string achievementsXMLFileName = "achievements.xml";
        static readonly string achievementsXMLFilePath = Paths.InitializePath(achievementsXMLFileName);
        public static void ChallengeCompletion(StarColor starColor)
        {
            achievements.HasCompletedChallenge = true;
            switch(starColor)
            {
                case StarColor.gold:
                    achievements.achievementLevel = StarColor.gold;
                    if((int)achievements.achievementLevel >= (int)StarColor.gold) break;
                    break;
                case StarColor.silver:
                    if((int)achievements.achievementLevel >= (int)StarColor.silver) break;
                    achievements.achievementLevel = StarColor.silver;
                    break;
                case StarColor.bronze:
                    if((int)achievements.achievementLevel >= (int)StarColor.bronze) break;
                    achievements.achievementLevel = StarColor.bronze;
                    break;
            }
            SaveAchievements();
        }
        public static async Task HandleAchievements(bool returnDefault = false)
        {
            if(returnDefault)
            {
                achievements = DefaultAchievements();
                SaveAchievements();
            }
            if(!File.Exists(achievementsXMLFilePath))
            {
                achievements = DefaultAchievements();
                SaveAchievements();
            }
            else
            {
                achievements = LoadAchievements();
            }

            Achievements DefaultAchievements()
            {
                return new Achievements
                {
                    HasCompletedChallenge = false,
                    achievementLevel = StarColor.none 
                };
            }
        }
        static Achievements LoadAchievements()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Achievements));

            using(FileStream fs = new FileStream(achievementsXMLFilePath, FileMode.Open, FileAccess.Read))
            {
                return (Achievements)xmlSerializer.Deserialize(fs)!;
            }
        }
        static void SaveAchievements()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Achievements));
            
            using(TextWriter sw = new StreamWriter(achievementsXMLFilePath))
            {
                xmlSerializer.Serialize(sw, achievements);
            }
        }
        public static bool YNConfirmation()
        {
            ConsoleKeyInfo input;
            while(true)
            {
                input = Console.ReadKey(true);
                if(input.Key == ConsoleKey.Y || input.Key == ConsoleKey.D1 || input.Key == ConsoleKey.NumPad1) return true;
                else return false;
            }
        }
    }
}