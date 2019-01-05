using System;
using System.Collections.Generic;
using KorfbalStatistics.Viewmodel;
using static KorfbalStatistics.Model.Enums;

namespace KorfbalStatistics.Command
{
    public class InterceptionCommand : AttackEndingCommand
    {
        public InterceptionCommand(GameStatisticViewModel gameStatisticViewModel) : base(gameStatisticViewModel)
        {
            StatisticsNeeded = new List<EStatisticType>
            {
                EStatisticType.Interception
            };
            StatisticType = EStatisticType.Interception;
        }

        public override void Execute()
        {
            if (StatisticsNeeded.Count != 0)
                return;
            myCurrentAttack.Turnover(GetStatistic(EStatisticType.Interception));
            base.Execute();
        }
        public override void Undo()
        {
            base.Undo();
        }
    }
}
