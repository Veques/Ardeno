using Ardeno.Commands;
using Ardeno.Helpers.Enums;
using Ardeno.Models;
using Ardeno.Services;
using Ardeno.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace Ardeno.ViewModels
{
    public class QuizViewModel : BaseViewModel
    {

        #region Fields
        private readonly ApplicationDbContext db = new();

        private Question _currentQuestion;
        private readonly string _difficulty;
        private bool _gameStarted = false;


        public ICommand NavigateBack { get; set; }
        public ICommand NextQuestion { get; set; }
        public ICommand CheckCommand { get; set; }
        public ICommand StartCommand { get; set; }

        #endregion

        #region Constructor
        public QuizViewModel(string parameter, NavigationStore navigationStore)
        {
            _difficulty = parameter;

            NavigateBack = new NavigationCommand<GameTypeViewModel>(new NavigationService<GameTypeViewModel>(
                navigationStore, () => new GameTypeViewModel(navigationStore)), x => true);

            NextQuestion = new RelayCommand(x => AnotherQuestion(), x => true);
            CheckCommand = new RelayCommand(CheckQuestion, x => _gameStarted);
            StartCommand = new RelayCommand(x => StartGame(), x => true);

        }

        #endregion

        #region QuizMethods
        private void StartGame()
        {
            StartVisibility = Visibility.Hidden;
            ProgressBarVisibility = Visibility.Visible;
            _gameStarted = true;
            LoadNewQuestion(_difficulty);

        }
        private void LoadNewQuestion(string difficulty)
        {
            CheckIfAllDone();

            var row = db.Questions.Where(x => x.QuestionDifficulty == difficulty && x.Done == 0).FirstOrDefault();

            Question = row.QuestionTitle;
            Answer1 = row.FirstAnswer;
            Answer2 = row.SecondAnswer;
            Answer3 = row.ThirdAnswer;
            Answer4 = row.FourthAnswer;

            _currentQuestion = row;
            IsAnimating = true;
        }
        private void AnotherQuestion()
        {
            IsCorrect1 = 0; 
            IsCorrect2 = 0; 
            IsCorrect3 = 0; 
            IsCorrect4 = 0;

            _currentQuestion.Done = 1;
            db.SaveChanges();
            IsAnimating = false;
            ShowRightAnswer(_currentQuestion.CorrectAnswer);
            ResetButtonsAndStartGame();
        }
        private void CheckIfAllDone()
        {
            if(!db.Questions.Any(x => x.Done == 0 && x.QuestionDifficulty == _difficulty))
            {
                foreach(var question in db.Questions.Where(x => x.QuestionDifficulty == _difficulty))
                {
                    question.Done = 0;
                }
                    db.SaveChanges();
            }
        }
        private void CheckQuestion(object parameter)
        {
            var clickedButton = parameter as String;
            var correctAnswer = _currentQuestion.CorrectAnswer;

            var playerAnswer = "";

            switch (clickedButton)
            {

                //1 - tak , 2 - nie
                case "Button1":
                    {
                        if (Answer1.Equals(correctAnswer))
                        {
                            IsCorrect1 = 1;
                            IsCorrect2 = 2;
                            IsCorrect3 = 2;
                            IsCorrect4 = 2;
                        }
                        else
                        {
                            ShowRightAnswer(correctAnswer);
                        }
                    }
                    break;
                case "Button2":
                    {
                        if (Answer2.Equals(correctAnswer))
                        {
                            IsCorrect1 = 2;
                            IsCorrect2 = 1;
                            IsCorrect3 = 2;
                            IsCorrect4 = 2;
                        }
                        else
                        {
                            ShowRightAnswer(correctAnswer);
                        }
                    }
                    break;
                case "Button3":
                    {
                        if (Answer3.Equals(correctAnswer))
                        {
                            IsCorrect1 = 2;
                            IsCorrect2 = 2;
                            IsCorrect3 = 1;
                            IsCorrect4 = 2;
                        }
                        else
                        {
                            ShowRightAnswer(correctAnswer);
                        }
                    }
                    break;
                case "Button4":
                    {
                        if (Answer4.Equals(correctAnswer))
                        {
                            IsCorrect1 = 2;
                            IsCorrect2 = 2;
                            IsCorrect3 = 2;
                            IsCorrect4 = 1;
                        }
                        else
                        {
                            ShowRightAnswer(correctAnswer);
                        }
                    }
                    break;
            }
            IsAnimating = false;
            _currentQuestion.Done = 1;

             db.SaveChanges();
            ResetButtonsAndStartGame();
        }
        private void ResetButtonsAndStartGame()
        {
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                IsCorrect1 = 0;
                IsCorrect2 = 0;
                IsCorrect3 = 0;
                IsCorrect4 = 0;
                LoadNewQuestion(_difficulty);
    });
        }

        private void ShowRightAnswer(string correctAnswer)
        {
            if (correctAnswer == null) return;

            if (correctAnswer.Equals(Answer1))
            {
                IsCorrect1 = 1;
                IsCorrect2 = 2;
                IsCorrect3 = 2;
                IsCorrect4 = 2;
            }
            else if (correctAnswer.Equals(Answer2))
            {
                IsCorrect1 = 2;
                IsCorrect2 = 1;
                IsCorrect3 = 2;
                IsCorrect4 = 2;
            }
            else if (correctAnswer.Equals(Answer3))
            {
                IsCorrect1 = 2;
                IsCorrect2 = 2;
                IsCorrect3 = 1;
                IsCorrect4 = 2;
            }
            else
            {
                IsCorrect1 = 2;
                IsCorrect2 = 2;
                IsCorrect3 = 2;
                IsCorrect4 = 1;
            }
        }
        public void OnProgressBarAnimationFinish()
        {
            //getting called from view on AnimationFinish
            ShowRightAnswer(_currentQuestion.CorrectAnswer);
            IsAnimating = false;
        }
        #endregion

        #region Properties

        private string _question ;

        public string Question
        {
            get { return _question; }
            set { _question = value;
                OnPropertyChanged(nameof(Question));
            }
        }

        private string _answer1 ;

        public string Answer1
        {
            get { return _answer1; }
            set
            {
                _answer1 = value;
                OnPropertyChanged(nameof(Answer1));
            }
        }

        private string _answer2 ;

        public string Answer2
        {
            get { return _answer2; }
            set
            {
                _answer2 = value;
                OnPropertyChanged(nameof(Answer2));
            }
        }

        private string _answer3 ;

        public string Answer3
        {
            get { return _answer3; }
            set
            {
                _answer3 = value;
                OnPropertyChanged(nameof(Answer3));
            }
        }

        private string _answer4 ;

        public string Answer4
        {
            get { return _answer4; }
            set
            {
                _answer4 = value;
                OnPropertyChanged(nameof(Answer4));
            }
        }

        private int _isCorrect1;

        public int IsCorrect1
        {
            get { return _isCorrect1; }
            set { _isCorrect1 = value;
                OnPropertyChanged(nameof(IsCorrect1));
            }
        }

        private int _isCorrect2;

        public int IsCorrect2
        {
            get { return _isCorrect2; }
            set
            {
                _isCorrect2 = value;
                OnPropertyChanged(nameof(IsCorrect2));
            }
        }

        private int _isCorrect3;

        public int IsCorrect3
        {
            get { return _isCorrect3; }
            set
            {
                _isCorrect3 = value;
                OnPropertyChanged(nameof(IsCorrect3));
            }
        }

        private int _isCorrect4;

        public int IsCorrect4
        {
            get { return _isCorrect4; }
            set
            {
                _isCorrect4 = value;
                OnPropertyChanged(nameof(IsCorrect4));
            }
        }

        private bool _isAnimating;

        public bool IsAnimating
        {
            get { return _isAnimating; }
            set { _isAnimating = value;
                OnPropertyChanged(nameof(IsAnimating));
            }
        }

        private Visibility _startVisibility = Visibility.Visible;

        public Visibility StartVisibility
        {
            get { return _startVisibility; }
            set { _startVisibility = value;
                OnPropertyChanged(nameof(StartVisibility));
            }
        }

        private Visibility _progressBarVisiblity = Visibility.Collapsed;

        public Visibility ProgressBarVisibility
        {
            get { return _progressBarVisiblity; }
            set
            {
                _progressBarVisiblity = value;
                OnPropertyChanged(nameof(ProgressBarVisibility));
            }
        }



        #endregion
    }
}
