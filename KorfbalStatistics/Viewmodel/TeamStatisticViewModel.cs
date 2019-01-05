namespace KorfbalStatistics.Viewmodel
{
    public class TeamStatisticViewModel : BaseStatisticViewModel
    {
        public TeamStatisticViewModel()
        {
            CurrentZoneToShowFilter = Model.Enums.EZoneFunction.None;
        }
    }
}