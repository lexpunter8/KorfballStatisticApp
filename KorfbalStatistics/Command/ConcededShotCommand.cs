using System;
using System.Collections.Generic;
using System.Linq;
using KorfbalStatistics.Interface;
using KorfbalStatistics.Model;
using KorfbalStatistics.Viewmodel;
using static KorfbalStatistics.Model.Enums;

namespace KorfbalStatistics.Command
{
    public class ConcededShotCommand : AttackEndingCommand
    {
        public ConcededShotCommand(GameStatisticViewModel gameStatisticViewModel) : base(gameStatisticViewModel)
        {
            myAttack = gameStatisticViewModel.CurrentAttack;
            myOldAttack = myAttack.Clone() as Attack;

            StatisticsNeeded = new List<EStatisticType>
            {
                EStatisticType.DefensiveRebound
            };
            StatisticType = EStatisticType.ConcededShot;
        }
        private IAttack myAttack;

        public override void Execute()
        {
            Guid reboundGuid = GetStatistic(EStatisticType.DefensiveRebound);
            myAttack.Shot(Guid.Parse("4aeab093-8e88-4b41-aa4d-1aa41ba7a8fd"), reboundGuid);
            if (reboundGuid == Guid.Empty)
                base.Execute();
            IsCompleted = true;
        }

        public override void Redo()
        {
        }

        public override void Undo()
        {
            DbAttackShot shotPlayer = myAttack.Shots.FirstOrDefault();
            if (shotPlayer == null)
                return;
            if (shotPlayer.Count > 1)
                shotPlayer.Count--;
            else
                myAttack.Shots.Remove(shotPlayer);

            DbAttackRebound reboundPlayer = myAttack.Rebounds.FirstOrDefault();

            if (reboundPlayer == null)
                return;
            if (reboundPlayer.Count > 1)
                reboundPlayer.Count--;
            else
                myAttack.Rebounds.Remove(reboundPlayer);


        }
    }
}