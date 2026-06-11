using System.Collections;
using System.Drawing;
using Microsoft.Extensions.Hosting.Internal;

namespace QuizStraz
{
    static class QuizExec
    {
        public delegate void OnChallengeCompleted(StarColor starColor);
        public static event OnChallengeCompleted? onChallengeCompleted;
        public static void QuizExecution(bool tryHardMode = false)
        {
            bool doQuit = false;
            Console.Clear();
             
            int amountOfQuestions = 0;
            int amountOfCorrectAnswers = 0;
            if(tryHardMode) amountOfQuestions = QuizQuestions.QuestionsList.Count();
            else amountOfQuestions = Settings.config.HowManyQuestionsToAsk;    

            Random rng = new Random();
            IEnumerable<AQuestion> aQuestions = QuizQuestions.QuestionsList
            .OrderBy(q => rng.Next())
            .Take(amountOfQuestions);

            List<AQuestion> QuestionsToAsk = aQuestions.ToList();

            int questionCounter = 1;
            int questionYPosition = 0;
            int[] questionToAskHeightPropeties = new int[4];
            foreach(AQuestion q in QuestionsToAsk)
            {
                int safeYCursorPosition = Console.CursorTop;
                questionToAskHeightPropeties = CheckHeightOfQuestion(q);

                ConsoleManipulation.TrySettingYCursorPosition(safeYCursorPosition, true);
                Console.ForegroundColor = Settings.config.QuestionColor;
                Console.WriteLine(questionCounter + ". " + q.QuestionTitle);
                Console.ForegroundColor = Settings.config.GivenAnswerColor;
                Console.WriteLine("A. " + q.AnswerA);
                Console.WriteLine("B. " + q.AnswerB);
                Console.WriteLine("C. " + q.AnswerC);
                safeYCursorPosition = Console.CursorTop;
                questionYPosition = safeYCursorPosition;
                questionCounter++;            

                AskedQuestionHandler(q);
                ConsoleManipulation.TrySettingYCursorPosition(safeYCursorPosition, true);               
                Console.WriteLine();
                if(doQuit) break;
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine("Koniec quizu, twój wynik to: {0}/{1}", amountOfCorrectAnswers, amountOfQuestions);
            QuizLogs.SaveScore(amountOfCorrectAnswers, amountOfQuestions);
            switch(amountOfCorrectAnswers) // Checking achievement requirements
            {
                case >= StarColorAnswerRequirements.GoldStarRequirement:
                    onChallengeCompleted?.Invoke(StarColor.gold);
                    break;
                case >= StarColorAnswerRequirements.SilverStarRequirement:
                    onChallengeCompleted?.Invoke(StarColor.silver);
                    break;
                case >= StarColorAnswerRequirements.BronzeStarRequirement:
                    onChallengeCompleted?.Invoke(StarColor.bronze);
                    break;
            }
            Console.WriteLine("Wciśnij przycisk aby wrócić do menu");
            Console.ReadKey(true);

            int[] CheckHeightOfQuestion(AQuestion questionToCheck)
            {
                int[] questionHeightPropeties = new int[]
                {
                    CheckHeightOfQuestionLine(questionToCheck.QuestionTitle.Length),
                    CheckHeightOfQuestionLine(questionToCheck.AnswerA.Length),
                    CheckHeightOfQuestionLine(questionToCheck.AnswerB.Length),
                    CheckHeightOfQuestionLine(questionToCheck.AnswerC.Length),
                };                
                
                int CheckHeightOfQuestionLine(int lineLenght)
                {
                    int howMuchHeightLineTakes = 0;
                    do
                    {
                        howMuchHeightLineTakes++;
                        lineLenght = lineLenght - Console.BufferWidth;
                    }while(lineLenght > Console.BufferWidth);

                    return howMuchHeightLineTakes;
                }

                return questionHeightPropeties;
            }

            void AskedQuestionHandler(AQuestion currentQuestion)
            {
                ConsoleKeyInfo input;

                while(true)
                {
                    input = Console.ReadKey(true);
                    
                    if(input.Key == ConsoleKey.Q || input.Key == ConsoleKey.Escape)
                    {
                        doQuit = true;
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    } 
                    if(input.Key == ConsoleKey.A || input.Key == ConsoleKey.D1 || input.Key == ConsoleKey.NumPad1)
                    {
                        CheckAnswer(currentQuestion, CorrectAnswerEnum.A);
                        break;
                    }
                    if(input.Key == ConsoleKey.B || input.Key == ConsoleKey.D2 || input.Key == ConsoleKey.NumPad2)
                    {
                        CheckAnswer(currentQuestion, CorrectAnswerEnum.B);
                        break;
                    }
                    if(input.Key == ConsoleKey.C || input.Key == ConsoleKey.D3 || input.Key == ConsoleKey.NumPad3)
                    {
                        CheckAnswer(currentQuestion, CorrectAnswerEnum.C);
                        break;
                    }
                }
            }

            void CheckAnswer(AQuestion currentQuestion, QuizStraz.CorrectAnswerEnum answerEnum)
            {
                if(currentQuestion.CorrectAnswer == answerEnum)
                {
                    amountOfCorrectAnswers++;
                    MarkAnswer(questionYPosition, true);
                } 
                else
                {
                    MarkAnswer(questionYPosition, false);
                }

                void MarkAnswer(int yPosition, bool isCorrect)
                {
                    CorrectAnswerEnum enumToSwitch;
                    if(!isCorrect)
                    {
                        enumToSwitch = answerEnum;
                        ColorAnswer(Settings.config.WrongAnswerColor);
                    }

                    enumToSwitch = currentQuestion.CorrectAnswer;
                    ColorAnswer(Settings.config.CorrectAnswerColor);

                    void ColorAnswer(ConsoleColor color)
                    {
                        Console.ForegroundColor = color;
                        switch(enumToSwitch)
                        {
                            case CorrectAnswerEnum.A:
                                ConsoleManipulation.EmptyLineAndReplaceText("A. " + currentQuestion.AnswerA, yPosition-questionToAskHeightPropeties[1]-questionToAskHeightPropeties[2]-questionToAskHeightPropeties[3]);
                                break;
                            case CorrectAnswerEnum.B:
                                ConsoleManipulation.EmptyLineAndReplaceText("B. " + currentQuestion.AnswerB, yPosition-questionToAskHeightPropeties[2]-questionToAskHeightPropeties[3]);
                                break;
                            case CorrectAnswerEnum.C:
                                ConsoleManipulation.EmptyLineAndReplaceText("C. " + currentQuestion.AnswerC, yPosition-questionToAskHeightPropeties[3]);
                                break;
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
        }
    }
}