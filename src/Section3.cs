namespace MultiThread
{
    public static class Section3
    {
        private static bool shouldStop = false;

        public static void StartUp() {
            Console.WriteLine("Process Start!");

            //Thread 오브젝트 만들기
            var thread1 = new Thread(DoWork);
            var thread2 = new Thread(DoWork);

            //Thread 시작
            thread1.Start();
            thread2.Start();

            
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            //shouldStop = true; //visibilty 문제 발생
            Volatile.Write(ref shouldStop, true);
            Console.WriteLine("exiting...");
            
            //Thread 종료 대기
            thread1.Join();
            thread2.Join();

            Console.WriteLine("All done!");
        }
        
        private static void DoWork()
        {
            while(!shouldStop) {
                Console.WriteLine("shouldStop: " + shouldStop);
            }
        }
    }
}