using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReactiveUI;

namespace FrostyEditor.Utilities;

public static class Async
{
        /// <summary>
        /// Runs a task on the RxApp.TaskpoolScheduler
        /// </summary>
        /// <typeparam name="TResult">The type of the result returned by the task.</typeparam>
        /// <param name="task">The task to run</param>
        /// <returns>The result of the task.</returns>
        public static async Task<TResult> RunInBackground<TResult>(Func<Task<TResult>> task)
        {
            return await Observable.FromAsync(task).SubscribeOn(RxApp.TaskpoolScheduler);
        }

        /// <summary>
        /// Runs a task on the RxApp.TaskpoolScheduler
        /// </summary>
        /// <param name="task">The task to run</param>
        public static async Task RunInBackground(Func<Task> task)
        {
            await Observable.FromAsync(task).SubscribeOn(RxApp.TaskpoolScheduler);
        }

        /// <summary>
        /// Runs a task on the RxApp.MainThreadScheduler
        /// </summary>
        /// <typeparam name="TResult">The type of the result returned by the task.</typeparam>
        /// <param name="task">The task to run</param>
        /// <returns>The result of the task.</returns>
        public static async Task<TResult> RunOnUI<TResult>(Func<Task<TResult>> task)
        {
            return await Observable.FromAsync(task).SubscribeOn(RxApp.MainThreadScheduler);
        }

        /// <summary>
        /// Runs a task on the RxApp.MainThreadScheduler
        /// </summary>
        /// <param name="task">The task to run</param>
        public static async Task RunOnUI(Func<Task> task)
        {
            await Observable.FromAsync(task).SubscribeOn(RxApp.MainThreadScheduler);
        }
}