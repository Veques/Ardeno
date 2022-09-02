using Ardeno.Commands;
using Ardeno.Models;
using Ardeno.Stores;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Reflection.Metadata.BlobBuilder;
using System.IO;
using Ardeno.Helpers.Enums;
using Ardeno.Services;

namespace Ardeno.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        private readonly ApplicationDbContext db = new();
        public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;

        public ICommand NavigateRegister { get; set; }
        public ICommand NavigateLogin { get; set; }
        public ICommand ManageDatabaseCommand { get; set; }

        public MainViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnViewModelChange;

            NavigateRegister = new NavigationCommand<RegisterViewModel>(new NavigationService<RegisterViewModel>(
                navigationStore, () => new RegisterViewModel(navigationStore)), 
                x => true);

            NavigateLogin = new NavigationCommand<LoginViewModel>(new NavigationService<LoginViewModel>(
                navigationStore, () => new LoginViewModel(navigationStore)), 
                x => true);

            ManageDatabaseCommand = new RelayCommand(x => DatabaseActions(), x => true);

        }

        private void DatabaseActions()
        {
            //Czy napewno?
            try
            {
                db.Users.RemoveRange(db.Users);
                db.Questions.RemoveRange(db.Questions);
            }
            catch { }

            if (db.SaveChanges() > 0)
            {
                MessageBox.Show("Wyszczono");
            }

            FillDatabase();
        }

        private void FillDatabase()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Helpers\Quizz.xlsx");
            try
            {
                OleDbConnection connection = new((@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';"));
                connection.Open();
                OleDbCommand command = new("SELECT * FROM [" + "Arkusz1" + "$]", connection);
                DataTable Data = new();
                OleDbDataAdapter adapter = new(command);
                adapter.Fill(Data);
                connection.Close();
                for (int i = Data.Rows.Count - 1; i >= 0; i--)
                {
                    if (Data.Rows[i][0].ToString() == String.Empty)
                    {
                        Data.Rows.RemoveAt(i);
                    }
                }
                List<Question> list = new();

                list = (from DataRow dr in Data.Rows
                        select new Question()
                        {
                            QuestionTitle= dr["Question"].ToString(),
                            QuestionDifficulty = dr["Difficulty"].ToString(),
                            FirstAnswer = dr["Answer1"].ToString(),
                            SecondAnswer = dr["Answer2"].ToString(),
                            ThirdAnswer = dr["Answer3"].ToString(),
                            FourthAnswer = dr["Answer4"].ToString(),
                            CorrectAnswer = dr["CorrectAnswer"].ToString(),
                            Done = (double)dr["Done"]

                        }).ToList();

                foreach (Question item in list)
                {
                    db.Questions.Add(item);
                }
                if (db.SaveChanges() > 0)
                {
                    MessageBox.Show("Dodano [ytania z pliku Excel");
                }

            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Coś poszło nie tak");

            }
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

        #region Properties
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

        #endregion
    }
}
