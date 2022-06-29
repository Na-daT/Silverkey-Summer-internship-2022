using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesExercise1
{
    internal class FileHandler
    {
        private static string _path = Path.Combine(Directory.GetCurrentDirectory(), "database.json");

        public static async Task<FileStream> ReadFile()
        {
            FileStream openStream = File.OpenRead(_path);
            return openStream;
        }

        public static async Task<bool> WriteFile(string? json)
        {
            try
            {
                await File.WriteAllTextAsync(_path, json);
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
