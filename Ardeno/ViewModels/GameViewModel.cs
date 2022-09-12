using Ardeno.Stores;

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
