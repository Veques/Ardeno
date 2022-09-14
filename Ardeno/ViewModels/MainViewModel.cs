using Ardeno.Commands;
using Ardeno.Models;
using Ardeno.Services;
using Ardeno.Stores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Ardeno.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        #region Fields
        private readonly NavigationStore _navigationStore;
        private readonly ApplicationDbContext db = new();
        public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;

        public ICommand NavigateRegister { get; set; }
        public ICommand NavigateLogin { get; set; }
        public ICommand FillDatabaseCommand { get; set; }
        public ICommand ClearDatabaseCommand { get; set; }

        #endregion

        #region Constructor
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

            ClearDatabaseCommand = new RelayCommand(x => ClearDatabase(), x => true);
            FillDatabaseCommand = new RelayCommand(x => FillDatabase(), x => !db.Quotes.Any());

        }

        private void ClearDatabase()
        {
            try
            {
                db.Users.RemoveRange(db.Users);
                db.Questions.RemoveRange(db.Questions);
                db.Words.RemoveRange(db.Words);
                db.Quotes.RemoveRange(db.Quotes);
            }
            catch { }

            if (db.SaveChanges() > 0)
            {
                MessageBox.Show("Wyczyszczono");
            }
        }

        #endregion

        #region Methods
        private void FillDatabase()
        {
            FillDatabaseQuiz();
            FillDatabaseLotrle();
            FillDatabaseQuotes();
            if (db.SaveChanges() > 0)
            {
                MessageBox.Show("Dodano pytania z pliku Excel");
            }
        }

        private void FillDatabaseLotrle()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Helpers\Lotrle.xlsx");
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
                List<Word> list = new();

                list = (from DataRow dr in Data.Rows
                        select new Word()
                        {
                            CurrentWord = dr["Word"].ToString().ToUpper(),
                            Hint = dr["Hint"] == null ? string.Empty : dr["Hint"].ToString(),
                            Done = 0

                        }).ToList();

                foreach (Word item in list)
                {
                    db.Words.Add(item);
                }
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Coś poszło nie tak");

            }
        }

        private void FillDatabaseQuiz()
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
                            QuestionTitle = dr["Question"].ToString(),
                            QuestionDifficulty = dr["Difficulty"].ToString(),
                            FirstAnswer = dr["Answer1"].ToString(),
                            SecondAnswer = dr["Answer2"].ToString(),
                            ThirdAnswer = dr["Answer3"].ToString(),
                            FourthAnswer = dr["Answer4"].ToString(),
                            CorrectAnswer = dr["CorrectAnswer"].ToString(),
                            Done = 0

                        }).ToList();

                foreach (Question item in list)
                {
                    db.Questions.Add(item);
                }

            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Coś poszło nie tak");

            }
        }

        private void FillDatabaseQuotes()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Helpers\Quotes.xlsx");
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
                List<Quote> list = new();

                list = (from DataRow dr in Data.Rows
                        select new Quote()
                        {
                            CurrentQuote = dr["Quote"].ToString(),
                            Author = dr["Author"].ToString()

                        }).ToList();

                foreach (Quote item in list)
                {
                    db.Quotes.Add(item);
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

        #endregion

        #region Properties
        private bool _isRegister = false;

        public bool IsRegister
        {
            get { return _isRegister; }
            set
            {
                _isRegister = value;
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

                if (value)
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
            set
            {
                visibility = value;
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
