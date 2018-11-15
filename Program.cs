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

            foreach (var handler in handlersOfTask) // MoveNext calls x.Handle() here
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

            foreach (var handler in handlersOfAsyncTask) // MoveNext calls x.Handle() here
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

            foreach (var handler in handlersOfFunc) // MoveNext returns a Func, which can be used to call x.Handle()
            {
                Console.WriteLine($"In foreach, before await");
                await handler().ConfigureAwait(false); // x.Handle() is called here
                Console.WriteLine($"In foreach, after await");
            }

            Console.WriteLine("Enumerating an IQueryable<Task> with exception before await");

            // handlers as tasks
            var handlersOfFuncExBefore = new[] {
                    new HandlerWithExceptionBefore("1"),
                    new HandlerWithExceptionBefore("2"),
                    new HandlerWithExceptionBefore("3")
                    }
                .Select(x => x.Handle());

            try
            {
                foreach (var handler in handlersOfFuncExBefore)
                {
                    Console.WriteLine($"In foreach, before await");
                    try
                    {
                        await handler.ConfigureAwait(false);
                    }
                    catch (HandlerException ex)
                    {
                        Console.WriteLine($"Exception while awaiting, Handler {ex.Handler.Name}: {ex.Message}");
                    }

                    Console.WriteLine($"In foreach, after await");
                }
            }
            catch (HandlerException ex)
            {
                Console.WriteLine($"Exception while enumerating, Handler {ex.Handler.Name}: {ex.Message}");
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

        public virtual async Task Handle()
        {
            Console.WriteLine($"Handler {Name}, before await");
            await Task.Delay(10);
            Console.WriteLine($"Handler {Name}, after await");
        }
    }

    public class HandlerWithExceptionBefore : Handler
    {
        public HandlerWithExceptionBefore(string name) : base(name)
        {
        }

        public override async Task Handle()
        {
            Console.WriteLine($"Handler {Name}, before await");

            throw new HandlerException(this, "Exception before await");

            await Task.Delay(10);

            Console.WriteLine($"Handler {Name}, after await");
        }
    }

    public class HandlerWithExceptionAfter : Handler
    {
        public HandlerWithExceptionAfter(string name) : base(name)
        {
        }

        public override async Task Handle()
        {
            Console.WriteLine($"Handler {Name}, before await");

            await Task.Delay(10);

            throw new HandlerException(this, "Exception after await");

            Console.WriteLine($"Handler {Name}, after await");
        }
    }

    public class HandlerException : Exception
    {
        public HandlerException(Handler handler, string message)
            : base(message)
        {
            Handler = handler;
        }

        public Handler Handler { get; }
    }
}
