using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MicrosoftDocs
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("program start");
            Task<int> k = GetSomeIntAsync();
            Console.WriteLine("After task");
            Console.ReadLine();
        }

        static async Task<int> GetSomeIntAsync()
        {
            Console.WriteLine("Simulate calculation...");
            int result = 3;
            await Task.Delay(100);
            Console.WriteLine("Calculation done");
            return result;
            
        }


    }

    
}
