using System.Collections.Concurrent;

namespace MultiThread
{
    public class Section8
    {
        // Thread-Per-Message Pattern

        public void StartUp()
        {
            Console.WriteLine("Process Start!");

            //Thread 오브젝트 만들기
            List<Thread> threads = new List<Thread>();


            var thread1 = new Thread(DoWork);
            thread1.Name = "Brown";
            thread1.Start("Send email");

            var thread2 = new Thread(DoWork);
            thread2.Name = "Bob";
            thread2.Start("Write DB");

            var thread3 = new Thread(DoWork);
            thread3.Name = "Alice";
            thread3.Start("Massive Calculation");

            thread1.Join();
            thread2.Join();
            thread3.Join();

            Console.WriteLine("All done!");
        }

        private void DoWork(object? name)
        {
            Thread.Sleep(2000);
            Console.WriteLine(Thread.CurrentThread.Name + " : " + name);

        }
    }
}