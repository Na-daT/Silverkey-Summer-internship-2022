using System.Text;
using System.Text.Json;

namespace RecipesExercise1
{
    internal static class JsonHandler
    {
        public static async Task<string> SerializeAsync<T>(T obj)
        {
            try
            {
                string json = string.Empty;
                using (var stream = new MemoryStream())
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true
                    };

                    await JsonSerializer.SerializeAsync(stream, obj, options);
                    stream.Position = 0;
                    using (var reader = new StreamReader(stream))
                    {
                        json = await reader.ReadToEndAsync();
                    }
                }
                return json;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return String.Empty;
            }
        }

        public static async Task<T?> DeserializeAsync<T>(string jsonText)
        {
            try
            {
                using var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonText));
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                T? objects =
                    await JsonSerializer.DeserializeAsync<T>(stream, options);
                return objects;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return (T)new object();
            }
        }

    }
}