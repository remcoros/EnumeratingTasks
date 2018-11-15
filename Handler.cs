using System;
using System.Threading.Tasks;

namespace EnumeratingTasks
{
    public class Handler
    {
        public Handler(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public virtual async Task Handle()
        {
            if (Name == "2")
            {
                throw new InvalidOperationException();
            }

            Console.WriteLine($"Handler {Name}, before await");
            await Task.Delay(10);
            Console.WriteLine($"Handler {Name}, after await");
        }
    }
}
