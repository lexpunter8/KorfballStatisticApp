using KorfbalStatistics.Interface;
using KorfbalStatistics.Model;
using KorfbalStatistics.Viewmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using static KorfbalStatistics.Model.Enums;

namespace KorfbalStatistics.Command
{
    public class ShotCommand : AttackEndingCommand
    {
        public ShotCommand(GameStatisticViewModel gameStatisticViewModel) : base(gameStatisticViewModel)
        {
            myAttack = gameStatisticViewModel.CurrentAttack;

            StatisticsNeeded = new List<EStatisticType>
            {
                EStatisticType.Shot,
                EStatisticType.Rebound
            };
            StatisticType = EStatisticType.Shot;
        }
        private IAttack myAttack;

        public override void Execute()
        {
            Guid reboundGuid = GetStatistic(EStatisticType.Rebound);
            myAttack.Shot(GetStatistic(EStatisticType.Shot), reboundGuid);
            if (reboundGuid == Guid.Empty)
                base.Execute();
            IsCompleted = true;
        }

        public override void Redo()
        {
        }

        public override void Undo()
        {
            DbAttackShot shotPlayer = myAttack.Shots.FirstOrDefault(p => p.PlayerId == GetStatistic(EStatisticType.Shot));
            if (shotPlayer.Count > 1)
                shotPlayer.Count--;
            else
                myAttack.Shots.Remove(shotPlayer);

            DbAttackRebound reboundPlayer = myAttack.Rebounds.FirstOrDefault(p => p.PlayerId == GetStatistic(EStatisticType.Rebound));
            if (reboundPlayer.Count > 1)
                reboundPlayer.Count--;
            else
                myAttack.Rebounds.Remove(reboundPlayer);

            
        }
    }
}