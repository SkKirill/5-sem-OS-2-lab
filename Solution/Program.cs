using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Lab2
{
    class Program
    {

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private unsafe static extern uint CreateThread(
        uint* lpThreadAttributes,
        uint dwStackSize,
        ThreadStart lpStartAddress,
        uint* lpParameter,
        uint dwCreationFlags,
        out uint lpThreadId);

        public struct ThreadData
        {
            public int Id;
            public bool IsFinished;
        }

        static unsafe void Main(string[] args)
        {
            ThreadData thread1Data = new ThreadData { Id = 1, IsFinished = false };
            ThreadData thread2Data = new ThreadData { Id = 2, IsFinished = false };

            uint tid;
            CreateThread(null, 0, new ThreadStart(() => PrintNumber(ref thread1Data)), null, 0, out tid);
            CreateThread(null, 0, new ThreadStart(() => PrintNumber(ref thread2Data)), null, 0, out tid);

            Thread.Sleep(3000);
            thread1Data.IsFinished = true;
            thread2Data.IsFinished = true;

            Console.WriteLine("Все потоки завершены.");
            Console.ReadKey();
        }

        public static void PrintNumber(ref ThreadData data)
        {
            try
            {
                while (!data.IsFinished)
                {
                    Console.WriteLine($"Поток {data.Id}");
                    Thread.Sleep(500);
                }
                throw new ThreadInterruptedException();
            }
            catch (ThreadInterruptedException e)
            {
                Console.WriteLine($"Поток {data.Id} был прерван");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Поток {data.Id} произошла ошибка: {e.Message}");
            }
        }
    }
}


