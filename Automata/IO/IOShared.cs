using System.IO.Abstractions;

namespace Automata.IO;
// ReSharper disable once InconsistentNaming
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