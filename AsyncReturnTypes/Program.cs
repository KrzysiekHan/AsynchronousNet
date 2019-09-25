using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncReturnTypes
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(ShowTodaysInfo().Result);
            Console.WriteLine("---------------------");
            DisplayCurrentInfo().Wait();
            Console.ReadLine();
        }

        private static async Task<string> ShowTodaysInfo()
        {
            string ret = $"Today is {DateTime.Today:D}\n" +
                "Todays hours of leisure: " + $"{await GetLeisureHours()}";
            return ret;
        }

        static async Task<int> GetLeisureHours()
        {
            var today = await Task.FromResult<string>(DateTime.Now.DayOfWeek.ToString());
            int leisureHours;
            if (today.First() == 'S')
            {
                leisureHours = 16;
            }
            else
            {
                leisureHours = 5;
            }
            return leisureHours;
        }

        static async Task DisplayCurrentInfo()
        {
            await WaitAndApologize();
            Console.WriteLine($"Today is {DateTime.Now:D}");
            Console.WriteLine($"The current time is {DateTime.Now.TimeOfDay:t}");
            Console.WriteLine("The current temperature is 76 degrees.");
        }

        static async Task WaitAndApologize()
        {
            await Task.Delay(2000);
            Console.WriteLine("Im sorry for delay...\n");
        }
    }
}
