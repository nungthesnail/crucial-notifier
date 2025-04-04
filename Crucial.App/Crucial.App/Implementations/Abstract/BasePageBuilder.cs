using Avalonia.Controls;
using Crucial.App.Exceptions;
using Crucial.App.Interfaces;

namespace Crucial.App.Implementations.Abstract;

public abstract class BasePageBuilder : IPageBuilder
{
    private readonly IPageArgumentsValidator _argumentsValidator;
    private object?[]? _arguments;

    protected BasePageBuilder(IPageArgumentsValidator argumentsValidator)
    {
        _argumentsValidator = argumentsValidator;
    }
    
    public void SetArguments(object?[] arguments) => _arguments = arguments;

    public UserControl Build()
    {
        if (_arguments is null)
            throw new PageInvalidArgsException("Arguments isn't specified");
        if (!_argumentsValidator.ArgumentsAreValid(_arguments))
            throw new PageInvalidArgsException($"Arguments for page are invalid");
        
        return BuildWithArguments(_arguments);
    }

    protected abstract UserControl BuildWithArguments(object?[] arguments);
}
