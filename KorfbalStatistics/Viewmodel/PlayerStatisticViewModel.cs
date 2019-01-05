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
using KorfbalStatistics.Interface;
using KorfbalStatistics.Model;
using KorfbalStatistics.Services;
using OxyPlot;

namespace KorfbalStatistics.Viewmodel
{
    public class PlayerStatisticViewModel : IStatisticViewModel
    {
        public PlayerStatisticViewModel(Guid playerId)
        {
            if (playerId == Guid.Empty)
                return;
            PlayerId = playerId;
            Player = DbManager.Instance.PlayerDbManager.GetPlayerById(playerId);
        }
        private int GetShotCount(bool isOpponentAttack)
        {
            int shotCount = 0;
            myShots.ForEach(r =>
            {
                DbAttack attack = DbManager.Instance.GameDbManager.GetAttackById(r.AttackId);
                if (attack.IsOpponentAttack == isOpponentAttack)
                    shotCount += r.Count;
            });
            return shotCount + GetGoalCount(isOpponentAttack);
        }
        public int ReboundCount => GetReboundCount(false);

        private int GetReboundCount(bool isOpponentAttack)
        {
            int reboundCount = 0;
            myRebounds.ForEach(r =>
            {
                DbAttack attack = DbManager.Instance.GameDbManager.GetAttackById(r.AttackId);
                if (attack.IsOpponentAttack == isOpponentAttack)
                    reboundCount += r.Count;
            });
            return reboundCount;
        }
        public int ConcededShotCount => GetShotCount(true);
        public int ShotCount => GetShotCount(false);

        public int DevensiveReboundCount => GetReboundCount(true);

        public int GoalCount => GetGoalCount(false);
        
        private int GetGoalCount(bool isOpponent)
        {
            int goalCount = 0;
            myGoals.ForEach(g =>
            {
                DbAttack attack = myAttacks.FirstOrDefault(a =>
                {
                    if (!a.GoalId.HasValue)
                        return false;
                    return a.GoalId.Value == g.Id;
                });
                if (attack.IsOpponentAttack == isOpponent)
                    goalCount++;
            });
            return goalCount;
        }

        private int GetAssitCount(bool isOpponent)
        {
            int assistCount = 0;
            myAssist.ForEach(g =>
            {
                DbAttack attack = myAttacks.FirstOrDefault(a =>
                {
                    if (!a.GoalId.HasValue)
                        return false;
                    return a.GoalId.Value == g.Id;
                });
                if (attack.IsOpponentAttack == isOpponent)
                    assistCount++;
            });
            return assistCount;
        }

        public int ConcededGoalCount => GetGoalCount(true);

        public int InterceptionCount => myAttacks.Where(a => a.IsOpponentAttack && a.TurnoverPlayerId == PlayerId).Count();

        public int TurnoverCount => myAttacks.Where(a => !a.IsOpponentAttack && a.TurnoverPlayerId == PlayerId).Count();

        public int ShotClokcOverrideCount => throw new NotImplementedException();

        public int AttackCount => throw new NotImplementedException();

        public int AssistCount => GetAssitCount(false);

        public PlotModel PlotModel => null;

        public Enums.EZoneFunction CurrentZoneToShowFilter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Guid PlayerId { get; }
        public DbPlayer Player { get; }

        public void SetAttacks()
        {
            var db = DbManager.Instance.PlayerDbManager;
            Guid gameId = MainViewModel.Instance.CurrentGame.Id;
            myGoals.AddRange(db.GetGoalsForPlayerByGame(PlayerId, gameId));
            myAssist.AddRange(db.GetAssitsForPlayerByGame(PlayerId, gameId));
            myRebounds.AddRange(db.GetReboundsForPlayerByGame(PlayerId, gameId));
            myShots.AddRange(db.GetShotsForPlayerByGame(PlayerId, gameId));
            myAttacks.AddRange(DbManager.Instance.GameDbManager.GetAttacksForGame(gameId));
        }

        private List<DbAttackGoal> myGoals = new List<DbAttackGoal>();
        private List<DbAttackGoal> myAssist = new List<DbAttackGoal>();
        private List<DbAttackRebound> myRebounds = new List<DbAttackRebound>();
        private List<DbAttackShot> myShots = new List<DbAttackShot>();
        private List<DbAttack> myAttacks = new List<DbAttack>();
    }
}