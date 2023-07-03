Console.WriteLine("Process Start!");

//Thread 오브젝트 만들기
var thread1 = new Thread(DoWork);
var thread2 = new Thread(DoWork);

//Thread 시작
thread1.Start();
thread2.Start();

//Thread 종료 대기
thread1.Join();
thread2.Join();

Console.WriteLine("All done!");

static void DoWork()
{
    Console.WriteLine("Doing work...");
    Thread.Sleep(1000);
    Console.WriteLine("Work done!");
}