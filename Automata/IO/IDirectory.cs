namespace Automata.IO;

public interface IDirectory : IO
{
}

public sealed class Directory : IDirectory
{
    public IDirectory? Root { get; }

    public Directory(string path)
    {
        Path = System.IO.Path.GetFullPath(path + "/");
    }

    public Directory(IDirectory root, string name)
    {
        Root = root;
        Path = System.IO.Path.GetFullPath(Root.Path + "/" + name);
    }

    public string Path { get; }
}