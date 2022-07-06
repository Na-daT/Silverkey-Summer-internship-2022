
public static class FileHandler
{
    public static async Task<string> ReadAsync(string fileName)
    {
        if (!File.Exists(fileName) || new FileInfo(fileName).Length == 0)
        {
            await File.WriteAllTextAsync(fileName, "[]");
        }
        return await File.ReadAllTextAsync(fileName);
    }

    public static async Task<bool> WriteAsync(string fileName, string? json)
    {
        try
        {
            await File.WriteAllTextAsync(fileName, json);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
