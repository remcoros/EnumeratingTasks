using System;

namespace EnumeratingTasks
{
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
