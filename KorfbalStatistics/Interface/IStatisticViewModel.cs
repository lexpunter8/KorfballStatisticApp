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
using OxyPlot;
using static KorfbalStatistics.Model.Enums;

namespace KorfbalStatistics.Interface
{
    public interface IStatisticViewModel
    {
        int ShotCount { get; }
        int ConcededShotCount { get; }
        int ReboundCount { get; }
        int DevensiveReboundCount { get; }
        int GoalCount { get; }
        int ConcededGoalCount { get; }
        int InterceptionCount { get;  }
        int TurnoverCount { get; }
        int ShotClokcOverrideCount { get;  }
        int AttackCount { get; }
        int AssistCount { get; }
        PlotModel PlotModel { get; }
        void SetAttacks();
        EZoneFunction CurrentZoneToShowFilter { get; set; }
    }
}