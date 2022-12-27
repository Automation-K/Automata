namespace Automata.IO;

public static class RelativeFileExtensions
{
    public static string Prefix(this IRelativeFile file)
    {
        return IOShared.FileSystem.Path.GetFileNameWithoutExtension(file.Name);
    }
    
    public static IRelativeFile RelativeFile(this IFile file, IDirectory excludeDirectoryPath)
    {
        return RelativeIO.RelativeFile(excludeDirectoryPath.Path, file.Path);
    }
}