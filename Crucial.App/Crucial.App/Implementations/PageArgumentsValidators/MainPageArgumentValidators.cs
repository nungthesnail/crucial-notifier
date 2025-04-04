using Crucial.App.Interfaces;

namespace Crucial.App.Implementations.PageArgumentsValidators;

public class MainPageArgumentValidators : IPageArgumentsValidator
{
    public bool ArgumentsAreValid(object?[] _) => true;
}
