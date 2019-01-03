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
using KorfbalStatistics.Interface;
using KorfbalStatistics.Viewmodel;
using static KorfbalStatistics.Model.Enums;

namespace KorfbalStatistics.Command
{
    public class TurnoverCommand : AttackEndingCommand
    {
        public TurnoverCommand(GameStatisticViewModel gameStatisticViewModel) : base(gameStatisticViewModel)
        {
            StatisticsNeeded = new List<EStatisticType>
            {
                EStatisticType.Turnover
            };
            StatisticType = EStatisticType.Turnover;
        }

        public override void Execute()
        {
            if (StatisticsNeeded.Count != 0)
                return;
            myCurrentAttack.Turnover(GetStatistic(EStatisticType.Turnover));
            base.Execute();
        }
        public override void Undo()
        {
            base.Undo();
        }
    }
}