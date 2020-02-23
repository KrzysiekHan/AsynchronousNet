using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskPlayground
{
    public class AsyncFactory
    {
        /// <summary>
        /// Wykorzystanie klasy <c>TaskCompletionSource</c> z przypisaniem rezultatu poprzez metodę <c>SetResult</c>
        /// </summary>
        public static Task<int> GetIntAsync()
        {
            var tcs = new TaskCompletionSource<int>();
            var timer = new System.Timers.Timer(2000);
            timer.AutoReset = false;
            timer.Elapsed += (s, e) =>
            {
                tcs.SetResult(10);
                timer.Dispose();
            };
            timer.Start();
            return tcs.Task;
        }

        /// <summary>
        /// Przykład wykorzystania <c>CancellationToken</c> do anulowania długo trwających zadań
        /// </summary>
        public static Task<int> GetIntAsyncWithCancellation(CancellationToken token = default(CancellationToken))
        {
            var tcs = new TaskCompletionSource<int>();

            if (token.IsCancellationRequested)
            {
                tcs.SetCanceled();
                return tcs.Task;
            }

            var timer = new System.Timers.Timer(2000);
            timer.AutoReset = false;
            timer.Elapsed += (s, e) =>
            {
                tcs.TrySetResult(10);
                timer.Dispose();
            };

            if (token.CanBeCanceled)
            {
                token.Register(() => {
                    tcs.TrySetCanceled();
                    timer.Dispose();
                });
            }
            timer.Start();
            return tcs.Task;
        }

    }
}
