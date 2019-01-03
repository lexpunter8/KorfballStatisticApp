using System;
namespace KorfbalStatistics.Interface
{
    public interface IDbUser
    {
        int Id { get; set; }
        string Name { get; set; }
        char Sex { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
    public interface IDbPlayer
    {
        int Id { get; set; }
        int TeamId { get; set; }
        int Number { get; set; }
    }
    public interface IDbCoach
    {
        int Id { get; set; }
        int TeamId { get; set; }
    }
    public interface IDbTeam
    {
        int Id { get; set; }
        string Name { get; set; }
    }
    public interface IDbFormation
    {
        int Id { get; set; }
        int PlayerId { get; set; }
        int GameId { get; set; }
        char Function { get; set; }
    }
    public interface IDbGame
    {
        int Id { get; set; }
        int TeamId { get; set; }
        string Opponent { get; set; }
        DateTime Date { get; set; }
        bool IsHome { get; set; }
    }
    public interface IDbAttackShot
    {
        Guid AttackId { get; set; }
        Guid PlayerId { get; set; }
        int Count { get; set; }
    }
    public interface IDbAttack
    {
        int Id { get; set; }
        int GoalId { get; set; }
        int TurnoverPlayerId { get; set; }
        int GameId { get; set; }
        bool IsChotClockOverride { get; set; }
        bool IsAttackFunction { get; set; }
        bool IsDefence { get; set; }
    }
    public interface IDbAttackGoal
    {
        Guid Id { get; set; }
        Guid GoalTypeId { get; set; }
        Guid PlayerId { get; set; }
        Guid AssistPlayerId { get; set; }
    }
    public interface IDbGoalType
    {
        int Id { get; set; }
        string Name { get; set; }
    }
    public interface IDbAttackRebound
    {
        Guid AttackId { get; set; }
        Guid PlayerId { get; set; }
        int Count { get; set; }
    }
}