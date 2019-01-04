using KorfbalStatistics.Interface;
using KorfbalStatistics.Model;
using System;

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

        public string GetGoalTypeById(Guid goalTypeId)
        {
            return myGameDbManager.GetGoalTypeById(goalTypeId).Name;
        }
    }
}