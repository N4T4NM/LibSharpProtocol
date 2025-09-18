using System;
using System.Threading.Tasks;

namespace LibSharpProtocol.Core.Async;

public class AsyncDispatcher
{
    public static void Run(Action action) => _ = new AsyncDispatcher(Task.Run(action));

    void EndTask() => Task.GetAwaiter().GetResult();
    private AsyncDispatcher(Task task)
    {
        Task = task;
        Task.ConfigureAwait(false).GetAwaiter().OnCompleted(EndTask);
    }
    
    private Task Task { get; }
}