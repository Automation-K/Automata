namespace Automata.IO;

public static class FileReadWriteExtensions
{
    public static async Task Append(this IFile file, string text)
    {
        file.Create();
        await IOShared.FileSystem.File.AppendAllTextAsync(file.Path, text);
    }
}