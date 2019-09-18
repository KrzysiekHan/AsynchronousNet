using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IO_bound_async_await
{
    class Program
    {
        private const string URL = "https://docs.microsoft.com/en-us/dotnet/csharp/csharp";

        static void Main(string[] args)
        {
            DoSynchronousWork();
            var someTask = DoSomethingAsync();
            DoSynchronousWorkAfterAwait();
            someTask.Wait(); 
            Console.ReadLine();
        }
        public static void DoSynchronousWork()
        {
            Console.WriteLine("1. Some work synchronously");
        }

        static async Task DoSomethingAsync()
        {
            Console.WriteLine("2. Async task has started...");
            await GetStringAsync(); 
        }

        static async Task GetStringAsync()
        {
            using (var httpClient = new HttpClient())
            {
                Console.WriteLine("3. Awaiting the result of GetStringAsync of Http Client...");
                string result = await httpClient.GetStringAsync(URL); 
                Console.WriteLine("4. The awaited task has completed. Let's get the content length...");
                Console.WriteLine($"5. The length of http Get for {URL}");
                Console.WriteLine($"6. {result.Length} character");
            }
        }

        static void DoSynchronousWorkAfterAwait()
        {
            Console.WriteLine("7. Do some unrelated work...");
        }
    }
}
