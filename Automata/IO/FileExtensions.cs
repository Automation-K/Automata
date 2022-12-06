namespace Automata.IO;

public static class FileExtensions
{
    public static void Create(this IFile file)
    {
        if (!file.Directory.Exist())
            file.Directory.Create();
        System.IO.File.Create(file.Path).Close();
    }

    public static bool Exist(this IFile file) => System.IO.File.Exists(file.Path);

    public static async Task<string> ReadAsync(this IFile file)
    {
        if (file.Exist())
            return await System.IO.File.ReadAllTextAsync(file.Path); ;
        return string.Empty;
    }

    public static async Task<string> ReadOrCreate(this IFile file)
    {
        if (file.Exist())
            return await System.IO.File.ReadAllTextAsync(file.Path);
        file.Create();
        return await file.ReadAsync();
    }

    public static async Task Copy(this IFile file, IDirectory destination)
    {
        System.IO.File.Copy(file.Path, destination.File(Path.GetFileName(file.Path)).Path, true);
    }

    public static string NameWithoutExtension(this IFile file)
    {
        return Path.GetFileNameWithoutExtension(file.Name);
    }

    public static async Task WriteAsync(this IFile file, string text)
    {
        file.Create();
        await System.IO.File.WriteAllTextAsync(file.Path,text);
    }

    public static FileInfo Info(this IFile file)
    {
        return new FileInfo(file.Path);
    }

    public static async Task Delete(this IFile file)
    {
        System.IO.File.Delete(file.Path);
    }
}