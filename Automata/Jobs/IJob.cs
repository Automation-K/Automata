using JetBrains.Lifetimes;

namespace Automata.Jobs;

public interface IJob
{
    JobState? State { get; }
    
    Task Run();
}

public abstract record JobState();
public record JobFailed() : JobState;
public record JobSuccessed() : JobState;
