namespace Automata.IO;

public static class DirectoryExtensions
{
    public static void Create(this IDirectory directory) => IOShared.FileSystem.Directory.CreateDirectory(directory.Path);
    public static bool Exist(this IDirectory directory) => IOShared.FileSystem.Directory.Exists(directory.Path);
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

    public static async IAsyncEnumerable<IFile> EnumerateFiles(this IDirectory dir)
    {
        var directoryInfo = new DirectoryInfo(dir.Path);
        foreach (var enumerateFile in directoryInfo.EnumerateFiles())
        {
            yield return dir.File(enumerateFile.Name);
        }
    }

    public static async Task<List<IFile>> FilesDeep(this IDirectory root)
    {
        var files = new List<IFile>();
        foreach (var dir in await root.Directories())
        {
            files.AddRange(await FilesDeep(dir));
        }

        var rootFiles = await root.Files();
        files.AddRange(rootFiles);
        return files;
    }

    public static async Task<List<IFile>> FilesDeep(this IDirectory root, Predicate<IFile> predicate)
    {
        var files = new List<IFile>();
        foreach (var dir in await root.Directories())
        {
            files.AddRange(await FilesDeep(dir, predicate));
        }

        var rootFiles = await root.Files();
        files.AddRange(rootFiles.Where(x => predicate(x)));
        return files;
    }

    public static async IAsyncEnumerable<IFile> EnumerateFilesDeep(this IDirectory root)
    {
        await foreach (var enumerateFile in root.EnumerateFiles())
            yield return enumerateFile;
        
        // Проходим по всем подпапкам
        foreach (var directory in await root.Directories())
            await foreach (var file in directory.EnumerateFilesDeep())
                yield return file;
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
        var files = IOShared.FileSystem.Directory.GetFiles(root.Path, pattern, SearchOption.AllDirectories);
        if (files.Length == 0)
            return null;
        return new File(new Directory(IOShared.FileSystem.Path.GetDirectoryName(files[0])!),
            IOShared.FileSystem.Path.GetFileName(files[0]));
    }
    
    public static IDirectory Join(this IDirectory directory, IRelativeDirectory relativeDirectory)
    {
        var newDir = new Directory($"{directory.Path}/{relativeDirectory.Path}/");
        return newDir;
    }
    
    public static IFile Join(this IDirectory directory, IRelativeFile relativeFile)
    {
        var newDir = directory.Join(relativeFile.RelativeRoot);
        return newDir.File(relativeFile.Name);
    }

    public static bool Compare(this IDirectory original, IDirectory? compare)
    {
        if (compare is null)
            return false;

        if (original.Path == compare.Path)
            return true;

        return false;
    }

    public static DirectoryInfo Info(this IDirectory directory)
    {
        return new DirectoryInfo(directory.Path);
    }

    public static IDirectory ToDirectory(this DirectoryInfo info) => new Directory(info.FullName);

    public static string Name(this IDirectory directory) => directory.Info().Name;
    
    public static IDirectory? Parent(this IDirectory directory)
    {
        var parent = directory.Info().Parent?.ToDirectory();
        return parent;
    }

    public static bool HasInheritance(this IDirectory a, IDirectory? b)
    {
        if (b is null)
            return false;

        var s = a.Path.Length <= b.Path.Length ? a : b;
        var l = s == a ? b : a;

        var sPath = s.Path;

        IDirectory? parent = null;
        parent = l.Parent();
        while (parent is not null)
        {
            if (parent.Path == sPath)
                return true;
            parent = parent.Parent();
        }
        return false;
    }
}