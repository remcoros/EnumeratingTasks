using System;
using System.Threading.Tasks;

namespace EnumeratingTasks
{
    public class HandlerWithoutAsync : Handler
    {
        public HandlerWithoutAsync(string name) : base(name)
        {
        }

        public override Task Handle()
        {
            Console.WriteLine($"Handler {Name}, before await");

            throw new HandlerException(this, "Exception before await");

            return Task.Delay(10);

            // Console.WriteLine($"Handler {Name}, after await");
        }
    }
}
