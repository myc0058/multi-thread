namespace MultiThread
{
    public class Section3
    {
        private bool shouldStop = false;
        
        // 반드시 디버거 attach 없이 Release로 실행해야 됨!!!!!

        public void StartUp() {
            Console.WriteLine("Process Start!");

            //Thread 오브젝트 만들기
            var thread1 = new Thread(DoWork);

            //Thread 시작
            thread1.Start();

            Thread.Sleep(1000); // thread1이 먼저 실행되도록 하기 위함
            
            // Atomicity, Visibility, prevent ReOrdering
            Volatile.Write(ref shouldStop, true);

            //shouldStop = true; // visibility 문제
            
            //Thread 종료 대기
            thread1.Join();

            Console.WriteLine("All done!");
        }
        
        private void DoWork()
        {
            bool toggle = false;
            //while(shouldStop == false) { // visibility 문제
            while(Volatile.Read(ref shouldStop) == false) {
                toggle = !toggle;
                //Console.WriteLine("Working..."); // Context Switching
                //Thread.Sleep(1000); // Context Switching
                
            }
        }
    }
}