using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using KorfbalStatistics.Model;
using KorfbalStatistics.Services;

namespace KorfbalStatistics.Viewmodel
{
    public class PlayersViewModel
    {
        public PlayersViewModel()
        {
            try
            {

                PlayersService service = ServiceLocator.GetService<PlayersService>();
                Players.AddRange(service.GetPlayersForTeamId(MainViewModel.Instance.Team.Id));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public List<DbPlayer> Players { get; private set; } = new List<DbPlayer>();

        public void UpdatePlayer(DbPlayer player)
        {
            DbManager.Instance.PlayerDbManager.UpdatePlayer(player);
        }

        public void AddPlayer(DbPlayer player)
        {
            DbManager.Instance.PlayerDbManager.AddPlayer(player);
        }
    }
}