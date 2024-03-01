using Rrs.Wpf;

namespace TestApp.ViewModels;

internal class Page6ViewModel : ViewModel
{
    private string? _text;

    public string? Text
    {
        get => _text;
        set
        {
            _text = value;
            OnPropertyChanged(nameof(Text));
        }
    }

}
