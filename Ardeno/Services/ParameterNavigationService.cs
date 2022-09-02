using Ardeno.Stores;
using Ardeno.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardeno.Services
{
    public class ParameterNavigationService<TParameter, TViewModel>
        where TViewModel : BaseViewModel 
    {

        private readonly NavigationStore _navigationStore;
        private readonly Func<TParameter, TViewModel> _createViewModel;

        public ParameterNavigationService(NavigationStore navigationStore, Func<TParameter, TViewModel> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        public void Navigate(TParameter parameter)
        {
            _navigationStore.CurrentViewModel = _createViewModel(parameter);
        }
    }
}
