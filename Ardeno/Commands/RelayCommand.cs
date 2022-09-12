using System;
using System.Windows.Input;

namespace Ardeno.Commands
{
    public class RelayCommand : ICommand
    {
        private Action<object> _execute { get; }
        private Predicate<object> _canExecute { get; }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }


        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter)
        {

            return _canExecute(parameter);
        }

        public void Execute(object? parameter)
        {

            _execute(parameter);
        }
    }
}
