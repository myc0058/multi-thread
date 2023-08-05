using System.Diagnostics;

namespace MultiThread
{
    public class Section13
    {
        // spin lock example

        const int N = 100000;
        static Queue<Data> _queue = new Queue<Data>();
        static Queue<Data> _queue2 = new Queue<Data>();

        static object _lock = new Object();
        static SpinLock _spinlock = new SpinLock();

        class Data
        {
            public string Name { get; set; }
            public double Number { get; set; }
        }

        public void StartUp()
        {
            UseLock();
            _queue.Clear();
            UseSpinLock();
        }

        private static void UpdateWithSpinLock(Data d, int i)
        {
            bool lockTaken = false;
            try
            {
                _spinlock.Enter(ref lockTaken);
                _queue.Enqueue(d);
            }
            finally
            {
                if (lockTaken)
                    _spinlock.Exit(false);
            }
        }

        private static void UseSpinLock()
        {

            Stopwatch sw = Stopwatch.StartNew();

            Parallel.Invoke(
                    () =>
                    {
                        for (int i = 0; i < N; i++)
                        {
                            UpdateWithSpinLock(new Data() { Name = i.ToString(), Number = i }, i);
                        }
                    },
                    () =>
                    {
                        for (int i = 0; i < N; i++)
                        {
                            UpdateWithSpinLock(new Data() { Name = i.ToString(), Number = i }, i);
                        }
                    }
                );
            sw.Stop();
            Console.WriteLine("elapsed ms with spinlock: {0}", sw.ElapsedMilliseconds);
        }

        static void UpdateWithLock(Data d, int i)
        {
            lock (_lock)
            {
                _queue2.Enqueue(d);
            }
        }

        private static void UseLock()
        {
            Stopwatch sw = Stopwatch.StartNew();

            Parallel.Invoke(
                    () =>
                    {
                        for (int i = 0; i < N; i++)
                        {
                            UpdateWithLock(new Data() { Name = i.ToString(), Number = i }, i);
                        }
                    },
                    () =>
                    {
                        for (int i = 0; i < N; i++)
                        {
                            UpdateWithLock(new Data() { Name = i.ToString(), Number = i }, i);
                        }
                    }
                );
            sw.Stop();
            Console.WriteLine("elapsed ms with lock: {0}", sw.ElapsedMilliseconds);
        }





    }
}