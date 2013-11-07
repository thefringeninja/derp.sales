namespace Derp.Sales.Messaging
{
    public interface IBus : EventPublisher, CommandSender, Subscriber, Handles<Message>
    {
    }
}
