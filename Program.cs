using System;
using System.Linq;
using System.Threading.Tasks;

namespace EnumeratingTasks
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // await EnumeratingTasks.Run();

            try
            {
                await IQueryableAsyncTask();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Caught while awaiting IQueryableAsyncTask: ${ex.GetType()}");
            }

            try
            {
                await IQueryableTask();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Caught while awaiting IQueryableTask: ${ex.GetType()}");
            }

            Console.ReadKey(true);
        }

        public static async Task IQueryableAsyncTask()
        {
            Console.WriteLine("Enumerating an IQueryable<async Task>");

            var handlersOfAsyncTask = new[]
                {
                    new Handler("1"),
                    new Handler("2"),
                    new Handler("3")
                }
                .Select(async x => await x.Handle());

            try
            {
                await Task.WhenAll(handlersOfAsyncTask);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType());
            }
        }

        public static async Task IQueryableTask()
        {
            Console.WriteLine("Enumerating an IQueryable<Task>");

            var handlersOfTask = new[] {
                    new Handler("1"),
                    new Handler("2"),
                    new Handler("3")
                }
               .Select(x => x.Handle());

            try
            {
                await Task.WhenAll(handlersOfTask);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType());
            }
        }
    }
}
