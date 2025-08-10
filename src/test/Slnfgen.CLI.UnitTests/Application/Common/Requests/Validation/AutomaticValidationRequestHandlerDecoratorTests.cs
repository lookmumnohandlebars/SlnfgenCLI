using FluentValidation;
using Slnfgen.CLI.Application.Common.Requests;
using Slnfgen.CLI.Application.Common.Requests.Validation;

namespace Slnfgen.CLI.UnitTests.Application.Common.Requests.Validation;

public class AutomaticValidationRequestHandlerDecoratorTests
{
    private readonly AutomaticValidationRequestHandlerDecorator<TestRequest, TestResponse> _sut;

    public AutomaticValidationRequestHandlerDecoratorTests()
    {
        _sut = new AutomaticValidationRequestHandlerDecorator<TestRequest, TestResponse>(
            new TestRequestHandler(),
            new TestRequestValidator()
        );
    }

    [Fact]
    public void Handle_ShouldReturnResponse_WhenRequestIsValid()
    {
        // Arrange
        var request = new TestRequest("Valid");

        // Act
        var response = _sut.Handle(request);

        // Assert
        response.Should().BeEquivalentTo(new TestResponse("Handled: Valid"));
    }

    [Fact]
    public void Handle_ShouldThrowValidationErrorIfRequestInvalid()
    {
        // Arrange
        var request = new TestRequest("Invalid");

        // Act
        var act = () => _sut.Handle(request);

        // Assert
        act.Should().Throw<RequestValidationException>();
    }

    #region Helper Classes

    private class TestRequest : IEquatable<TestRequest>
    {
        public string Value { get; }

        public TestRequest(string value)
        {
            Value = value;
        }

        public bool Equals(TestRequest? other)
        {
            if (other is null)
                return false;
            return Value == other.Value;
        }
    }

    private class TestRequestValidator : AbstractValidator<TestRequest>
    {
        public TestRequestValidator()
        {
            RuleFor(x => x.Value).NotEmpty().NotEqual("Invalid");
        }
    }

    private class TestResponse : IEquatable<TestResponse>
    {
        public string Value { get; }

        public TestResponse(string value)
        {
            Value = value;
        }

        public bool Equals(TestResponse? other)
        {
            if (other is null)
                return false;
            return Value == other.Value;
        }
    }

    private class TestRequestHandler : IRequestHandler<TestRequest, TestResponse>
    {
        public TestResponse Handle(TestRequest request)
        {
            return new TestResponse($"Handled: {request.Value}");
        }
    }

    #endregion
}
