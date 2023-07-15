using System.Collections.Concurrent;

namespace MultiThread
{
    public class Section6
    {
        // Producer Consumer Pattern
        // https://en.wikipedia.org/wiki/Guarded_suspension

        private bool shouldStop = false;

        private ConcurrentQueue<string> requests = new ConcurrentQueue<string>();

        public void StartUp()
        {
            Console.WriteLine("Process Start!");

            //Thread 오브젝트 만들기
            List<Thread> threads = new List<Thread>();

            for (int i = 0; i < 3; i++)
            {
                var thread = new Thread(Produce);
                thread.Name = "Producer" + i;
                threads.Add(thread);
            }

            for (int i = 0; i < 3; i++)
            {
                var thread = new Thread(Consume);
                thread.Name = "Consumer" + i;
                threads.Add(thread);
            }

            //Thread 시작
            foreach (var thread in threads)
            {
                thread.Start();
            }

            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();

            Volatile.Write(ref shouldStop, true);

            //Thread 종료 대기
            foreach (var thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine("All done!");
        }

        private void Produce()
        {
            while (Volatile.Read(ref shouldStop) == false)
            {
                var request = DateTime.Now.ToString();
                requests.Enqueue(request);
                Console.WriteLine(Thread.CurrentThread.Name + " : " + request);
                Thread.Sleep(100);
            }
        }

        private void Consume()
        {
            while (Volatile.Read(ref shouldStop) == false)
            {

                if (requests.TryDequeue(out string? request) == true)
                {
                    Console.WriteLine(Thread.CurrentThread.Name + " : " + request);
                }

                Thread.Sleep(100);
            }
        }
    }
}