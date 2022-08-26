using Ardeno.Stores;
using Ardeno.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardeno.Commands
{
    public class NavigationCommand<TViewModel> : CommandBase
        where TViewModel : BaseViewModel
    {

        private readonly NavigationStore _command;
        private readonly Func<TViewModel> _createViewModel;
        private readonly Predicate<object> _canExecute;

        public NavigationCommand(NavigationStore command, Func<TViewModel> createViewModel, Predicate<object> canExecute)
        {
            _command = command;
            _createViewModel = createViewModel;
            _canExecute = canExecute;
        }

        public override bool CanExecute(object? parameter)
        {
            return _canExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            _command.CurrentViewModel = _createViewModel();
        }
    }
}
