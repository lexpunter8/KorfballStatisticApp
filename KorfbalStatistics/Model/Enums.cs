using System.ComponentModel;

namespace KorfbalStatistics.Model
{
    public class Enums
    {
        public enum EZoneFunction
        {
            None,
            [Description("Aanval")]
            Attack,
            [Description("Verdediging")]
            Defence
        }

        public enum EPlayerFunction
        {
            [Description("A")]
            Attack,
            [Description("D")]
            Defence,
            [Description("S")]
            Substitute
        }

        public enum EStatisticType
        {
            None,
            [Description("Wie verloor de bal?")]
            Turnover,
            [Description("Wie maakte de onderschepping?")]
            Interception,
            [Description("Wie maakt het doelpunt?")]
            Goal,
            [Description("Wie kreeg het doelpunt tegen?")]
            ConcededGoal,
            [Description("Tegen wie was het schot?")]
            ConcededShot,
            [Description("Wie nam het schot?")]
            Shot,
            [Description("")]
            ShotclockOverride,
            [Description("Wie gaf de assist?")]
            Assist,
            [Description("Wie had de rebound?")]
            Rebound,
            [Description("Wie had de rebound?")]
            DefensiveRebound,
            [Description("Hoe is het doelpunt gescoord?")]
            GoalType
        }

        public enum EGameStatus
        {
            [Description("None")]
            None,
            [Description("1H")]
            FirstHalf,
            [Description("2H")]
            SecondHalf,
            [Description("Ended")]
            Ended           
        }
    }
}