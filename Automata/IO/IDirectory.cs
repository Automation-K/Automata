namespace Automata.IO;

public interface IDirectory : IO
{
}

public sealed class Directory : IDirectory
{
    public IDirectory? Root { get; }

    public Directory(string path)
    {
        Path = IOShared.FileSystem.Path.GetFullPath(path + "/");
    }

    public Directory(IDirectory root, string name)
    {
        Root = root;
        Path = IOShared.FileSystem.Path.GetFullPath(Root.Path + "/" + name);
    }

    public string Path { get; }
}