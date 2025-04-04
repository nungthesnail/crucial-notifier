namespace Crucial.App.Interfaces;

public interface IPageArgumentsValidator
{
    bool ArgumentsAreValid(object?[] args);
}
