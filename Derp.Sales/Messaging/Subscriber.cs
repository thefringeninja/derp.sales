namespace Derp.Sales.Messaging
{
    public interface Subscriber
    {
        void Subscribe<T>(Handles<T> handler) where T : Message;
        void Unsubscribe<T>(Handles<T> handler) where T : Message;
    }
}