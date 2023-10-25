using System.Collections.Generic;

using MathCore.WPF.ViewModels;

namespace DynamicDataGrid.ViewModels;

public class DataViewModel(string Title, string path) : TitledViewModel(Title)
{
    public string FilePath { get; } = path;

    private string[]? _PathComponents;
    public IEnumerable<string> PathComponents => _PathComponents ??= FilePath.Split('/', '\\');
}
