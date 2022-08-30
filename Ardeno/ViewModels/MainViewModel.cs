using Ardeno.Commands;
using Ardeno.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Ardeno.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;

        public ICommand NavigateRegister { get; set; }
        public ICommand NavigateLogin { get; set; }

        public MainViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnViewModelChange;

            NavigateRegister = new NavigationCommand<RegisterViewModel>(navigationStore,
                () => new RegisterViewModel(navigationStore),
                x => true);
            NavigateLogin = new NavigationCommand<LoginViewModel>(navigationStore,
                () => new LoginViewModel(navigationStore),
                x => true);

        }

        private void OnViewModelChange()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        private void OnStateChange()
        {
            if (IsRegister)
            {
                NavigateRegister.Execute(this);
            }
            else
            {
                NavigateLogin.Execute(this);
            }
        }

        private bool _isRegister = false;

        public bool IsRegister
        {
            get { return _isRegister; }
            set { _isRegister = value;
                OnStateChange();
                OnPropertyChanged(nameof(IsRegister));
            }
        }

        private bool _isChecked;

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                
                if(value)
                {
                    Visibility = Visibility.Visible;
                    MenuVisibility = Visibility.Hidden;
                }
                else
                {
                    Visibility = Visibility.Collapsed;
                    Task.Run(() =>
                    {
                        Thread.Sleep(180);
                    MenuVisibility = Visibility.Visible;
                    });
                }

                OnPropertyChanged(nameof(IsChecked));
            }
        }

        private Visibility visibility = Visibility.Hidden;

        public Visibility Visibility
        {
            get { return visibility; }
            set { visibility = value;
                OnPropertyChanged(nameof(Visibility));
            }
        }

        private Visibility _menuVisibility = Visibility.Visible;

        public Visibility MenuVisibility
        {
            get { return _menuVisibility; }
            set
            {
                _menuVisibility = value;
                OnPropertyChanged(nameof(MenuVisibility));
            }
        }


    }
}
