using Automata.Extensions.Common;
using JetBrains.Lifetimes;
using Microsoft.Extensions.Logging;

namespace Automata.Flows;

public abstract class Flow
{
    protected readonly LifetimeDefinition LifetimeDefinition;

    protected readonly ILogger Logger;

    public Lifetime Lifetime => LifetimeDefinition.Lifetime;
    
    protected Flow(ILogger logger, Lifetime? lifetime = null)
    {
        LifetimeDefinition = lifetime is null ? new LifetimeDefinition() : Lifetime.Define(lifetime.Value);
        LifetimeDefinition.Lifetime.OnTermination(() => Task.Run(Termination).GetAwaiter().GetResult());

        Logger = logger;
    }

    protected abstract Task Start();
    protected abstract Task Termination();

    public async Task Ini()
    {
        if (LifetimeDefinition.Status != LifetimeStatus.Alive)
            return;
        
        await Start();
    }

    public async Task Terminate()
    {
        if (LifetimeDefinition.Status != LifetimeStatus.Alive)
            return;
        
        await Task.Run(()=>LifetimeDefinition.Terminate())
            .HandleExceptions(Logger, "flow-exception-logger");
    }
}