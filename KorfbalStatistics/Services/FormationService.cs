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
using static KorfbalStatistics.Model.Enums;

namespace KorfbalStatistics.Services
{
    public class FormationService
    {
        private readonly FormationDbManager myFormationDbManager;

        public FormationService(FormationDbManager formationDbManager)
        {
            myFormationDbManager = formationDbManager;
        }

        public Player[] GetFormationForGame(Guid gameId)
        {
            List<Player> players = new List<Player>();
            List<DbFormation> formation = myFormationDbManager.GetFormationByGameId(gameId).ToList();

            formation.ForEach(f => {

                DbPlayer dbPlayer = DbManager.Instance.PlayerDbManager.GetPlayerById(f.PlayerId);

                EPlayerFunction function;
                if (f.Function.Equals("A"))
                    function = EPlayerFunction.Attack;
                else if (f.Function.Equals("D"))
                    function = EPlayerFunction.Defence;
                else
                    function = EPlayerFunction.Substitute;

                players.Add(new Player
                {
                    Id = dbPlayer.Id,
                    Abbrevation = dbPlayer.Abbrevation,
                    Number = dbPlayer.Number,
                    ZoneFunction = function
                });
            });

            return players.ToArray();
        }

        public bool GameHasFormation(Guid gameId)
        {
            var formation = myFormationDbManager.GetFormationByGameId(gameId).ToList();

            if (formation.Count < 8)
                return false;

            int attackersCount = 0;
            int defendersCount = 0;

            formation.ForEach(f =>
            {
                if (f.Function.Equals("A"))
                    attackersCount++;
                if (f.Function.Equals("D"))
                    defendersCount++;
            });

            return attackersCount == 4 && defendersCount == 4 ? true : false;
        }
    }
}