namespace MultiThread
{
    public class Section11
    {
        // Thread-Specific Storage

        private static ThreadLocal<int> threadLocal = new ThreadLocal<int>(() => 0);

        public void StartUp()
        {
            new Thread(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    threadLocal.Value++;
                    Console.WriteLine($"Thread A : {threadLocal.Value}");
                    Thread.Sleep(0);
                }
            }).Start();

            new Thread(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    threadLocal.Value++;
                    Console.WriteLine($"Thread B : {threadLocal.Value}");
                    Thread.Sleep(0);
                }
            }).Start();

            Console.ReadKey();
        }

    }
}