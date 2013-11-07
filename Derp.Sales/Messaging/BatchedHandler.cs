using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Derp.Sales.Messaging
{
    public class BatchedHandler<T> : Handles<T> where T : Message
    {
        private readonly Func<IEnumerable<T>, Task> onCapacityReached;
        private readonly int capacity;
        private readonly IList<T> queue = new List<T>(); 
        public BatchedHandler(Func<IEnumerable<T>, Task> onCapacityReached, int capacity = 1)
        {
            this.onCapacityReached = onCapacityReached;
            this.capacity = capacity;
        }

        public async Task Handle(T message)
        {
            queue.Add(message);
            if (queue.Count < capacity)
            {
                return;
            }

            await onCapacityReached(new List<T>(queue));

            queue.Clear();
        }
    }
}