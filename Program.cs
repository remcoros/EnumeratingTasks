using System;
using System.Linq;
using System.Threading.Tasks;

namespace EnumeratingTasks
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enumerating an IQueryable<Task>");

            var handlersOfTask = new[] {
                    new Handler("1"), 
                    new Handler("2"), 
                    new Handler("3")
                    }
                .Select(x => x.Handle());
            
            foreach(var handler in handlersOfTask) // MoveNext calls x.Handle() here
            {
                Console.WriteLine($"In foreach, before await");
                await handler.ConfigureAwait(false);
                Console.WriteLine($"In foreach, after await");
            }

            Console.WriteLine("Enumerating an IQueryable<async Task>");

            var handlersOfAsyncTask = new[] {
                    new Handler("1"), 
                    new Handler("2"), 
                    new Handler("3")
                    }
                .Select(async x => await x.Handle());
            
            foreach(var handler in handlersOfAsyncTask) // MoveNext calls x.Handle() here
            {
                Console.WriteLine($"In foreach, before await");
                await handler.ConfigureAwait(false);
                Console.WriteLine($"In foreach, after await");
            }

            Console.WriteLine("Enumerating an IQueryable<Func<Task>>");

            // handlers as tasks
            var handlersOfFunc = new[] {
                    new Handler("1"), 
                    new Handler("2"), 
                    new Handler("3")
                    }
                .Select(x => new Func<Task>(() => x.Handle()));
            
            foreach(var handler in handlersOfFunc) // MoveNext returns a Func, which can be used to call x.Handle()
            {
                Console.WriteLine($"In foreach, before await");
                await handler().ConfigureAwait(false); // x.Handle() is called here
                Console.WriteLine($"In foreach, after await");
            }
        }
    }

    public class Handler
    {
        public Handler(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public async Task Handle()
        {
            Console.WriteLine($"Handler {Name}, before await");
            await Task.Delay(100);
            Console.WriteLine($"Handler {Name}, after await");
        }
    }
}
