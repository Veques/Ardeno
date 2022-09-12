using Ardeno.Commands;
using Ardeno.Models;
using Ardeno.Services;
using Ardeno.Stores;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace Ardeno.ViewModels
{

    public class Score
    {
        public string Player { get; set; }
        public int QuizzScore { get; set; }
        public int LotrleScore { get; set; }
        public int CombinedScore { get; set; }
    }

    public class ScoreboardViewModel : BaseViewModel
    {
        private readonly ApplicationDbContext db = new();
        public ICommand NavigateBack { get; set; }

        public ScoreboardViewModel(NavigationStore navigationStore)
        {

            Scores = new();
            FillDataGrid();
            ScoresCollectionView = CollectionViewSource.GetDefaultView(Scores);

            NavigateBack = new NavigationCommand<GameTypeViewModel>(new NavigationService<GameTypeViewModel>(
                navigationStore, () => new GameTypeViewModel(navigationStore)), x => true);
        }

        private void FillDataGrid()
        {
            foreach (var user in db.Users)
            {
                Scores.Add(new Score
                {
                    Player = user.Username,
                    LotrleScore = user.LotrleScore,
                    QuizzScore = user.QuizScore,
                    CombinedScore = user.QuizScore + user.LotrleScore
                });
            }
        }
        #region Properties
        private ICollectionView _scoresCollectionView;

        public ICollectionView ScoresCollectionView
        {
            get { return _scoresCollectionView; }
            set
            {
                _scoresCollectionView = value;
                OnPropertyChanged(nameof(ScoresCollectionView));
            }
        }

        private ObservableCollection<Score> _scores;

        public ObservableCollection<Score> Scores
        {
            get { return _scores; }
            set
            {
                _scores = value;
                OnPropertyChanged(nameof(Scores));
            }
        }
        #endregion
    }
}
