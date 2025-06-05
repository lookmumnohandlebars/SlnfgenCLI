namespace Slnfgen.CLI;

public interface IRequestHandler<in TRequest, out TResponse>
{
    TResponse Handle(TRequest request);
}

public interface IRequestHandler<in TRequest>
{
    void Handle(TRequest request);
}

public interface IAsyncRequestHandler<in TRequest, TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}

public interface IAsyncRequestHandler<in TRequest>
{
    Task HandleAsync(TRequest request, CancellationToken cancellationToken = default);

}