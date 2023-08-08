namespace MultiThread
{
    public class Section14
    {
        // Lock-free example

        private int loopCount = 100000;
        private SpinLock spinLock = new SpinLock();

        public void StartUp()
        {
            startDumb();
            startWaitFree();
            startLockFree();
            startCASLockFree();
        }

        private void startDumb()
        {
            int sharedValue = 0;

            Action action = new Action(() =>
            {
                for (int i = 0; i < loopCount; i++)
                {
                    sharedValue++;
                }
            });

            Thread thread1 = new Thread(() => action());
            Thread thread2 = new Thread(() => action());

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            Console.WriteLine("Dumb - Final shared value: " + sharedValue);
        }

        private void startWaitFree()
        {
            int sharedValue = 0;

            Action action = new Action(() =>
            {
                for (int i = 0; i < loopCount; i++)
                {
                    Interlocked.Increment(ref sharedValue);
                }
            });

            Thread thread1 = new Thread(() => action());
            Thread thread2 = new Thread(() => action());

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            Console.WriteLine("WaitFree - Final shared value: " + sharedValue);
        }

        private void startLockFree()
        {

            int sharedValue = 0;

            Action action = () =>
            {
                for (int i = 0; i < loopCount; i++)
                {
                    bool lockTaken = false;
                    try
                    {
                        spinLock.Enter(ref lockTaken);
                        sharedValue++;
                    }
                    finally
                    {
                        if (lockTaken)
                        {
                            spinLock.Exit();
                        }
                    }
                }
            };

            Thread thread1 = new Thread(() => action());
            Thread thread2 = new Thread(() => action());

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            Console.WriteLine("LockFree - Final shared value: " + sharedValue);
        }

        private void startCASLockFree()
        {
            int sharedValue = 0;

            Action action = () =>
            {
                int temp;
                for (int i = 0; i < loopCount; i++)
                {
                    do
                    {
                        temp = sharedValue;
                    } while (Interlocked.CompareExchange(ref sharedValue, temp + 1, temp) != temp);
                }
            };

            Thread thread1 = new Thread(() => action());
            Thread thread2 = new Thread(() => action());

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            Console.WriteLine("CASLockFree - Final shared value: " + sharedValue);
        }
    }
}