using Ardeno.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ardeno.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;

        public GameViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnViewModelChange;
        }

        private void OnViewModelChange()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
