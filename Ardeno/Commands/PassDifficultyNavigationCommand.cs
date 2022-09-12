using Ardeno.Services;
using Ardeno.ViewModels;
using System;

namespace Ardeno.Commands
{
    public class PassDifficultyNavigationCommand : CommandBase
    {

        private readonly ParameterNavigationService<String, QuizViewModel> _navigationService;
        private readonly GameTypeViewModel _viewModel;

        public PassDifficultyNavigationCommand(ParameterNavigationService<String, QuizViewModel> navigationService, GameTypeViewModel viewModel)
        {
            _navigationService = navigationService;
            _viewModel = viewModel;
        }


        public override void Execute(object parameter)
        {
            var difficulty = _viewModel.QuizDifficulty;

            _navigationService.Navigate(difficulty);
        }
    }
}
