using KorfbalStatistics.Interface;
using KorfbalStatistics.Model;
using KorfbalStatistics.Services;
using KorfbalStatistics.Viewmodel;
using System.Collections.Generic;
using static KorfbalStatistics.Model.Enums;

namespace KorfbalStatistics.Command
{
    public class ShotclockoverrideCommand : AttackEndingCommand
    {
        public ShotclockoverrideCommand(GameStatisticViewModel gameStatisticViewModel) 
            : base (gameStatisticViewModel)
        {
            StatisticsNeeded = new List<EStatisticType>();
            StatisticType = EStatisticType.ShotclockOverride;
        }

        public override void Execute()
        {
            myCurrentAttack.ShotclockOverride(true);
            base.Execute();
        }

        public override void Redo()
        {
            base.Redo();
        }

        public override void Undo()
        {
            base.Undo();
        }
    }
}