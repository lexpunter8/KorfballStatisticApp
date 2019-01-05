using KorfbalStatistics.Interface;
using KorfbalStatistics.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using static KorfbalStatistics.Model.Enums;

namespace KorfbalStatistics.Services
{
    public class GameService : IGameService
    {
        private readonly GameDbManager myGameDbManager;

        public GameService(GameDbManager gameDbManager)
        {
            myGameDbManager = gameDbManager;
        }

        public void AddAttackToDb(IAttack attack)
        {
            myGameDbManager.AddAttack(attack.DbAttack);

            if (attack.Shots.Count > 0)
                attack.Shots.ForEach(s =>
                {
                    myGameDbManager.AddShot(s);
                });
            if (attack.Rebounds.Count > 0)
                attack.Rebounds.ForEach(r =>
                {
                    myGameDbManager.AddRebound(r);
                });
            if (attack.Goal != null)
            {
                myGameDbManager.AddGoal(attack.Goal);
            }

        }

        public void RemoveAttackFromDb(IAttack attack)
        {

            if (attack.Shots.Count > 0)
                attack.Shots.ForEach(s =>
                {
                    myGameDbManager.RemoveShot(s);
                });
            if (attack.Rebounds.Count > 0)
                attack.Rebounds.ForEach(r =>
                {
                    myGameDbManager.RemoveRebound(r);
                });
            if (attack.Goal != null)
            {
                myGameDbManager.RemoveGoal(attack.Goal);
            }
            myGameDbManager.RemoveAttack(attack.DbAttack);
        }

        public List<Attack> GetAttacksByGameId(Guid gameId, EZoneFunction zoneFilter)
        {
            List<Attack> attacks = new List<Attack>(myGameDbManager.GetFullAttackForGame(gameId));

            if (zoneFilter == EZoneFunction.None)
                return attacks;
            string zoneStartFunctionFilter = zoneFilter == EZoneFunction.Attack ? "A" : "D";
            return attacks.Where(a => a.DbAttack.ZoneStartFunction.Equals(zoneStartFunctionFilter)).ToList();
        }

        public DbGoalType GetGoalTypeById(Guid goalTypeId)
        {
            return myGameDbManager.GetGoalTypeById(goalTypeId);
        }
    }
}