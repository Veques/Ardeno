using Ardeno.Commands;
using Ardeno.Models;
using Ardeno.Stores;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Ardeno.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly ApplicationDbContext db = new();
        public ICommand LogInCommand { get; set; }
        public ICommand SignInCommand { get; set; }
        public ICommand NavigateGamePage { get; set; }
        public ICommand ShowSignInCommand { get; set; }
        public ICommand HideSignInCommand { get; set; }
        public LoginViewModel(NavigationStore navigationStore)
        {
            ShowSignInCommand = new RelayCommand(x => ShowSignInWindow(), x=> true);
            HideSignInCommand = new RelayCommand(x => HideSignInWindow(), x=> true);
            LogInCommand = new RelayCommand(x => Login(),
                                            x => !string.IsNullOrEmpty(_username) && 
                                                 !string.IsNullOrEmpty(_password));
            SignInCommand = new RelayCommand(x => CreateAccount(),
                                             x => !string.IsNullOrEmpty(_usernameRegister) && 
                                                  !string.IsNullOrEmpty(_passwordRegister));
            NavigateGamePage = new NavigationCommand<GameTypeViewModel>(navigationStore,
                () => new GameTypeViewModel(navigationStore), x => true);
        }

        private void HideSignInWindow()
        {
            SignInVisibility = Visibility.Collapsed;
            ShowSignInVisibility = Visibility.Visible;
        }

        private void ShowSignInWindow()
        {
            SignInVisibility = Visibility.Visible;
            ShowSignInVisibility = Visibility.Collapsed;

        }

        #region Login
        private void Login()
        {
            if (!db.Users.Any(x => x.Username == Username))
            {
                MessageBox.Show("Takie konto nie istnieje");
                return;
            }

            NavigateGamePage.Execute(this);

        }
        #endregion

        #region CreateAccount
        private void CreateAccount()
        {
            if (db.Users.Any(x => x.Username == _username))
            {
                MessageBox.Show("Takie konto już istnieje");
                return;
            }

            db.Users.Add(new User
            {
                Username = _username,
                Password = _password
            });

            if (db.SaveChanges() > 0)
            {
                MessageBox.Show("Sukces");
            }

        }
        #endregion


        #region Properties

        private bool _activeLogin = true;

        public bool ActiveLogin 
        { 
            get => _activeLogin; 
            set
            {
                _activeLogin = value; 
                OnPropertyChanged(nameof(ActiveLogin));
            } 

        }


        private string _username;

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }


        private string _usernameRegister;

        public string UsernameRegister
        {
            get { return _usernameRegister; }
            set
            {
                _usernameRegister = value;
                OnPropertyChanged(nameof(UsernameRegister));
            }
        }

        private string _passwordRegister;

        public string PasswordRegister
        {
            get { return _passwordRegister; }
            set
            {
                _passwordRegister = value;
                OnPropertyChanged(nameof(PasswordRegister));
            }
        }


        private Visibility _signInVisibility = Visibility.Collapsed;

        public Visibility SignInVisibility
        {
            get { return _signInVisibility; }
            set { _signInVisibility = value;
                OnPropertyChanged(nameof(SignInVisibility));
            }
        }

        private Visibility _showSignInVisibility;

        public Visibility ShowSignInVisibility
        {
            get { return _showSignInVisibility; }
            set
            {
                _showSignInVisibility = value;
                OnPropertyChanged(nameof(ShowSignInVisibility));
            }
        }

        #endregion
    }
}
