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
    public class FormationViewModel
    {
        public FormationViewModel(FormationDbManager formationDbManager)
        {
            myGame = MainViewModel.Instance.CurrentGame;
            Guid teamId = DbManager.Instance.PlayerDbManager.GetTeamIdByUserId(MainViewModel.Instance.LoggedInUser.Id);
            Players.AddRange(ServiceLocator.GetService<PlayersService>().GetPlayersForTeamId(teamId));
            myFormationDbManager = formationDbManager;
        }
        private List<DbPlayer> Players { get; set; } = new List<DbPlayer>();
        private List<DbPlayer> myDefensivePlayers = new List<DbPlayer>();
        private List<DbPlayer> myAttacingPlayers = new List<DbPlayer>();
        private DbGame myGame;
        private readonly FormationDbManager myFormationDbManager;

        public List<DbPlayer> GetPlayers()
        {
            List<DbPlayer> players = new List<DbPlayer>(Players);
            players.RemoveAll(p => myDefensivePlayers.Contains(p) || myAttacingPlayers.Contains(p));
            return players;
        }

        public void AddDefensivePlayer(DbPlayer player)
        {
            myDefensivePlayers.Add(player);
        }

        public void AddAttackingPlayer(DbPlayer player)
        {
            myAttacingPlayers.Add(player);
        }

        public bool SaveFormation()
        {
            if (myAttacingPlayers.Count != 4 || myDefensivePlayers.Count != 4)
                return false;
            List<DbFormation> formation = new List<DbFormation>();
            myAttacingPlayers.ForEach(p =>
            {
                formation.Add(new DbFormation
                {
                    Id = Guid.NewGuid(),
                    Function = "A",
                    GameId = myGame.Id,
                    PlayerId = p.Id
                });
            });

            myDefensivePlayers.ForEach(p =>
            {
                formation.Add(new DbFormation
                {
                    Id = Guid.NewGuid(),
                    Function = "D",
                    GameId = myGame.Id,
                    PlayerId = p.Id
                });
            });

            Players.ForEach(p =>
            {
                if (formation.Any(f => f.PlayerId == p.Id))
                    return;
                formation.Add(new DbFormation
                {
                    Id = Guid.NewGuid(),
                    Function = "S",
                    GameId = myGame.Id,
                    PlayerId = p.Id
                });
            });
            myFormationDbManager.AddFormation(formation);
            return true;
        }
    }
}