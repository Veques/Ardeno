using Ardeno.Commands;
using Ardeno.Models;
using Ardeno.Stores;
using Ardeno.Views;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Ardeno.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly ApplicationDbContext db = new();
        public ICommand LogInCommand { get; set; }
        public LoginViewModel(NavigationStore navigationStore)
        {

            LogInCommand = new RelayCommand(x => Login(),
                                            x => !string.IsNullOrEmpty(_username) &&
                                                 !string.IsNullOrEmpty(_password));

        }

        #region Login
        private void Login()
        {
            if (!db.Users.Any(x => x.Username == Username))
            {
                CommunicateState = "Takie konto nie istnieje";
                ErrorAnimation = true;
                return;
            }

            if (!db.Users.Any(x => x.Username == Username && x.Password == Password))
            {
                ErrorAnimation = true;
                CommunicateState = "Błędne dane";
                return;
            }

            if(!db.Quotes.Any())
            {
                CommunicateState = "Najpierw zaimportuj bazę danych";
                ErrorAnimation = true;
                return;
            }

            LoggedUser = Username;

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

        public static string LoggedUser { get; private set; }

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

        private string _communicateState;

        public string CommunicateState
        {
            get { return _communicateState; }
            set
            {
                _communicateState = value;
                OnPropertyChanged(nameof(CommunicateState));
            }
        }
        private bool _errorAnimation = false;

        public bool ErrorAnimation
        {
            get { return _errorAnimation; }
            set
            {
                _errorAnimation = value;
                OnPropertyChanged(nameof(ErrorAnimation));
            }
        }
        #endregion
    }
}
