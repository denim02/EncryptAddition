using System;
using System.Threading.Tasks;
using System.Windows.Input;

public abstract class BaseAsyncCommand : ICommand
{
    // Used to track execution status to block button
    private bool _isExecuting;
    public bool IsExecuting
    {
        get
        {
            return _isExecuting;
        }
        set
        {
            _isExecuting = value;
            OnCallExecuteChanged();
        }
    }

    public event EventHandler CanExecuteChanged;

    public virtual bool CanExecute(object parameter)
    {
        return !IsExecuting;
    }

    // Will be called when the command is executed
    public async void Execute(object parameter)
    {
        IsExecuting = true;

        await ExecuteAsync(parameter);
        IsExecuting = false;
    }

    // Underlying method to be implemented by inheriting classes
    public abstract Task ExecuteAsync(object parameter);

    protected void OnCallExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, new EventArgs());
    }
}
