using System.Collections.Concurrent;

/***********************************************************************

2024-03-02 writeCount에 의한 배타제어가 되지 않는 문제를 수정하였습니다.

************************************************************************/

namespace MultiThread
{
    public class Section7
    {
        // Read Write Lock Pattern

        private bool shouldStop = false;

        private int readCount = 0;
        private int writeCount = 0;
        private string message = DateTime.Now.ToString();

        public void StartUp()
        {
            Console.WriteLine("Process Start!");

            //Thread 오브젝트 만들기
            List<Thread> threads = new List<Thread>();

            for (int i = 0; i < 2; i++)
            {
                var thread = new Thread(Write);
                thread.Name = "Writer" + i;
                threads.Add(thread);
            }

            for (int i = 0; i < 6; i++)
            {
                var thread = new Thread(Read);
                thread.Name = "Reader" + i;
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

        /*
            주의: 이 예제는 Volatile.Read, Volatile.Write로 Reordering을 막고 있습니다.
            Interlocked.Increment, Interlocked.Decrement로 Atomicity를 보장하고 있지만 Reordering은 막지 않습니다.
            아래 코드는 문제 없이 돌아가지만 코드가 추가되면 문제가 발생할 여지가 많습니다.
            Lock을 쓰지 않고 멀티스레드 프로그래밍을 하는건 매우 위험합니다.
        */

        private void Write()
        {
            while (Volatile.Read(ref shouldStop) == false)
            {
                var result = Interlocked.CompareExchange(ref writeCount, 1, 0);

                if (result != 0)
                {
                    continue;
                }

                try
                {
                    if (Volatile.Read(ref readCount) > 0)
                    {
                        continue;
                    }

                    var request = DateTime.Now.ToString();

                    Volatile.Write(ref message, request);
                    Console.WriteLine(Thread.CurrentThread.Name + " : " + request);
                }
                finally
                {
                    Interlocked.Decrement(ref writeCount);
                }

                Thread.Sleep(50);
            }
        }

        private void Read()
        {
            while (Volatile.Read(ref shouldStop) == false)
            {
                if (Volatile.Read(ref writeCount) > 0)
                {
                    continue;
                }

                Interlocked.Increment(ref readCount);

                try
                {
                    string readMessage = Volatile.Read(ref message);
                    Console.WriteLine(Thread.CurrentThread.Name + " : " + message);

                }
                finally
                {
                    Interlocked.Decrement(ref readCount);
                }

                Thread.Sleep(50);

            }
        }
    }
}