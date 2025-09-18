using System.Threading.Tasks;

namespace LibSharpProtocol.Core.Async;

public class AsyncSignal<T>
{
    public void Signal(T result) => _tcs.TrySetResult(result);
    public Task<T> Await() => _tcs.Task;
    
    readonly TaskCompletionSource<T> _tcs = new();
}