namespace Smew.Infrastructure
{
    public interface IReactiveEvent<T> {
        T Body {get;init;}
    }
}