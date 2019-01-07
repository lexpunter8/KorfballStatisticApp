using KorfbalStatistics.Interface;
using KorfbalStatistics.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using KorfbalStatistics.CustomExtensions;
using static KorfbalStatistics.Model.Enums;

namespace KorfbalStatistics.Command
{
    public class BaseStatisticCommand : IStatisticCommand
    {

        public List<EStatisticType> StatisticsNeeded { get; set; }
        private Dictionary<EStatisticType, Guid> StatisticValues { get; set; } = new Dictionary<EStatisticType, Guid>();

        public Guid GetStatistic(EStatisticType stat)
        {
            foreach (KeyValuePair<EStatisticType, Guid> test in StatisticValues)
            {
                Guid guid = test.Value;
            }
            return StatisticValues.FirstOrDefault(s => s.Key.Equals(stat)).Value;
        }

        public void SetStatistic(EStatisticType stat, Guid value)
        {
            StatisticValues.Add(stat, value);
            StatisticsNeeded.Remove(stat);
        }
        public EStatisticType GetNextStatistic()
        {
            return StatisticsNeeded.FirstOrDefault();
        }

        public virtual void Execute() { }

        public virtual void Redo()
        {
        }

        public virtual void Undo()
        {
        }

        public EStatisticType GetPreviousStatistic()
        {
            KeyValuePair<EStatisticType, Guid> lastStat = StatisticValues.LastOrDefault();
            List<EStatisticType> newList = new List<EStatisticType>
            {
                lastStat.Key
            };
            newList.AddRange(StatisticsNeeded);
            StatisticsNeeded = new List<EStatisticType>(newList);
            StatisticValues.Remove(lastStat.Key);
            return lastStat.Key;
        }

        public EStatisticType StatisticType { get; set; }
        public bool IsCompleted { get; set; }
    }
}
