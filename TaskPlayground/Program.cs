using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            MsDocsTests mstests = new MsDocsTests();
            Task task = mstests.ConfigureAwaitMethod();
            task.Start();
            task.Wait();
            if (false)
            {
                ConsumeGetIntWithCancellation();
                TaskWhenAllExample();
                ConsumeGetIntAsyncMethod();
                FromResultExample();
            }
            Console.ReadLine();

        }


        /// <summary>
        /// Przykład anulowania długotrwałej operacji przy użyciu <c>CancellationToken</c>.
        /// Sprawdź implementację <see cref="AsyncFactory.GetIntAsyncWithCancellation(CancellationToken)"/> 
        /// </summary>
        static void ConsumeGetIntWithCancellation()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(1000);//task trwa 2000
            AsyncFactory.GetIntAsyncWithCancellation(cts.Token).ContinueWith((task)=> {
                cts.Dispose();//usuń token ponieważ task został ukończony
                if(task.Status == TaskStatus.RanToCompletion)
                {
                    Console.WriteLine(task.Result);
                }
                else if (task.Status == TaskStatus.Canceled) 
                {
                    Console.WriteLine("Task cancelled");
                }
                else
                {
                    Console.WriteLine(
                    "An error has been occurred. Details:");
                    Console.WriteLine(
                    task.Exception.InnerException.Message);
                }
            });

            Console.ReadLine();
        }

        /// <summary>
        /// Przykład wykorzystania metody FromResult do zwrócenia ukończonego Taska,
        /// wykorzystywane w przypadku gdy dane są już gotowe (np cache'owane)
        /// </summary>
        static void FromResultExample()
        {
            var intTask = GetIntAsyncDummy();
            if (intTask.IsCompleted)
            {
                Console.WriteLine("Completed instantly: {0}",intTask.Result);
            }
            else
            {
                intTask.ContinueWith((task) =>
                {
                    if (task.Status == TaskStatus.RanToCompletion)
                    {
                        Console.WriteLine(task.Result);
                    }
                    else if (task.Status == TaskStatus.Canceled)
                    {
                        Console.WriteLine("The task has been canceled.");
                    }
                    else
                    {
                        Console.WriteLine("An error has been occurred. Details:");
                        Console.WriteLine(task.Exception.InnerException.Message);
                    }
                });
            }
            Console.ReadLine();
        }
        static Task<int> GetIntAsyncDummy()
        {
            return Task.FromResult(10);
        }

        /// <summary>
        /// Przykład uruchomienia kilku tasków i zrealizowania callback'a w ContinueWith
        /// </summary>
        static void TaskWhenAllExample()
        {
            var httpClient = new HttpClient();
            Task<string> twitterTask = httpClient.GetStringAsync("http://twitter.com");
            var httpClient2 = new HttpClient();
            Task<string> googleTask = httpClient2.GetStringAsync("http://google.com");
            Console.WriteLine("Before WhenAll");
            Task<string[]> task = Task.WhenAll(twitterTask, googleTask);
            Console.WriteLine("Before ContinueWith");           
            task.ContinueWith(stringArray =>
            {
                //tutaj taski są zakończone a ich zwracana wartość przekazana jest do lambdy
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    for (int i = 0; i < stringArray.Result.Length; i++)
                    {
                        Console.WriteLine(stringArray.Result[i].Substring(0, 100));
                    }
                }
                else if (task.Status == TaskStatus.Canceled)
                {
                    Console.WriteLine("Task has been canceled. ID: {0}", task.Id);
                }
                else
                {
                    Console.WriteLine("An error has been occuerred.");
                    foreach (var ex in task.Exception.InnerExceptions)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                Console.WriteLine("end of task result");
            });
            Console.WriteLine("program end...");
            Console.ReadLine();
        }

        /// <summary>
        /// Wykorzystanie klasy <c>TaskCompletionSource</c> z przypisaniem rezultatu poprzez metodę <c>SetResult</c>
        /// Sprawdź implementację <see cref="AsyncFactory.GetIntAsync()"/> 
        /// </summary>
        static void ConsumeGetIntAsyncMethod()
        {
            Console.WriteLine("ConsumeGetIntAsyncMethod start");
            AsyncFactory.GetIntAsync().ContinueWith((task)=> {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    Console.WriteLine(task.Result);
                }
                else if (task.Status == TaskStatus.Canceled)
                {
                    Console.WriteLine("Task cancelled");
                }
                else
                {
                    Console.WriteLine("Error occured");
                    Console.WriteLine(task.Exception.InnerException.Message);
                }
            });
            Console.WriteLine("ConsumeGetIntAsyncMethod end");
            Console.ReadLine();
        }
    }
}
