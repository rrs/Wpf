using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Rrs.Wpf;

public class AsyncRelayCommand : ICommand
{
    private readonly Func<object, Task> execute;
    private readonly Func<object, bool> canExecute;

    private bool _isExecuting;

    public AsyncRelayCommand(Func<object, Task> execute, Func<object, bool> canExecute = null)
    {
        this.execute = execute;
        this.canExecute = canExecute ?? (_ => true);
    }

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public void RaiseCanExecuteChanged()
    {
        CommandManager.InvalidateRequerySuggested();
    }

    public bool CanExecute(object parameter) => !_isExecuting && canExecute(parameter);

    public async void Execute(object parameter)
    {
        _isExecuting = true;
        RaiseCanExecuteChanged();

        try
        {
            await execute(parameter);
        }
        finally
        {
            _isExecuting = false;
            RaiseCanExecuteChanged();
        }
    }
}
