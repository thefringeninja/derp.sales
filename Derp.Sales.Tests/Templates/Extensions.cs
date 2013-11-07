using System.Collections.ObjectModel;
using System.Linq;
using Derp.Sales.Messaging;
using Derp.Sales.Tests.Fixtures;
using Nancy.Testing;

namespace Derp.Sales.Tests.Templates
{
    public static class Extensions
    {
        public static ConfigurableBootstrapper.ConfigurableBootstrapperConfigurator WithBus(
            this ConfigurableBootstrapper.ConfigurableBootstrapperConfigurator configure)
        {
            var bus = new Bus();
            var messages = new Collection<Message>();

            typeof (Message).Assembly.GetTypes().Union(typeof(Extensions).Assembly.GetTypes())
                            .Where(typeof (Message).IsAssignableFrom)
                            .Where(type => false == type.IsAbstract && type.IsClass)
                            .ForEach(messageType => bus.Subscribe(messageType, new AdHocHandler<Message>(
                                m =>
                                {
                                    messages.Add(m);
                                })));

            configure.ApplicationStartup(
                (_, pipelines) => pipelines.AfterRequest.AddItemToStartOfPipeline(
                    context => { context.Items[SystemUnderTest.MessagesKey] = messages; }));
            
            return configure.Dependency(bus);
        }
    }
}
