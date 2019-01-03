using KorfbalStatistics.Command;
using System;
using System.Collections.Generic;
using static KorfbalStatistics.Model.Enums;

namespace KorfbalStatistics.Interface
{
    public interface IStatisticCommand : ICommand
    {
        List<EStatisticType> StatisticsNeeded { get; set; }
        void SetStatistic(EStatisticType stat, Guid value);
        Guid GetStatistic(EStatisticType stat);
        EStatisticType GetNextStatistic();
        EStatisticType GetPreviousStatistic();
    }
}