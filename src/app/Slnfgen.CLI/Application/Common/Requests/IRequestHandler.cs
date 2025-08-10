using JetBrains.Annotations;
using Slnfgen.CLI.Application.Common.Types;

namespace Slnfgen.CLI.Application.Common.Requests;

/// <summary>
/// </summary>
/// <typeparam name="TRequest">Type of the request object</typeparam>
/// <typeparam name="TResponse">Type of the returned response object</typeparam>
[UsedImplicitly]
public interface IRequestHandler<in TRequest, out TResponse>
    where TRequest : IEquatable<TRequest>
{
    /// <summary>
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    TResponse Handle(TRequest request);
}

/// <summary>
/// </summary>
/// <typeparam name="TRequest"></typeparam>
[UsedImplicitly]
public interface IRequestHandler<in TRequest> : IRequestHandler<TRequest, Unit>
    where TRequest : IEquatable<TRequest> { }
