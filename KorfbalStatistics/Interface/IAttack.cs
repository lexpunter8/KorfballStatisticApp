using KorfbalStatistics.Model;
using System;
using System.Collections.Generic;

namespace KorfbalStatistics.Interface
{
    public interface IAttack : ICloneable
    {
        DbAttack DbAttack { get; set; }
        List<DbAttackRebound> Rebounds { get; set; }
        List<DbAttackShot> Shots { get; set; }
        DbAttackGoal Goal { get; set; }

        void AddGoal(Guid playerId, Guid assistId, Guid goalTypeId);

        void ShotclockOverride(bool v);

        void Turnover(Guid playerId);

        void Shot(Guid shotPlayerId, Guid reboundPlayerId = new Guid());
    }
}