namespace Automata.IO;

public interface IFile : IO
{
    IDirectory Directory { get; }
    string Name { get; }
}

public sealed class File : IFile
{
    public IDirectory Directory { get; }
    public string Name { get; }

    public File(IDirectory directory, string name)
    {
        Directory = directory;
        Name = name;
    }

    public string Path => System.IO.Path.GetFullPath(Directory.Path + "/" + Name);
}