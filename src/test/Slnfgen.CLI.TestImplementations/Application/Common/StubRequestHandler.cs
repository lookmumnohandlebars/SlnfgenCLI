namespace Slnfgen.CLI.TestImplementations.Application.Common;

public class StubRequestHandler<TRequest> : IRequestHandler<TRequest>
    where TRequest : IEquatable<TRequest>
{
    private Dictionary<TRequest, int> _stubCollection = new();

    public void Handle(TRequest request)
    {
        var isStubRecorded = _stubCollection.TryGetValue(request, out var stubCount);
        if (isStubRecorded)
        {
            _stubCollection[request] = stubCount + 1;
        }
        else
        {
            _stubCollection.Add(request, 1);
        }
    }

    public int GetRequestCalls(TRequest request)
    {
        var isStubRecorded = _stubCollection.TryGetValue(request, out var stubCount);
        return isStubRecorded ? stubCount : 0;
    }
}
