using System;
using System.Threading.Tasks;

namespace EnumeratingTasks
{
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
}
