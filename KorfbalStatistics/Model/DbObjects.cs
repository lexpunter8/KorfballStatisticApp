using KorfbalStatistics.Interface;
using System;
using SQLite;
using System.Reflection;
using static KorfbalStatistics.CustomExtensions.Attributes;

namespace KorfbalStatistics.Model
{
    [Table("User")]
    public class DbUser
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }
        [RemoteDbName("First_Name")]
        public string FirstName { get; set; }
        [MaxLength(1)]
        public string Sex { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsSynchronised { get; set; }

    }
    [Table("Player")]
    public class DbPlayer : DbUser
    {
        [RemoteDbName("Team_Id")]
        public Guid TeamId { get; set; }
        public int Number { get; set; }
        public string Abbrevation { get; set; }
    }
    [Table("Coach")]
    public class DbCoach : DbUser
    {
        [RemoteDbName("Team_Id")]
        public Guid TeamId { get; set; }
    }
    [Table("Team")]
    public class DbTeam
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsSynchronised { get; set; }
    }
    [Table("Formation")]
    public class DbFormation
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        [RemoteDbName("Player_ID")]
        public Guid PlayerId { get; set; }
        [RemoteDbName("Game_Id")]
        public Guid GameId { get; set; }
        [MaxLength(1)]
        public string CurrentFunction { get; set; }
        public string StartFunction { get; set; }
        public bool IsSynchronised { get; set; }
    }
    [Table("Game")]
    public class DbGame
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        [RemoteDbName("Team_Id")]
        public Guid TeamId { get; set; }
        public string Opponent { get; set; }
        public DateTime Date { get; set; }
        public bool IsHome { get; set; }
        public string Status { get; set; } //DNS H1, H2, END
        public bool IsSynchronised { get; set; }
    }
    [Table("AttackShot")]
    public class DbAttackShot
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        [RemoteDbName("Attack_Id")]
        public Guid AttackId { get; set; }
        [RemoteDbName("Player_Id")]
        public Guid PlayerId { get; set; }
        public int Count { get; set; }
        public bool IsSynchronised { get; set; }
    }
    [Table("Attack")]
    public class DbAttack
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        [RemoteDbName("Goal_Id")]
        public Guid? GoalId { get; set; }
        [RemoteDbName("Turnover_Player_Id")]
        public Guid? TurnoverPlayerId { get; set; }
        [RemoteDbName("Game_Id")]
        public Guid GameId { get; set; }
        public bool IsShotClockOverride { get; set; }
        public bool IsOpponentAttack { get; set; }
        public string ZoneStartFunction { get; set; } // A/D
        public bool IsFirstHalf { get; set; }
        public bool IsSynchronised { get; set; }
    }
    [Table("AttackGoal")]
    public class DbAttackGoal
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        [RemoteDbName("GoalType_Id")]
        public Guid GoalTypeId { get; set; }
        [RemoteDbName("Player_Id")]
        public Guid PlayerId { get; set; }
        [RemoteDbName("Assist_Player_Id")]
        public Guid AssistPlayerId { get; set; }
        public bool IsSynchronised { get; set; }
    }
    [Table("GoalType")]
    public class DbGoalType
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsSynchronised { get; set; }
    }
    [Table("AttackRebound")]
    public class DbAttackRebound : IDbAttackRebound
    {
        [PrimaryKey, AutoIncrement]
        public Guid Id { get; set; }
        [RemoteDbName("Attack_Id")]
        public Guid AttackId { get; set; }
        [RemoteDbName("Player_Id")]
        public Guid PlayerId { get; set; }
        public int Count { get; set; }
        public bool IsSynchronised { get; set; }
    }
}