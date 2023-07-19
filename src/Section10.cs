namespace MultiThread
{
    public class Section10
    {
        // Future Pattern

        public class Future
        {
            private string? result = null;

            public string Result()
            {
                while (Volatile.Read(ref result) == null)
                {
                    Thread.Sleep(100);
                }

                return result ?? string.Empty;
            }

            public void Startup(Action action)
            {
                var thread = new Thread(() =>
                {
                    action();
                    Volatile.Write(ref result, "Done");
                });
                thread.Start();
            }
        }

        public void StartUp()
        {
            Console.WriteLine("Process Start!");

            var future1 = new Future();
            future1.Startup(() =>
            {
                Thread.Sleep(3000);
            });

            var future2 = new Future();
            future2.Startup(() =>
            {
                Thread.Sleep(1000);
            });

            var future3 = new Future();
            future3.Startup(() =>
            {
                Thread.Sleep(2000);
            });

            Console.WriteLine("뭔가를 더 할수 있음 Start");
            Thread.Sleep(1000);
            Console.WriteLine("뭔가를 더 할수 있음 Finish");

            Console.WriteLine(future1.Result());
            Console.WriteLine(future2.Result());
            Console.WriteLine(future3.Result());
        }
    }
}