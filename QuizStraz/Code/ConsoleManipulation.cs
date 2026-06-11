//namespace ConsoleManipulation
//{
    class ConsoleManipulation
    {
        protected static int xAxisOrigin = 0;
        protected static int yAxisOrigin = 0;

        public static void ReplaceText(string text, int x, int y)
        {
            try
            {
                Console.SetCursorPosition(xAxisOrigin + x, yAxisOrigin + y);
                Console.Write(text);
                Console.SetCursorPosition(xAxisOrigin + x, yAxisOrigin + y);
            }   
            catch (ArgumentOutOfRangeException e)
            {
                Console.Write("Exception: " + e.Message);
            }         
        }
        public static void EmptyLineAndReplaceText(string text, int y)
        {
            EmptyLineAndSetCursor(y);
            Console.Write(text);             
        }
        public static void EmptyLine(int y)
        {
            try
            {
                Console.SetCursorPosition(0, y);
                Console.Write(new string(' ', Console.WindowWidth));                
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Write("Exception: " + e.Message);
            }
        }
        public static void EmptyLineAndSetCursor(int y)
        {
            try
            {
                Console.SetCursorPosition(0, y);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, y);                
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Write("Exception: " + e.Message);
            }
        }
        public static void TrySettingYCursorPosition(int y, bool defaultTopOrBottom)
        {
            int targetSafeYPosition = 0;
            if(defaultTopOrBottom) targetSafeYPosition = Console.BufferHeight;
            try
            {
                Console.SetCursorPosition(0, y);
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Error while setting cursor position, setting back to safe spot");
                Console.SetCursorPosition(0, targetSafeYPosition-1);
            }
        }
    }
//}