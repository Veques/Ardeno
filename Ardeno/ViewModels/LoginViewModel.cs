using Ardeno.Commands;
using Ardeno.Models;
using Ardeno.Stores;
using Ardeno.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Ardeno.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly ApplicationDbContext db = new();
        public ICommand LogInCommand { get; set; }
        public ICommand NavigateGamePage { get; set; }
        public LoginViewModel(NavigationStore navigationStore)
        {

            LogInCommand = new RelayCommand(x => Login(),
                                            x => !string.IsNullOrEmpty(_username) && 
                                                 !string.IsNullOrEmpty(_password));
            NavigateGamePage = new NavigationCommand<GameTypeViewModel>(navigationStore,
                () => new GameTypeViewModel(navigationStore), x => true);
        }

        #region Login
        private void Login()
        {
            if (!db.Users.Any(x => x.Username == Username))
            {
                MessageBox.Show("Takie konto nie istnieje");
                return;
            }

            GameWindow gameWindow = new()
            {
                Name = "GameWindow"
            };
            gameWindow.Show();

            foreach (Window item in Application.Current.Windows)
            {
                if (item.Name != "GameWindow")
                {
                    item.Close();
                }
            }

        }
        #endregion

        #region Properties
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
        #endregion
    }
}
