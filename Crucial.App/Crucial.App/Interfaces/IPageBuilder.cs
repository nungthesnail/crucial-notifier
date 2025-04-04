using Avalonia.Controls;

namespace Crucial.App.Interfaces;

public interface IPageBuilder
{
    void SetArguments(object?[] arguments);
    UserControl Build();
}
