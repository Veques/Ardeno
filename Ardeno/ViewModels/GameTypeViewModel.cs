using Ardeno.Commands;
using Ardeno.Helpers.Enums;
using Ardeno.Services;
using Ardeno.Stores;
using Microsoft.EntityFrameworkCore.Metadata;
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
    public class GameTypeViewModel : BaseViewModel
    {
        public string QuizDifficulty { get; set; }
        public ICommand NavigateLotrle { get; set; }
        public ICommand NavigateQuiz { get; set; }
        public ICommand CheckDifficulty { get; set; }
        public ICommand QuizClickedCommand { get; set; }
        public GameTypeViewModel(NavigationStore navigationStore)
        {
            ParameterNavigationService<String, QuizViewModel> NavigateWithParameter = new(navigationStore, (parameter) => new QuizViewModel(parameter, navigationStore));

            NavigateLotrle = new NavigationCommand<LotrleViewModel>(new NavigationService<LotrleViewModel>(
                navigationStore, () => new LotrleViewModel(navigationStore)), x => true);

            NavigateQuiz = new PassDifficultyNavigationCommand(NavigateWithParameter, this);

            QuizClickedCommand = new RelayCommand(x => ShowDifficulty(), x => true);

            CheckDifficulty = new RelayCommand(ChooseDifficultyQuiz, x => true);

        }
        

    private void ChooseDifficultyQuiz(object parameter)
    {
        var difficulty = parameter as String;


        switch (difficulty)
        {
            case "Easy": QuizDifficulty = "Easy";
                break;
            case "Medium": QuizDifficulty = "Medium";
                break;
            case "Hard": QuizDifficulty = "Hard";
                break;
        }

            NavigateQuiz.Execute(this);
    }

        private void ShowDifficulty()
        {
            if (DifficultyQuizVisibility == Visibility.Visible)
            {
                Task.Run(() =>
                {
                    Thread.Sleep(550);
                    DifficultyQuizVisibility = Visibility.Hidden;
                });
            }
            else
            {
                DifficultyQuizVisibility = Visibility.Visible;
            }

            StartAnimationQuiz ^= true;
        }


        #region Properties
        private bool _quizzAnimation = false;

        public bool StartAnimationQuiz
        {
            get { return _quizzAnimation; }
            set { _quizzAnimation = value;
                OnPropertyChanged(nameof(StartAnimationQuiz));
            }
        }

        private Visibility  _quizDiffVisibility = Visibility.Hidden;

        public Visibility DifficultyQuizVisibility
        {
            get { return _quizDiffVisibility; }
            set { _quizDiffVisibility = value;
                OnPropertyChanged(nameof(DifficultyQuizVisibility));
            }
        }
        #endregion

    }
}
