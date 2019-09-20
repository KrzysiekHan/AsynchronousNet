using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MicrosoftDocs
{
    class Program
    {
        static void Main(string[] args)
        {
            ExampleWithAwait();
            Console.ReadLine();
            ExampleWithoutAwait();
            Console.ReadLine();
            ExampleWhenAll();
            Console.ReadLine();
            ExampleWhenAny();
            Console.ReadLine();
            ExampleAny();
            Console.ReadLine();
            AccessTheWebAsync();
            Console.ReadLine();
        }

        static async Task AccessTheWebAsync()
        {
            // You need to add a reference to System.Net.Http to declare client.
            using (HttpClient client = new HttpClient())
            {
                Task<string> getStringTask = client.GetStringAsync("https://docs.microsoft.com");
                string urlContents = await getStringTask;
                Console.WriteLine("Microsoft docs chars count" + urlContents.Length); 
            }
        }
        //równoległe uruchomienie wszystkich tasków
        static async Task ExampleAny()
        {
            Console.WriteLine("ExampleAny");
            var intTask = GetSomeIntAsync();
            var stringTask = GetSomeStringAsync();
            var charTask = GetSomeCharAsync();

            var allTasks = new List<Task> { intTask, stringTask, charTask };

            while (allTasks.Any())
            {
                Task finished = await Task.WhenAny(allTasks);
                if (finished == intTask)
                {
                    Console.WriteLine("int task");
                } else if (finished == stringTask)
                {
                    Console.WriteLine("string task");
                } else if (finished == charTask){
                    Console.WriteLine("char task");
                }
                allTasks.Remove(finished);
            }
        }

        //oczekiwanie na zakończenie któregokolwiek z tasków
        static async Task ExampleWhenAny()
        {
            Console.WriteLine("ExampleWhenAny");
            var k = GetSomeIntAsync();
            Console.WriteLine("after GetSomeIntAsync");
            var s = GetSomeStringAsync();
            Console.WriteLine("after GetSomeStringAsync");
            await Task.WhenAny(k, s);
            Console.WriteLine("after WhenAny");
        }

        //oczekiwanie na zakończenie wszystkich tasków
        static async Task ExampleWhenAll()
        {
            Console.WriteLine("ExampleWhenAll");
            var k = GetSomeIntAsync();
            Console.WriteLine("after GetSomeIntAsync");
            var s = GetSomeStringAsync();
            Console.WriteLine("after GetSomeStringAsync");
            Task.WhenAll(k,s);
            Console.WriteLine("after WhenAll");
        }

        static void ExampleWithoutAwait()
        {
            Console.WriteLine("ExampleWithoutAwait");
            var k = GetSomeIntAsync();
            Console.WriteLine("after GetSomeIntAsync");
            var s = GetSomeStringAsync();
            Console.WriteLine("after GetSomeStringAsync");
        }

        static async Task ExampleWithAwait()
        {
            Console.WriteLine("ExampleWithAwait");
            Task<int> k = GetSomeIntAsync();
            Console.WriteLine("after GetSomeIntAsync");
            string s = await GetSomeStringAsync();
            Console.WriteLine("after GetSomeStringAsync");
        }

        static async Task<int> GetSomeIntAsync()
        {
            Console.WriteLine("start GetSomeIntAsync");
            int result = 3;
            await Task.Delay(100);
            Console.WriteLine("end GetSomeIntAsync");
            return result;        
        }

        static async Task<string> GetSomeStringAsync()
        {
            Console.WriteLine("start GetSomeStringAsync");
            string result = "test";
            await Task.Delay(200);
            Console.WriteLine("end GetSomeStringAsync");
            return result;
        }

        static async Task<char> GetSomeCharAsync()
        {
            Console.WriteLine("start GetSomeCharAsync");
            char result = 'k';
            await Task.Delay(400);
            Console.WriteLine("end GetSomeCharAsync");
            return result;
        }


    }

    
}
