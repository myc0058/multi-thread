namespace MultiThread
{
  public class GuardedSuspension
  {
    private bool shouldStop = false;
    
    private Queue<string> requests = new Queue<string>();
    
    public void StartUp() {
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
      while(Volatile.Read(ref shouldStop) == false) {
        lock(requests) {
          string request = DateTime.Now.ToString();
          requests.Enqueue(request);
          Console.WriteLine(Thread.CurrentThread.Name + " : " + request);
          Monitor.PulseAll(requests);
          Thread.Sleep(100);
        }
      }
    }

    private void DoServer()
    {
      while(Volatile.Read(ref shouldStop) == false) {
        lock(requests) {
          if (requests.TryDequeue(out var request)) {
            Console.WriteLine(Thread.CurrentThread.Name + " : " + request);
          } else {
            Monitor.Wait(requests);
          }
        }
      }
    }

    
  }
}