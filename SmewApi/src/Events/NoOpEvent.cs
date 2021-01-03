using Smew.Infrastructure;

namespace Smew.Events
{
    public record NoOpEvent(string Body = ""): IReactiveEvent<string>;
}