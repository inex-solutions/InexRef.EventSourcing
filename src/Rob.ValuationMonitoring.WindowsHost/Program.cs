using System;
using System.Threading.Tasks;

namespace Rob.ValuationMonitoring.WindowsHost
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var task = Run(args);
                task.GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UNHANDLED EXCEPTION {ex}");
            }
            finally
            {
                if (Environment.UserInteractive)
                {
                    Console.WriteLine("Press enter to exit");
                    Console.ReadLine();
                }
            }
        }

        static async Task Run(string[] args)
        {
            using (var runner = new TestRunner())
            {
                runner.SetupEnvironment();
                await runner.ExecuteTest(new SimpleTest());
                //await runner.ExecuteTest(new LoadTest());
            }
        }
    }
}
