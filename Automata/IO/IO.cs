using System.IO.Abstractions;

namespace Automata.IO;

public interface IO
{
    public string Path { get; }
}

public static class IOShared
{
    static IOShared()
    {
        FileSystem = null;
    }
    
    private static IFileSystem _fileSystem = null!;
    public static IFileSystem FileSystem
    {
        get => _fileSystem;
        set
        {
            if (value == null)
            {
                value = new FileSystem();
            }
            _fileSystem = value;
        }
    }
}
