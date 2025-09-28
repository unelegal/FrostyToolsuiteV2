using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Util;

namespace FrostyEditor.Utilities;

public class AsyncSemaphore
{
    private readonly SemaphoreSlim m_semaphore;

    public AsyncSemaphore(int initialCount, int maxCount)
    {
        m_semaphore = new(1, 1);
    }

    public async Task<DisposableLock> WaitAsync()
    {
        await m_semaphore.WaitAsync();
        return new DisposableLock(m_semaphore);
    }

    public async Task<DisposableLock> WaitAsync(CancellationToken cancellationToken)
    {
        await m_semaphore.WaitAsync(cancellationToken);
        return new DisposableLock(m_semaphore);
    }

    public async Task<DisposableLock?> WaitAsync(int millisecondsTimeout, CancellationToken cancellationToken)
    {
        bool entered = await m_semaphore.WaitAsync(millisecondsTimeout, cancellationToken);
        if (!entered)
        {
            return null;
        }

        return new DisposableLock(m_semaphore);
    }

    public async Task<DisposableLock?> WaitAsync(int millisecondsTimeout)
    {
        bool entered = await m_semaphore.WaitAsync(millisecondsTimeout);
        if (!entered)
        {
            return null;
        }

        return new DisposableLock(m_semaphore);
    }

    public sealed class DisposableLock(SemaphoreSlim inSemaphore) : IDisposable
    {
        public void Dispose()
        {
            inSemaphore.Release();
        }
    }
}