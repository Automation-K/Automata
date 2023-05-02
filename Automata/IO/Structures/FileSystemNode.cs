namespace Automata.IO.Structures;

public class FileSystemNode
{
    public IO IO { get; private set; }

    public string Name
    {
        get
        {
            if (IO is IFile file)
            {
                return file.Prefix();
            }else if (IO is IDirectory directory)
            {
                return directory.Name();
            }

            throw new InvalidOperationException();
        }
    }
    
    public List<FileSystemNode> Nodes { get; private set; }

    public FileSystemNode(IO io)
    {
        IO = io;
        Nodes = new List<FileSystemNode>();
    }

    public async Task DoTreeAsync()
    {
        if (IO is IFile file)
            return;

        if (IO is IDirectory directory)
        {
            await foreach (var eFile in directory.EnumerateFiles())
            {
                // Добавляем файлы, создав их деревья
                var node = new FileSystemNode(eFile);
                await node.DoTreeAsync();
                Nodes.Add(node);
            }
            
            foreach( var dir in await directory.Directories())
            {
                var node = new FileSystemNode(dir);
                await node.DoTreeAsync();
                Nodes.Add(node);
            }
            return;
        }

        throw new InvalidOperationException();
    }
}