using Ardeno.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardeno.Stores
{
    public class NavigationStore
    {
        public event Action CurrentViewModelChanged;


        private BaseViewModel _baseViewModel;

        public BaseViewModel CurrentViewModel 
        { 
            get => _baseViewModel; 
            set
            {
                _baseViewModel = value; 
                OnCurrentViewModelChanged();
            } 
            
        }
        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}
