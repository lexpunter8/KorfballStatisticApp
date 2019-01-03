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
using KorfbalStatistics.Viewmodel;
using static KorfbalStatistics.Model.Enums;

namespace KorfbalStatistics.Command
{
    public class GoalConcededCommand : AttackEndingCommand
    {
        public GoalConcededCommand(GameStatisticViewModel gameStatisticViewModel) : base(gameStatisticViewModel)
        {
            StatisticsNeeded = new List<EStatisticType>
            {
                EStatisticType.ConcededGoal,
                EStatisticType.GoalType
            };
            StatisticType = EStatisticType.ConcededGoal;
        }

        public override void Execute()
        {
            myCurrentAttack.AddGoal(GetStatistic(EStatisticType.Goal), Guid.Empty, GetStatistic(EStatisticType.GoalType));
            base.Execute();
        }

        public override void Undo()
        {
            base.Undo();
        }
    }
}