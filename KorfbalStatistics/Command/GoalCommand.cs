using KorfbalStatistics.Viewmodel;
using System;
using System.Collections.Generic;
using static KorfbalStatistics.Model.Enums;

namespace KorfbalStatistics.Command
{
    public class GoalCommand : AttackEndingCommand
    {
        public GoalCommand(GameStatisticViewModel gameStatisticViewModel) : base(gameStatisticViewModel)
        {
            StatisticsNeeded = new List<EStatisticType>
            {
                EStatisticType.Goal,
                EStatisticType.Assist,
                EStatisticType.GoalType
            };
            StatisticType = EStatisticType.Goal;
        }

        public override void Execute()
        {
            myCurrentAttack.AddGoal(GetStatistic(EStatisticType.Goal), GetStatistic(EStatisticType.Assist), GetStatistic(EStatisticType.GoalType));
            base.Execute();
        }

        public override void Undo()
        {
            base.Undo();
        }
    }
}