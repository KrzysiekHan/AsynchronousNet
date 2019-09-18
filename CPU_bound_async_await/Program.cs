using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_bound_async_await
{
    class Program
    {
        static void Main(string[] args)
        {
            var calculateSomething = CalculateSomethingAsync(70);
            DoSomethingSynchronous();

            calculateSomething.Wait();
            Console.ReadLine();
        }

        private static void DoSomethingSynchronous()
        {
            Console.WriteLine("Doing some synchronous work");
        }

        static async Task<float> CalculateSomethingAsync(float value)
        {
            Console.WriteLine("CPU asynchronous task");
            var result = await Task.Run(() => value * 1.2f);
            Console.WriteLine($"Finished Task. Total of ${value} after calculation is ${result} ");
            return result;
        }
    }
}
