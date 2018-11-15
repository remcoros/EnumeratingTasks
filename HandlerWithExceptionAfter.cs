using System;
using System.Threading.Tasks;

namespace EnumeratingTasks
{
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
}
