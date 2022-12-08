using System.Text.Json;

namespace Automata.IO;

public static partial class FileExtensions
{
    public static async Task WriteJsonAsync<T>(this IFile file, T entity)
    {
        var stream = file.Stream();
        await JsonSerializer.SerializeAsync<T>(stream, entity);
        await stream.DisposeAsync();
    }

    public static async Task<T?> ReadJsonAsync<T>(this IFile file)
    {
        await using var stream = file.Stream();
        var entity = await JsonSerializer.DeserializeAsync<T>(stream);
        return entity;
    }
    
}