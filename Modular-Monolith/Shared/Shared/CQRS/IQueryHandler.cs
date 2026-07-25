namespace Shared.CQRS
{
    public interface IQueryHandler<in TQuery, out TResponse>
        where TQuery : IQuery<TResponse>
        where TResponse : notnull
    {
    }
}
