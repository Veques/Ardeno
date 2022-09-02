using Ardeno.Services;
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

        private readonly NavigationService<TViewModel> _navigationService;
        private readonly Predicate<object> _canExecute;

        public NavigationCommand(NavigationService<TViewModel> navigationService, Predicate<object> canExecute)
        {
            _navigationService = navigationService;
            _canExecute = canExecute;
        }

        public override bool CanExecute(object? parameter)
        {
            return _canExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            _navigationService.Navigate();
        }
    }
}
