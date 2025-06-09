using JetBrains.Annotations;

namespace Slnfgen.CLI;

/// <summary>
///
/// </summary>
/// <typeparam name="TRequest">Type of the request object</typeparam>
/// <typeparam name="TResponse">Type of the returned response object</typeparam>
[UsedImplicitly]
public interface IRequestHandler<in TRequest, out TResponse>
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    TResponse Handle(TRequest request);
}

/// <summary>
///
/// </summary>
/// <typeparam name="TRequest"></typeparam>
public interface IRequestHandler<in TRequest>
    where TRequest : IEquatable<TRequest>
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    void Handle(TRequest request);
}

/// <summary>
///
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
[UsedImplicitly]
public interface IAsyncRequestHandler<in TRequest, TResponse>
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}

/// <summary>
///
/// </summary>
/// <typeparam name="TRequest"></typeparam>
[UsedImplicitly]
public interface IAsyncRequestHandler<in TRequest>
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}
