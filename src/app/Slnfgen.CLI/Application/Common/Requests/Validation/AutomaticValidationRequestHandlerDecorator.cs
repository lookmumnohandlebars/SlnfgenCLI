using FluentValidation;
using Slnfgen.Application.Module.Common.Types;

namespace Slnfgen.CLI;

/// <summary>
///     Automatic validation request handler that validates the request using FluentValidation
///     Intended to be used as a decorator for other request
/// </summary>
/// <typeparam name="TIn"></typeparam>
/// <typeparam name="TOut"></typeparam>
internal class AutomaticValidationRequestHandlerDecorator<TIn, TOut> : IRequestHandler<TIn, TOut>
    where TIn : IEquatable<TIn>
{
    private readonly IRequestHandler<TIn, TOut> _innerHandler;
    private readonly IValidator<TIn> _validator;

    public AutomaticValidationRequestHandlerDecorator(
        IRequestHandler<TIn, TOut> innerHandler,
        IValidator<TIn> validator
    )
    {
        _innerHandler = innerHandler;
        _validator = validator;
    }

    public TOut Handle(TIn request)
    {
        var validationResult = _validator.Validate(request);
        return validationResult.IsValid
            ? _innerHandler.Handle(request)
            : throw new RequestValidationException(validationResult.Errors);
    }
}

internal class AutomaticValidationRequestHandlerDecorator<TIn>
    : AutomaticValidationRequestHandlerDecorator<TIn, Unit>,
        IRequestHandler<TIn>
    where TIn : IEquatable<TIn>
{
    public AutomaticValidationRequestHandlerDecorator(
        IRequestHandler<TIn, Unit> innerHandler,
        IValidator<TIn> validator
    )
        : base(innerHandler, validator) { }
}
