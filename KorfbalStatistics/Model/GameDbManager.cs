using System;
using System.Collections.Generic;
using System.Linq;
using KorfbalStatistics.Interface;
using KorfbalStatistics.RemoteDb;
using SQLite;
using static KorfbalStatistics.Model.Enums;

namespace KorfbalStatistics.Model
{
    public class GameDbManager : IGameDbManager
    {
        public GameDbManager(SQLiteConnection connection, GameRemoteDbManager gameRemoteDbManager)
        {
            myDbConnection = connection;
            myRemoteDb = gameRemoteDbManager;
        } 
        public void AddGame(DbGame game)
        {
            myDbConnection.Insert(game);
            myRemoteDb.AddGame(game);
        }
        public DbGame[] GetGamesForTeam(Guid teamId)
        {
            var localGames = new List<DbGame>(myDbConnection.Table<DbGame>().Where(dbg => dbg.TeamId.Equals(teamId) && !dbg.IsSynchronised));
            var remoteGames = new List<DbGame>(myRemoteDb.GetGamesForTeam(teamId));
            var games = new List<DbGame>();

            myDbConnection.DeleteAll<DbGame>();

            games.AddRange(remoteGames);
            games.AddRange(localGames);

            foreach (var remoteGame in remoteGames)
            {
                myDbConnection.InsertOrReplace(remoteGame);
            }

            return games.ToArray();
        }
        private SQLiteConnection myDbConnection { get; set; }

        private GameRemoteDbManager myRemoteDb;

        public Guid GetTeamIdByUserId(Guid userId)
        {
            return myDbConnection.Table<DbCoach>().FirstOrDefault(user => user.Id == userId).TeamId;
        }

        public void AddAttack(DbAttack attack)
        {
            myDbConnection.Insert(attack);
        }
        public void RemoveAttack(DbAttack attack)
        {
            myDbConnection.Delete(attack);
        }

        public DbAttack GetAttackById(Guid id)
        {
            DbAttack attack = myDbConnection.Table<DbAttack>().FirstOrDefault(a => a.Id == id);
            return attack;
        }

        public void AddRebound(DbAttackRebound rebound)
        {
            myDbConnection.Insert(rebound);
        }

        public void RemoveRebound(DbAttackRebound rebound)
        {
            myDbConnection.Delete(rebound);
        }

        public void AddShot(DbAttackShot shot)
        {
            myDbConnection.Insert(shot);
        }

        public void RemoveShot(DbAttackShot shot)
        {
            myDbConnection.Delete(shot);
        }

        public void AddGoal(DbAttackGoal goal)
        {
            myDbConnection.Insert(goal);
        }
        public void RemoveGoal(DbAttackGoal goal)
        {
            myDbConnection.Delete(goal);
        }

        public DbAttack[] GetAttacksForGame(Guid id)
        {
            var localAttacks = myDbConnection.Table<DbAttack>().Where(a => a.GameId == id).ToArray();
            return localAttacks;
            return myDbConnection.Query<DbAttack>("" +
                "SELECT * FROM Attack WHERE Id = ?", id).ToArray();
        }

        public Attack[] GetFullAttackForGame(Guid gameId)
        {
            List<Attack> attacks = new List<Attack>();
            DbAttack[] dbAttacks = GetAttacksForGame(gameId);
            foreach(DbAttack a in dbAttacks)
            {
                EZoneFunction function = a.IsOpponentAttack ? EZoneFunction.Defence : EZoneFunction.Attack;
                

                Attack attack = new Attack(function, a.ZoneStartFunction, gameId, a.IsFirstHalf);

                // get shots
                attack.Shots = myDbConnection.Table<DbAttackShot>().Where(s => s.AttackId == a.Id).ToList();
                // get rebounds
                attack.Rebounds = myDbConnection.Table<DbAttackRebound>().Where(r => r.AttackId == a.Id).ToList();
                // 
                if (a.GoalId != Guid.Empty)
                    attack.Goal = myDbConnection.Table<DbAttackGoal>().FirstOrDefault(g => g.Id == a.GoalId);
                attack.DbAttack = a;
                attacks.Add(attack);
            }
            return attacks.ToArray();
        }

        public DbGoalType GetGoalTypeById(Guid goalTypeId)
        {
            return myDbConnection.Table<DbGoalType>().FirstOrDefault(g => g.Id.Equals(goalTypeId));
        }

        public DbAttack GetLastAttackOfGame(Guid gameId)
        {
            return myDbConnection.Table<DbAttack>().LastOrDefault(a => a.GameId == gameId);
        }
    }
}