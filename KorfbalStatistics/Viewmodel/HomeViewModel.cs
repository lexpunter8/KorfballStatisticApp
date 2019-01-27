using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using KorfbalStatistics.Interface;
using KorfbalStatistics.Model;
using KorfbalStatistics.RemoteDb;

namespace KorfbalStatistics.Viewmodel
{
    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel(DbManager dbManager)
        {
            myGameDbManager = dbManager.GameDbManager;
        }

        private IGameDbManager myGameDbManager { get; set; }

        public void GetGames()
        {
            Games.Clear();
            var games = myGameDbManager.GetGamesForTeam(MainViewModel.Instance.Team.Id);
            if (games == null)
                return;
            Games.AddRange(games);
            OnPropertyChanged(nameof(Games));
        }

        public void CreateGame(string opponent, DateTime date, bool isHome)
        {
            DbGame game = new DbGame
            {
                Id = Guid.NewGuid(),
                Opponent = opponent,
                TeamId = MainViewModel.Instance.Team.Id,
                IsHome = isHome,
                Status = "H1",
                Date = date
            };
            myGameDbManager.AddGame(game);
            GetGames();
        }
        
        public List<DbGame> Games { get; } = new List<DbGame>
        {
             new DbGame {Id = new Guid(), IsHome = true, Date = DateTime.Now, Opponent = "Nothing to show", TeamId = new Guid()},

        };

        
    }

    
    
}