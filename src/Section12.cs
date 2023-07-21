namespace MultiThread
{
    public class Section12
    {
        // Dead Lock example

        private object lock1 = new object();
        private int x = 0;

        private object lock2 = new object();

        private int y = 0;

        public void DeadLockStartUp()
        {
            new Thread(() =>
            {
                lock (lock1)
                {
                    Thread.Sleep(1000);

                    Console.WriteLine("acquired lock2");

                    lock (lock2)
                    {
                        Console.WriteLine("Thread 1");
                    }
                }
            }).Start();

            new Thread(() =>
            {
                lock (lock2)
                {
                    Thread.Sleep(1000);

                    Console.WriteLine("acquired lock1");
                    lock (lock1)
                    {
                        Console.WriteLine("Thread 2");
                    }
                }
            }).Start();
        }

        // prevent dead lock example

        private object lock3 = new object();
        private object lock4 = new object();


        public void PreventDeadLockStartUp()
        {
            new Thread(() =>
            {
                lock (lock1)
                {
                    Thread.Sleep(1000);

                    Console.WriteLine("acquired lock2");

                    lock (lock2)
                    {
                        Console.WriteLine("Thread 1");
                    }
                }
            }).Start();

            new Thread(() =>
            {
                lock (lock1)
                {
                    Thread.Sleep(1000);

                    Console.WriteLine("acquired lock1");
                    lock (lock2)
                    {
                        Console.WriteLine("Thread 2");
                    }
                }
            }).Start();
        }

        // prevent dead lock with monitor with timeout example

        private object lock5 = new object();
        private object lock6 = new object();


        public void AvoidDeadLockWithMonitorStartUp()
        {
            var thread1 = new Thread(() =>
            {
                lock (lock1)
                {
                    Thread.Sleep(1000);
                    int oldX = x;
                    x = 1;
                    Console.WriteLine("acquired lock2");

                    if (Monitor.TryEnter(lock2, 1000))
                    {
                        try
                        {
                            y = 1;
                            Console.WriteLine("Thread 1");
                        }
                        finally
                        {
                            Monitor.Exit(lock2);
                        }
                    }
                    else
                    {
                        x = oldX;
                        Console.WriteLine("Thread 1 failed to acquire lock2");
                    }
                }
            });
            thread1.Start();

            var thread2 = new Thread(() =>
            {
                lock (lock2)
                {
                    Thread.Sleep(1000);
                    int oldY = y;
                    y = 2;
                    Console.WriteLine("acquired lock1");

                    if (Monitor.TryEnter(lock1, 1000))
                    {
                        try
                        {
                            x = 2;
                            Console.WriteLine("Thread 2");
                        }
                        finally
                        {
                            Monitor.Exit(lock1);
                        }
                    }
                    else
                    {
                        y = oldY;
                        Console.WriteLine("Thread 2 failed to acquire lock1");
                    }
                }
            });
            thread2.Start();

            thread1.Join();
            thread2.Join();

            Console.WriteLine($"x = {x}, y = {y}");


        }
    }
}