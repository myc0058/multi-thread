namespace MultiThread
{
    public class Section4
    {
        // Guarded Suspension Pattern
        // https://en.wikipedia.org/wiki/Guarded_suspension

        private bool shouldStop = false;

        private Queue<string> requests = new Queue<string>();

        public void StartUp()
        {
            Console.WriteLine("Process Start!");

            //Thread 오브젝트 만들기
            var thread1 = new Thread(DoClient);
            thread1.Name = "Client";
            var thread2 = new Thread(DoServer);
            thread2.Name = "Server";

            //Thread 시작
            thread1.Start();
            thread2.Start();

            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();

            Volatile.Write(ref shouldStop, true);

            // thread2가 Wait 상태일 때, 아래코드가 없으면 종료되지 않음
            lock (requests)
            {
                Monitor.PulseAll(requests);
            }

            //Thread 종료 대기
            thread1.Join();
            thread2.Join();

            Console.WriteLine("All done!");
        }

        private void DoClient()
        {
            while (Volatile.Read(ref shouldStop) == false)
            {
                lock (requests)
                {
                    string request = DateTime.Now.ToString();
                    requests.Enqueue(request);
                    Monitor.PulseAll(requests); //일해라 핫산!!
                    Console.WriteLine(Thread.CurrentThread.Name + " : " + request);
                }
                Thread.Sleep(1000); // 성능 이슈
            }
        }

        private void DoServer()
        {
            while (Volatile.Read(ref shouldStop) == false)
            {
                lock (requests)
                {
                    if (requests.TryDequeue(out var request))
                    {
                        Console.WriteLine(Thread.CurrentThread.Name + " : " + request);
                    }
                    else
                    {
                        Console.WriteLine(Thread.CurrentThread.Name + " : Wait");
                        Monitor.Wait(requests); // 일 없으니까 쉴래요
                        Console.WriteLine(Thread.CurrentThread.Name + " : Awake");
                    }
                }
            }
        }
    }
}