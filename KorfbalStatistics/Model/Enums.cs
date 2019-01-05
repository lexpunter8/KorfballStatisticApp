using System.ComponentModel;

namespace KorfbalStatistics.Model
{
    public class Enums
    {
        public enum EZoneFunction
        {
            None,
            [Description("Attack")]
            Attack,
            [Description("Defence")]
            Defence
        }

        public enum EPlayerFunction
        {
            Attack,
            Defence,
            Substitute
        }

        public enum EStatisticType
        {
            None,
            [Description("Who made the turnover?")]
            Turnover,
            [Description("Who made the interception?")]
            Interception,
            [Description("Who scored?")]
            Goal,
            [Description("Who conceded the goal?")]
            ConcededGoal,
            [Description("Against who was the shot?")]
            ConcededShot,
            [Description("who took the shot?")]
            Shot,
            [Description("")]
            ShotclockOverride,
            [Description("Who gave the assist?")]
            Assist,
            [Description("Who got the rebound?")]
            Rebound,
            [Description("Who got the rebound?")]
            DefensiveRebound,
            [Description("How was the goal made?")]
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