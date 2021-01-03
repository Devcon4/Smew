using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Smew.Infrastructure
{
    public interface IReactiveEventBroker
    {
        Task Queue<T>(IReactiveEvent<T> reactiveEvent);
    }

    public class ReactiveEventBroker : IReactiveEventBroker
    {
        private readonly IMessageBus _messageBus;

        public ReactiveEventBroker(IMessageBus messageBus)
        {
            _messageBus = messageBus;
            // Assembly
            //     .GetExecutingAssembly()
            //     .GetTypes()
            //     .Where(t => t.GetCustomAttributes<EventHandlerAttribute>().Count() > 0)
            //     .ToList()
            //     .ForEach(s => ActivatorUtilities.GetServiceOrCreateInstance(provider, s));
        }

        public Task Queue<T>(IReactiveEvent<T> reactiveEvent)
        {
            return _messageBus.PublishAsync(reactiveEvent);
        }

        private void Register() {
        }
    }
}