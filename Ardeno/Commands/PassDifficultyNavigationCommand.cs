using Ardeno.Helpers.Enums;
using Ardeno.Services;
using Ardeno.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
