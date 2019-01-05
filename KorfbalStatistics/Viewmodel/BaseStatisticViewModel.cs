using System;
using System.Collections.Generic;
using System.Linq;
using KorfbalStatistics.Interface;
using KorfbalStatistics.Model;
using KorfbalStatistics.Services;
using OxyPlot;
using OxyPlot.Series;
using static KorfbalStatistics.Model.Enums;

namespace KorfbalStatistics.Viewmodel
{
    public class BaseStatisticViewModel : IStatisticViewModel
    {
        private List<Attack> myAttacks { get; set; } = new List<Attack>();
        public int ShotCount => GetShotCount(false);
        public EZoneFunction CurrentZoneToShowFilter { get; set; }
        private int GetShotCount(bool isOpponentAttack)
        {
            int shotCount = 0;
            myAttacks.ForEach(a =>
            {
                if (a.DbAttack.IsOpponentAttack != isOpponentAttack)
                    return;
                shotCount += a.ShotCount();
            });
            return shotCount;
        }
        public int ReboundCount => GetReboundCount(false);

        private int GetReboundCount(bool isOpponentAttack)
        {
            int reboundCount = 0;
            myAttacks.ForEach(a =>
            {
                if (a.DbAttack.IsOpponentAttack != isOpponentAttack)
                    return;
                reboundCount += a.ReboundCount();
            });
            return reboundCount;
        }

        public int GoalCount => myAttacks.Where(a => a.Goal != null && !a.DbAttack.IsOpponentAttack).Count();
        public int InterceptionCount => myAttacks.Where(a => !a.DbAttack.IsOpponentAttack
                                                            && a.DbAttack.TurnoverPlayerId != null).Count();
        public int TurnoverCount => myAttacks.Where(a => a.DbAttack.IsOpponentAttack
                                                            && a.DbAttack.TurnoverPlayerId != null).Count();
        public int ShotClokcOverrideCount => myAttacks.Where(a => !a.DbAttack.IsOpponentAttack
                                                            && a.DbAttack.IsSchotClockOverride).Count();
        public int AttackCount => myAttacks.Where(a => !a.DbAttack.IsOpponentAttack).Count();
        public int AssistCount { get; set; }
        public PlotModel PlotModel => CreatePlot();

        public int ConcededShotCount => GetShotCount(true);

        public int DevensiveReboundCount => GetReboundCount(true);

        public int ConcededGoalCount => myAttacks.Where(a => a.Goal != null && a.DbAttack.IsOpponentAttack).Count();

        private PlotModel CreatePlot()
        {
            Dictionary<Guid, int> goals = new Dictionary<Guid, int>();
            myAttacks.ForEach(a =>
            {
                if (a.Goal == null)
                    return;
                if (goals.ContainsKey(a.Goal.GoalTypeId))
                    goals[a.Goal.GoalTypeId]++;
                else
                    goals.Add(a.Goal.GoalTypeId, 1);
            });

            var plotModel = new PlotModel();
            var pieSeries = new PieSeries
            {
                InsideLabelPosition = 0.8,
                StrokeThickness = 0,
                InsideLabelFormat = "{1}: {0}",
                OutsideLabelFormat = null

            };
            foreach (var goalTypeId in goals)
            {
                DbGoalType goalType = ServiceLocator.GetService<GameService>().GetGoalTypeById(goalTypeId.Key);
                pieSeries.Slices.Add(new PieSlice(goalType.Name, goalTypeId.Value));
            }

            plotModel.Series.Add(pieSeries);
            return plotModel;
        }

        public void SetAttacks()
        {
            myAttacks = ServiceLocator.GetService<GameService>().GetAttacksByGameId(MainViewModel.Instance.CurrentGame.Id, CurrentZoneToShowFilter);
        }
    }
}