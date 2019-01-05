

using System;
using System.Collections.Generic;
using System.Linq;
using KorfbalStatistics.Interface;
using KorfbalStatistics.Model;

namespace KorfbalStatistics.Viewmodel
{
    public class StatisticOverViewViewModel : BaseViewModel
    {
        public StatisticOverViewViewModel(DbManager dbManager)
        {
            myDbManager = dbManager.GameDbManager;
            myGameId =  MainViewModel.Instance.CurrentGame.Id;
            Attacks = new List<Attack>();
            Attacks = myDbManager.GetFullAttackForGame(myGameId).ToList();
        }

        private IGameDbManager myDbManager { get; set; }

        private Guid myGameId { get; set; }
        public List<Attack> Attacks { get; internal set; }
        public List<IStatisticViewModel> StatisticViewModels = new List<IStatisticViewModel>
        {
            new PlayerStatisticViewModel(Guid.Empty),
            new ZoneStatisticViewModel(),
            new TeamStatisticViewModel()
        };

        public IStatisticViewModel CurrentStatisticViewModel;

        public void SetTab(int tabId)
        {
            CurrentStatisticViewModel = StatisticViewModels[tabId];
        }
    }
}