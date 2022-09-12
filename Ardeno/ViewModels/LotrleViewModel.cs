using Ardeno.Commands;
using Ardeno.Models;
using Ardeno.Services;
using Ardeno.Stores;
using Ardeno.Views;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Ardeno.ViewModels
{
    #region Model
    public class Letters
    {
        public int Column { get; set; }
        public int Row { get; set; }
        public Button Button { get; set; }
    }
    #endregion
    public class LotrleViewModel : BaseViewModel
    {
        #region Fields
        //Word to guess
        private string _currentWord;
        //Actual Row
        private static int  _currentRow;
        //Actual Column
        private static int _currentColumn;

        private bool _hasEnded = false;

        private readonly Style _buttonStyle = (Style)Application.Current.Resources["LotrleButtons"];
        private readonly Storyboard _inputAnimation = (Storyboard)Application.Current.Resources["InputAnimation"];
        private readonly Storyboard _orangeAnimation = (Storyboard)Application.Current.Resources["OrangeLetter"];
        private readonly Storyboard _greenAnimation = (Storyboard)Application.Current.Resources["GreenLetter"];

        private readonly ApplicationDbContext db = new();

        public event EventHandler AnimationCalled;
        public ICommand KeyPressed { get; set; }
        public ICommand NavigateBack { get; set; }
        public ICommand AnotherWordCommand { get; set; }
        public ICommand HintCommand { get; set; }
        public ICommand InformationCommand { get; set; }

        #endregion

        #region Contructor
        public LotrleViewModel(NavigationStore navigationStore)
        {
            KeyPressed = new RelayCommand(GetLetter, x => true);

            NavigateBack = new NavigationCommand<GameTypeViewModel>(new NavigationService<GameTypeViewModel>(
                navigationStore, () => new GameTypeViewModel(navigationStore)), x => true);
            AnotherWordCommand = new RelayCommand(x => AnotherWord(), x => true);

            HintCommand = new RelayCommand(x => GetHint(), x => true);
            InformationCommand = new RelayCommand(x => InformationWindow(), x => true);

            LoadNewGame();
        }


        #endregion

        #region GameMethods
        private void AnotherWord()
        {
            db.Words.FirstOrDefault(x => x.CurrentWord == _currentWord).Done = 1;
            db.SaveChanges();
            Hint = string.Empty;
            LoadNewGame();
        }
        private void LoadNewGame()
        {
            PointsVisibility = Visibility.Hidden;
            GetNewWord();
            LoadGridAsync();
            _currentColumn = 1;
            _currentRow = 1;
        }
        private void Play(string letter)
        {
           
            //Dynamic button change
            var currentButton = Letters.Where(x => x.Row == _currentRow && x.Column == _currentColumn).FirstOrDefault();

            //Backspace click
            if(letter.Equals("BACK"))
            {
                var deleteButton = Letters.Where(x => x.Row == _currentRow && x.Column == _currentColumn - 1).FirstOrDefault();

                    if (deleteButton == null)
                    {
                        return;
                    }
                deleteButton.Button.Content = "";
                _currentColumn--;
                currentButton = deleteButton;
                currentButton.Button.BeginStoryboard(_inputAnimation);

                return;
            }

            //Enter to check if guessed
            if(letter.Equals("RETURN") && Letters.Where(x => x.Row == _currentRow).Any(x => x.Button.Content != "" && _currentColumn > _currentWord.Length ))
            {
                var word = string.Empty;

                //create a word from letters written
                foreach (var button in Letters.Where(x => x.Row == _currentRow))
                {
                    word += button.Button.Content;
                }

                //if there is no word like this return
                if (!db.Words.Any(x => x.CurrentWord.Equals(word)))
                {
                    return;
                }

                //if word exist change buttons background
                foreach (var button in Letters.Where(x => x.Row == _currentRow))
                {
                    if (_currentWord.Contains(button.Button.Content.ToString()) && button.Button.Content.ToString() != _currentWord[button.Column - 1].ToString())
                    {
                        button.Button.BeginStoryboard(_orangeAnimation);
                    }

                    if (_currentWord.Contains(button.Button.Content.ToString()) && button.Button.Content.ToString() == _currentWord[button.Column - 1].ToString())
                    {
                        button.Button.BeginStoryboard(_greenAnimation);
                    }
                }
                //Potentaial LOSE
                if (_currentRow == 6 && !_hasEnded)
                {
                    MessageBox.Show("Przegrałeś");
                    return;
                }

                //WIN
                if (_currentWord.Equals(word))
                {
                    MessageBox.Show("Wygrałeś");
                    db.Words.Single(x => x.CurrentWord == word).Done = 1;
                    db.SaveChanges();
                    _hasEnded = true;
                    AddPoints();
                    return;
                }

                //Another round if not guessed
                _currentRow++;
                _currentColumn = 1;

                return;
            }
            
            //Prevent Null button and characters other than letters
            if (_currentColumn > _currentWord.Length || letter.Length > 1)
            {
                return;
            }

            currentButton.Button.Content = letter;
            currentButton.Button.BeginStoryboard(_inputAnimation);

            //Another Column
            _currentColumn++;

        }
        private void AddPoints()
        {
            var loggedUser = LoginViewModel.LoggedUser;

            PointsVisibility = Visibility.Visible;
            db.Users.Single(x => x.Username == loggedUser).LotrleScore += 10;
            db.SaveChanges();


        }
        private void LoadGridAsync()
        {
            Letters = new();

            for (int i = 1; i <= 6; i++)
            {
                for(int j = 1; j <= _currentWord.Length; j ++)
                {
                    Letters.Add(new ViewModels.Letters
                    {
                        Row = i,
                        Column = j,
                        Button = new Button { Content = "", Style = _buttonStyle }
                    });
                }
            }
            Hint = string.Empty;
        }
        private void GetLetter(object? parameter)
        {
            var letter = (parameter as KeyEventArgs).Key.ToString().ToUpper();

            Play(letter);
        }
        private void GetNewWord()
        {
            CheckIfAllDone();

            _currentWord = db.Words.FirstOrDefault(x => x.Done == 0).CurrentWord.ToUpper();
        }
        private void CheckIfAllDone()
        {
            if (!db.Words.Any(x => x.Done == 0))
            {
                foreach (var word in db.Words)
                {
                    word.Done = 0;
                }
                db.SaveChanges();
            }
        }
        private static void InformationWindow()
        {
            InformationWindow window = new();
            window.ShowDialog();
        }
        private void GetHint()
        {
            Hint = db.Words.Single(x => x.CurrentWord == _currentWord).Hint;
        }
        protected virtual void OnAnimationCalled()
        {
            AnimationCalled?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Properties

        private ObservableCollection<Letters> _letters;

        public ObservableCollection<Letters> Letters
        { 
            get { return _letters; }
            set {
                _letters = value;
                OnPropertyChanged(nameof(Letters));
            }
        }

        private string _hint;

        public string Hint
        {
            get { return _hint; }
            set { _hint = value;
                OnPropertyChanged(nameof(Hint));
            }
        }

        private Visibility visibility = Visibility.Collapsed;

        public Visibility PointsVisibility
        {
            get { return visibility; }
            set { visibility = value;
                OnPropertyChanged(nameof(PointsVisibility));
            }
        }


        #endregion
    }
}
