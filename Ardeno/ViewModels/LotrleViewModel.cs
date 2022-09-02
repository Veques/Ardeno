using Ardeno.Commands;
using Ardeno.Services;
using Ardeno.Stores;
using Ardeno.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ardeno.ViewModels
{
    public class LotrleViewModel : BaseViewModel
    {
        public ICommand NavigateBack { get; set; }
        public LotrleViewModel(NavigationStore navigationStore)
        {

            NavigateBack = new NavigationCommand<GameTypeViewModel>(new NavigationService<GameTypeViewModel>(
                navigationStore, () => new GameTypeViewModel(navigationStore)), x => true);
        }
    }
}
