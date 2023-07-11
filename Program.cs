
namespace MultiThread
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // 반드시 디버거 attach 없이 Release로 실행해야 됨!!!!!

            //Section2.StartUp();       
            //new Section3().StartUp();
            new GuardedSuspension().StartUp();
            //new Section4Good().StartUp();
        }
    }
}
