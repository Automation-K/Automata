namespace Automata.IO;

public interface IDirectory : IO
{
    IDirectory? Root { get; }
}

public sealed class Directory : IDirectory
{
    public string Path { get; }
    public IDirectory? Root { get; }

    public Directory(string path)
    {
        Path = IO.CorrectSlash(path + "/");
    }

    public Directory(IDirectory root, string name)
    {
        Root = root;
        Path = IO.CorrectSlash(Root.Path + "/" + name);
    }
}