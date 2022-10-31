using Automata.Extensions.Common;
using JetBrains.Lifetimes;
using Microsoft.Extensions.Logging;

namespace Automata.Flows;

public abstract class Flow
{
    protected readonly LifetimeDefinition LifetimeDefinition;

    protected readonly ILogger Logger;
    
    protected Flow(ILogger logger)
    {
        if (LifetimeDefinition is null)
            LifetimeDefinition = new LifetimeDefinition();

        LifetimeDefinition.Lifetime.OnTermination(() => Termination().GetAwaiter().GetResult());
        
        Logger = logger;
    }

    protected Flow(ILogger logger, Lifetime lifetime) : this(logger)
    {
        LifetimeDefinition = Lifetime.Define(lifetime);
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