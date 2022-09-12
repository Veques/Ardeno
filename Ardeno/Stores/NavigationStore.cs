using Ardeno.ViewModels;
using System;

namespace Ardeno.Stores
{
    public class NavigationStore
    {
        public event Action CurrentViewModelChanged;


        private BaseViewModel _baseViewModel;

        public BaseViewModel CurrentViewModel
        {
            get => _baseViewModel;
            set
            {
                _baseViewModel = value;
                OnCurrentViewModelChanged();
            }

        }
        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}
