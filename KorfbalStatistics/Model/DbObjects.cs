using KorfbalStatistics.Interface;
using System;
using SQLite;

namespace KorfbalStatistics.Model
{
    [Table("User")]
    public class DbUser
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }
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
        public Guid TeamId { get; set; }
        public int Number { get; set; }
        public string Abbrevation { get; set; }
    }
    [Table("Coach")]
    public class DbCoach : DbUser
    {
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
        public Guid PlayerId { get; set; }
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
        [PrimaryKey, AutoIncrement]
        public Guid Id { get; set; }
        public Guid AttackId { get; set; }
        public Guid PlayerId { get; set; }
        public int Count { get; set; }
        public bool IsSynchronised { get; set; }
    }
    [Table("Attack")]
    public class DbAttack
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public Guid? GoalId { get; set; }
        public Guid? TurnoverPlayerId { get; set; }
        public Guid GameId { get; set; }
        public bool IsSchotClockOverride { get; set; }
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
        public Guid GoalTypeId { get; set; }
        public Guid PlayerId { get; set; }
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
        public Guid AttackId { get; set; }
        public Guid PlayerId { get; set; }
        public int Count { get; set; }
        public bool IsSynchronised { get; set; }
    }
}