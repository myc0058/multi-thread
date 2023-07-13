namespace MultiThread
{
    public class Section5
    {
        // Guarded Suspension Pattern
        // https://en.wikipedia.org/wiki/Guarded_suspension

        private bool shouldStop = false;

        private object lockObj = new object();
        private string message = string.Empty;
        private bool changed = false;

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

            //Thread 종료 대기
            thread1.Join();
            thread2.Join();

            Console.WriteLine("All done!");
        }

        private void DoClient()
        {
            while (Volatile.Read(ref shouldStop) == false)
            {
                lock (lockObj)
                {
                    string oldMessage = message;
                    message = DateTime.Now.ToString();
                    changed = true;
                    Console.WriteLine(Thread.CurrentThread.Name + " : " + oldMessage + " -> " + message);
                }
                Thread.Sleep(1000);
            }
        }

        private void DoServer()
        {
            while (Volatile.Read(ref shouldStop) == false)
            {
                lock (lockObj)
                {
                    if (changed == true)
                    {
                        Console.WriteLine(Thread.CurrentThread.Name + " : " + message);
                        changed = false;
                    }
                    else
                    {
                        Console.WriteLine(Thread.CurrentThread.Name + " : " + "No Message");
                        Thread.Sleep(1000);
                    }
                }
            }
        }
    }
}