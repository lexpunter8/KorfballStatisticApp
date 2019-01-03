using Android.App;
using Android.OS;
using KorfbalStatistics.Model;
using KorfbalStatistics.Viewmodel;
using KorfbalStatistics.Fragments;
using KorfbalStatistics.Services;

namespace KorfbalStatistics
{
    [Activity(Label = "GameStatisticsActivity")]
    public class GameStatisticsActivity : Activity
    {
        private GameStatisticViewModel myViewModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_gamestatistics);
            // Create your application here
            myViewModel = new GameStatisticViewModel(DbManager.Instance);

            var trans = FragmentManager.BeginTransaction();
            if (ServiceLocator.GetService<FormationService>().GameHasFormation(MainViewModel.Instance.CurrentGame.Id))
                trans.Add(Resource.Id.fragmentContainer, new GameStatisticsFragment());
            else
                trans.Add(Resource.Id.fragmentContainer, new FormationFragment());
            trans.Commit();
        }
    }
}