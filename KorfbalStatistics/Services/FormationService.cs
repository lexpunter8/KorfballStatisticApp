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

                EPlayerFunction currentFunction;
                if (f.CurrentFunction.Equals("A"))
                    currentFunction = EPlayerFunction.Attack;
                else if (f.CurrentFunction.Equals("D"))
                    currentFunction = EPlayerFunction.Defence;
                else
                    currentFunction = EPlayerFunction.Substitute;

                EPlayerFunction startFunction;
                if (f.StartFunction.Equals("A"))
                    startFunction = EPlayerFunction.Attack;
                else if (f.StartFunction.Equals("D"))
                    startFunction = EPlayerFunction.Defence;
                else
                    startFunction = EPlayerFunction.Substitute;

                players.Add(new Player
                {
                    Id = dbPlayer.Id,
                    Abbrevation = dbPlayer.Abbrevation,
                    Number = dbPlayer.Number,
                    CurrentZoneFunction = currentFunction,
                    StartZoneFunction = startFunction,
                    Name = dbPlayer.Name,
                    FirstName = dbPlayer.FirstName
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
                if (f.CurrentFunction.Equals("A"))
                    attackersCount++;
                if (f.CurrentFunction.Equals("D"))
                    defendersCount++;
            });

            return attackersCount == 4 && defendersCount == 4 ? true : false;
        }
    }
}