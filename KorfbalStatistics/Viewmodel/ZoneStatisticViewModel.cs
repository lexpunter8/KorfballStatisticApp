namespace KorfbalStatistics.Viewmodel
{
    public class ZoneStatisticViewModel : BaseStatisticViewModel
    {
        public ZoneStatisticViewModel()
        {
            CurrentZoneToShowFilter = Model.Enums.EZoneFunction.Attack;
        }
    }
}