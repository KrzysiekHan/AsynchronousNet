using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskPlayground
{
    /// <summary>
    ///  Testy w oparciu o dokumentację microsoftu
    /// </summary>
    public class MsDocsTests
    {
        public Task ConfigureAwaitMethod()
        {
            Task sthToDo = new Task(
                () => {
                    Console.WriteLine("start taska czekam 5s");
                    Random rnd = new Random();
                    DateTime[] dates = new DateTime[100];
                    Byte[] buffer = new byte[8];
                    int ctr = dates.GetLowerBound(0);
                    while (ctr <= dates.GetUpperBound(0))
                    {
                        rnd.NextBytes(buffer);
                        long ticks = BitConverter.ToInt64(buffer, 0);
                        if (ticks <= DateTime.MinValue.Ticks | ticks >= DateTime.MaxValue.Ticks)
                            continue;
                        dates[ctr] = new DateTime(ticks);
                        ctr++;
                    }
                    Console.WriteLine(dates[rnd.Next(0, dates.GetUpperBound(0))]);
                    Console.WriteLine("task zakończony");
                }
            );
            sthToDo.ConfigureAwait(false); //standardowo task zwraca kontynuację do oryginalnego kontekstu tutaj zakładamy że nie jest to potrzebne
            Console.WriteLine("kontynuacja w oryginalnym kontekście");
            return sthToDo;
        }
    }
}
