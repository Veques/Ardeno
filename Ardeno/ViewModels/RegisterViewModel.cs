using Ardeno.Commands;
using Ardeno.Models;
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
    public class RegisterViewModel : BaseViewModel
    {
        private readonly ApplicationDbContext db = new();
        public ICommand CreateAccount { get; set; }
        public RegisterViewModel(NavigationStore navigationStore)
        {

            CreateAccount = new RelayCommand(x => Create(),
                                             x => !string.IsNullOrEmpty(UsernameRegister) &&
                                                  !string.IsNullOrEmpty(PasswordRegister));
        }

        private void Create()
        {
            if (db.Users.Any(x => x.Username == UsernameRegister))
            {
                ErrorAnimation = true;
                CommunicateState = "Taki użytkownik już istnieje";

                Task.Run( () => 
                {
                    Thread.Sleep(2400);
                    CommunicateState = "";
                    ErrorAnimation = false;
                });
                return;
            }

            if(PasswordRegister !=  PasswordRegisterConfirm)
            {
                ErrorAnimation = true;
                CommunicateState = "Hasła się nie zgadzają";

                Task.Run(() =>
                {
                    Thread.Sleep(2400);
                    CommunicateState = "";
                    ErrorAnimation = false;
                });
                return;
            }

            db.Users.Add(new User
            {
                Username = UsernameRegister,
                Password = PasswordRegister
            });

            if (db.SaveChanges() > 0)
            {
                //Both to proc exit action
                SuccessAnimation = true;
                CommunicateState = "Dodano !";
                Task.Run(() =>
                {
                    Thread.Sleep(2000);
                    CommunicateState = "";
                    SuccessAnimation = false;
                    return;
                }
                );
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
        private string _passwordRegisterConfirm;

        public string PasswordRegisterConfirm
        {
            get { return _passwordRegisterConfirm; }
            set
            {
                _passwordRegisterConfirm = value;
                OnPropertyChanged(nameof(PasswordRegisterConfirm));
            }
        }

        private bool _successAnimation = false;

        public bool SuccessAnimation
        {
            get { return _successAnimation; }
            set {
                _successAnimation = value;
                OnPropertyChanged(nameof(SuccessAnimation)); 
            }
        }

        private bool _errorVisibility = false;

        public bool ErrorAnimation
        {
            get { return _errorVisibility; }
            set
            {
                _errorVisibility = value;
                OnPropertyChanged(nameof(ErrorAnimation));
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

    }
}
