using System.Diagnostics;
using System.Windows.Input;

namespace Rrs.Wpf;

/// <summary>
/// This is an implementation from the web that I have found from multiple sources. I do not know
/// its original author
/// </summary>
public class RelayCommand : ICommand
{
    private readonly Action<object?> _execute;
    private readonly Predicate<object?> _canExecute;

    public RelayCommand(Action<object?> execute)
        : this(execute, null) { }

    public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute ?? (_ => true);
    }

    [DebuggerStepThrough]
    public bool CanExecute(object? parameter)
    {
        return _canExecute(parameter);
    }

    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public void Execute(object? parameter)
    {
        _execute(parameter);
    }
}
