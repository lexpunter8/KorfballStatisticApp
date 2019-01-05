using System;
using System.Collections.Generic;
using System.Linq;
using KorfbalStatistics.Interface;
using static KorfbalStatistics.Model.Enums;

namespace KorfbalStatistics.Model
{
    public class Attack : IAttack
    {
        public Attack(EZoneFunction function, string startingZoneFunction, Guid gameId, bool isFirstHalf)
        {
            Init(function, startingZoneFunction, gameId, isFirstHalf);
        }

        public Attack(EZoneFunction function, EZoneFunction startingZoneFunction, Guid gameId, bool isFirstHalf)
        {
            string startingFunction = startingZoneFunction == EZoneFunction.Attack ? "A" : "D";
            Init(function, startingFunction, gameId, isFirstHalf);
        }
        private void Init(EZoneFunction function, string startingZoneFunction, Guid gameId, bool isFirstHalf)
        {
            DbAttack.IsOpponentAttack = function == EZoneFunction.Defence ? true : false;
            DbAttack.ZoneStartFunction = startingZoneFunction;
            DbAttack.Id = Guid.NewGuid();
            DbAttack.GameId = gameId;
            DbAttack.IsFirstHalf = isFirstHalf;
        }
        public DbAttack DbAttack { get; set; } = new DbAttack();
        public List<DbAttackRebound> Rebounds { get; set; } = new List<DbAttackRebound>();
        public List<DbAttackShot> Shots { get; set; } = new List<DbAttackShot>();
        public DbAttackGoal Goal { get; set; } = null;

        public void AddGoal(Guid playerId, Guid assistId, Guid goalTypeId)
        {
            Goal = new DbAttackGoal
            {
                Id = Guid.NewGuid(),
                PlayerId = playerId,
                AssistPlayerId = assistId,
                GoalTypeId = goalTypeId
            };
            DbAttack.GoalId = Goal.Id;
        }

        public void ShotclockOverride(bool v)
        {
            DbAttack.IsSchotClockOverride = v;
        }

        public void Turnover(Guid playerId)
        {
            DbAttack.TurnoverPlayerId = playerId;
        }

        public void Shot(Guid shotPlayerId, Guid reboundPlayerId = new Guid())
        {
            DbAttackShot playerShot = Shots.FirstOrDefault(p => p.PlayerId.Equals(shotPlayerId));
            if (playerShot == null)
                Shots.Add(new DbAttackShot
                {
                    Id = Guid.NewGuid(),
                    AttackId = DbAttack.Id,
                    PlayerId = shotPlayerId,
                    Count = 1
                });
            else
                playerShot.Count++;

            if (reboundPlayerId == Guid.Empty)
                return;

            IDbAttackRebound playerRebound = Rebounds.FirstOrDefault(p => p.PlayerId.Equals(reboundPlayerId));
            if (playerRebound == null)
                Rebounds.Add(new DbAttackRebound
                {
                    Id = Guid.NewGuid(),
                    AttackId = DbAttack.Id,
                    PlayerId = reboundPlayerId,
                    Count = 1
                });
            else
                playerRebound.Count++;
        }

        public int ShotCount()
        {
            int shotCount = 0;
            Shots.ForEach(s =>
            {
                shotCount += s.Count;
            });
            if (Goal != null)
                shotCount++;
            return shotCount;
        }
        public int ReboundCount()
        {
            int reboundCount = 0;
            Rebounds.ForEach(s =>
            {
                reboundCount += s.Count;
            });
            return reboundCount;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

    }

}