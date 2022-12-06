namespace Automata.IO.Containers;

public static class DirectoryExtensions
{
    public static void Create(this IDirectory directory) => System.IO.Directory.CreateDirectory(directory.Path);
    public static bool Exist(this IDirectory directory) => System.IO.Directory.Exists(directory.Path);
    public static IDirectory Directory(this IDirectory directory, string name)
        => new Directory(directory, name);

    public static IDirectory Directory(this IDirectory directory, params string[] names)
    {
        var root = directory;
        foreach (var name in names) root = root.Directory(name);
        return root;
    }
    
    public static IFile File(this IDirectory directory, string name)
        => new File(directory, name);
    
    public static async Task<List<IFile>> Files(this IDirectory dir)
    {
        var direcotryInfo = new DirectoryInfo(dir.Path);

        return direcotryInfo.GetFiles().Select(x => new File(dir, x.Name)).ToList<IFile>();
    }

    public static async Task<List<IDirectory>> Directories(this IDirectory dir)
    {
        var dirInfo = new DirectoryInfo(dir.Path);

        return dirInfo.GetDirectories().Select(x=> new Directory(dir,x.Name)).ToList<IDirectory>();
    }

    public static async Task Copy(this IDirectory root, IDirectory destination)
    {
        foreach (var initialDirectory in await root.Directories())
        {
            var copiedDirectory = destination.Directory(new DirectoryInfo(initialDirectory.Path).Name);
            copiedDirectory.Create();

            //Recursively clone the directory
            await Copy(initialDirectory, copiedDirectory);
        }

        foreach (var file in await root.Files())
        {
            await file.Copy(destination);
        }
    }
    
    public static async Task<IFile?> Find(this IDirectory root, string pattern)
    {
        var files = System.IO.Directory.GetFiles(root.Path, pattern, SearchOption.AllDirectories);
        if (files.Length == 0)
            return null;
        return new File(new Directory(Path.GetDirectoryName(files[0])), Path.GetFileName(files[0]));
    }
}