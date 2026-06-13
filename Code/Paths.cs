using System;
using System.Collections.Generic;
using System.Text;

namespace QuizStraz.Code
{
    public static class Paths
    {
        public const string DataFolderName = "QuizStrazData";
        public static readonly string DataFolderPath = InitializeDataFolderPath();
        static string AppDataPath()
        {
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return appDataFolder;
        }
        public static String InitializeDataFolderPath()
        {
            string appDataPath = AppDataPath();
            string dataFolderPath = Path.Combine(appDataPath, DataFolderName);

            if(!Directory.Exists(dataFolderPath))
            {
                Directory.CreateDirectory(dataFolderPath);           
            }

            return dataFolderPath;
        }
        public static String InitializePath(string fileName)
        {
            if (!Directory.Exists(DataFolderPath))
            {
                InitializeDataFolderPath();
            }

            string filePath = Path.Combine(DataFolderPath, fileName);

            return filePath;
        }
    }
}
