namespace Slnfgen.CLI.TestImplementations.Application.Common;

public class StubRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IEquatable<TRequest>
    where TResponse : IEquatable<TResponse>
{
    private readonly Dictionary<TRequest, TResponse> _stubbedResponses = new();
    private readonly Dictionary<TRequest, int> _stubCollection = new();

    public TResponse Handle(TRequest request)
    {
        var isStubRecorded = _stubCollection.TryGetValue(request, out var stubCount);
        if (isStubRecorded)
            _stubCollection[request] = stubCount + 1;
        else
            _stubCollection.Add(request, 1);

        return _stubbedResponses[request];
    }

    public void AddStubResponse(TRequest request, TResponse response)
    {
        _stubbedResponses.TryAdd(request, response);
    }

    public int GetRequestCalls(TRequest request)
    {
        var isStubRecorded = _stubCollection.TryGetValue(request, out var stubCount);
        return isStubRecorded ? stubCount : 0;
    }
}
