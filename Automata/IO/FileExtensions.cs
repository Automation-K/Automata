namespace Automata.IO;

public static partial class FileExtensions
{
    public static string Prefix(this IFile file)
    {
        return IOShared.FileSystem.Path.GetFileNameWithoutExtension(file.Name);
    }

    public static void Create(this IFile file)
    {
        if (!file.Directory.Exist())
            file.Directory.Create();
        IOShared.FileSystem.File.Create(file.Path).Close();
    }

    public static bool Exist(this IFile file) => IOShared.FileSystem.File.Exists(file.Path);

    public static async Task<string> ReadAsync(this IFile file)
    {
        if (file.Exist())
            return await IOShared.FileSystem.File.ReadAllTextAsync(file.Path); ;
        return string.Empty;
    }

    public static async Task<string> ReadOrCreate(this IFile file)
    {
        if (file.Exist())
            return await IOShared.FileSystem.File.ReadAllTextAsync(file.Path);
        file.Create();
        return await file.ReadAsync();
    }

    public static async Task Copy(this IFile file, IDirectory destination)
    {
        IOShared.FileSystem.File.Copy(file.Path, destination.File(
            IOShared.FileSystem.Path.GetFileName(file.Path)).Path, true);
    }

    public static string NameWithoutExtension(this IFile file)
    {
        return IOShared.FileSystem.Path.GetFileNameWithoutExtension(file.Name);
    }

    public static string Extension(this IFile file)
    {
        return IOShared.FileSystem.Path.GetExtension(file.Name);
    }
    
    public static async Task WriteAsync(this IFile file, string text)
    {
        file.Create();
        await IOShared.FileSystem.File.WriteAllTextAsync(file.Path,text);
    }

    public static FileInfo Info(this IFile file)
    {
        return new FileInfo(file.Path);
    }

    public static async Task Delete(this IFile file)
    {
        IOShared.FileSystem.File.Delete(file.Path);
    }

    public static FileStream Stream(this IFile file)
    {
        return new FileStream(file.Path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
    }

    public static IFile Rename(this IFile file, string newNameWithoutExtension)
    {
        var ext = file.Extension();
        var oldPath = file.Path;
        var newFile = file.Directory.File(newNameWithoutExtension + ext);
        IOShared.FileSystem.File.Move(oldPath, newFile.Path);
        return newFile;
    }
}