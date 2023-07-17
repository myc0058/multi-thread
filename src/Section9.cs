using System.Collections.Concurrent;

namespace MultiThread
{
    public class Section9
    {
        // Worker Thread Pattern
        private BlockingCollection<string> taskQueue = new BlockingCollection<string>(10);

        public Section9()
        {
        }

        public void StartUp()
        {
            Console.WriteLine("Process Start!");

            var workerThreads = new List<Thread>();

            for (int i = 0; i < 3; i++)
            {
                var thread = new Thread(() =>
                {
                    while (taskQueue.IsAddingCompleted == false)
                    {
                        try
                        {
                            var task = taskQueue.Take();
                            Console.WriteLine(Thread.CurrentThread.Name + " : " + task);
                        }
                        catch
                        {
                            break;
                        }
                    }
                });
                thread.Name = $"Worker Thread {i + 1}";
                thread.Start();
                workerThreads.Add(thread);
            }

            var orderThreads = new List<Thread>();

            for (int i = 0; i < 3; i++)
            {
                string task = string.Empty;

                switch (i)
                {
                    case 0: task = "Send email"; break;
                    case 1: task = "Write DB"; break;
                    case 2: task = "Massive Calculation"; break;
                }

                var thread = new Thread(() =>
                {
                    while (taskQueue.IsAddingCompleted == false)
                    {
                        try
                        {
                            taskQueue.Add(task);
                            Console.WriteLine(Thread.CurrentThread.Name + " : " + task);
                        }
                        catch
                        {
                            break;
                        }
                    }
                });
                thread.Name = $"Order Thread {i + 1}";
                thread.Start();
                orderThreads.Add(thread);
            }

            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();

            //Volatile.Write(ref shouldStop, true);

            taskQueue.CompleteAdding();

            foreach (var thread in workerThreads)
            {
                thread.Join();
            }

            foreach (var thread in orderThreads)
            {
                thread.Join();
            }

            Console.WriteLine("All done!");
        }
    }
}