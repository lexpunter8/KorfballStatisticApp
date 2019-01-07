using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using KorfbalStatistics.Interface;
using KorfbalStatistics.Model;

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
            Games.AddRange(myGameDbManager.GetGames());
            OnPropertyChanged(nameof(Games));
        }

        public void CreateGame(string opponent, DateTime date, bool isHome)
        {
            DbGame game = new DbGame
            {
                Id = Guid.NewGuid(),
                Opponent = opponent,
                TeamId = myGameDbManager.GetTeamIdByUserId(MainViewModel.Instance.LoggedInUser.Id),
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