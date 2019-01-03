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
            myAttack.Shot(Guid.Empty, Guid.Empty);
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