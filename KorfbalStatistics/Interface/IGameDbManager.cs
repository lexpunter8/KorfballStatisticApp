﻿using System;
using System.Collections.Generic;
using KorfbalStatistics.Model;

namespace KorfbalStatistics.Interface
{
    public interface IGameDbManager
    {
        DbGame[] GetGamesForTeam(Guid teamId);
        Guid GetTeamIdByUserId(Guid userId);
        void AddGame(DbGame game);
        void AddAttack(DbAttack atttack);
        DbAttack GetAttackById(Guid id);
        void AddRebound(DbAttackRebound rebound);
        void RemoveRebound(DbAttackRebound rebound);
        void AddShot(DbAttackShot shot);
        void RemoveShot(DbAttackShot shot);
        void AddGoal(DbAttackGoal goal);
        void RemoveGoal(DbAttackGoal goal);
        DbAttack[] GetAttacksForGame(Guid id);

        Attack[] GetFullAttackForGame(Guid gameId);
        DbGoalType GetGoalTypeById(Guid goalTypeId);
        DbAttack GetLastAttackOfGame(Guid gameId);
    }
}