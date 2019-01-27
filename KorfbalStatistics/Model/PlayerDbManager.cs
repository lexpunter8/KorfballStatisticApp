using System;
using System.Collections.Generic;
using System.Linq;
using KorfbalStatistics.Interface;
using KorfbalStatistics.RemoteDb;
using SQLite;

namespace KorfbalStatistics.Model
{
    public class PlayerDbManager : IPlayerDbManager
    {
        private SQLiteConnection myDbConnection;
        private PlayerRemoteDbManager myRemoteDb;

        public PlayerDbManager(SQLiteConnection connection, PlayerRemoteDbManager playerRemoteManager)
        {
            myDbConnection = connection;
            myRemoteDb = playerRemoteManager;
        }

        public DbPlayer GetPlayerById(Guid playerId)
        {
            if (playerId == Guid.Empty)
                return new DbPlayer { Id = Guid.Empty, Abbrevation = "-", Name = "-" };
            var localPlayer = myDbConnection.Table<DbPlayer>().FirstOrDefault(p => p.Id.Equals(playerId));
            if (localPlayer != null)
                return localPlayer;

            var remotePlayer = myRemoteDb.GetPlayerById(playerId);
            // TODO
            return remotePlayer;
        }
        public DbPlayer[] GetPlayersForTeamId(Guid teamId)
        {
            var localPlayers = myDbConnection.Table<DbPlayer>().Where(p => p.TeamId == teamId && !p.IsSynchronised).ToList();
            var remotePlayers = myRemoteDb.GetPlayersForTeamId(teamId).ToList();
            var allPlayers = new List<DbPlayer>();

            myDbConnection.DeleteAll<DbPlayer>();

            allPlayers.AddRange(remotePlayers);
            allPlayers.AddRange(localPlayers);

            foreach (var player in allPlayers)
            {
                myDbConnection.InsertOrReplace(player);
            }

            return allPlayers.ToArray();
        }

        public void UpdatePlayer(DbPlayer player)
        {
            myDbConnection.Update(player);
            myRemoteDb.UpdatePlayer(player);
        }
         
        public void AddPlayer(DbPlayer player)
        {
            myDbConnection.Insert(player);
            myRemoteDb.AddPlayer(player);
        }
        public void RemovePlayer(DbPlayer player)
        {
            myDbConnection.Delete(player);
        }

        internal Player GetUnkownPlayer()
        {
            return myDbConnection.Table<Player>().FirstOrDefault(p => p.Id.Equals(Guid.Parse("6467eac9-0164-4041-adc6-4b7b038c1a7d")));
        }

        public List<DbAttackRebound> GetReboundsForPlayerByGame(Guid playerId, Guid gameId)
        {
            List<Guid> attackGuids = new List<Guid>();
            myDbConnection.Table<DbAttack>().ToList().ForEach(a => {
                if (a.GameId.Equals(gameId))
                    attackGuids.Add(a.Id);
                });
            List<DbAttackRebound> allRebounds = myDbConnection.Table<DbAttackRebound>().Where(ar => ar.PlayerId.Equals(playerId)).ToList();

            var rebounds = new List<DbAttackRebound>();
            rebounds.AddRange(allRebounds.Where(r => attackGuids.Contains(r.AttackId)));
            return rebounds;
        }

        public List<DbAttackShot> GetShotsForPlayerByGame(Guid playerId, Guid gameId)
        {
            List<Guid> attackGuids = new List<Guid>();
            myDbConnection.Table<DbAttack>().ToList().ForEach(a => {
                if (a.GameId.Equals(gameId))
                    attackGuids.Add(a.Id);
            });
            List<DbAttackShot> allRebounds = myDbConnection.Table<DbAttackShot>().Where(ar => ar.PlayerId.Equals(playerId)).ToList();

            var shots = new List<DbAttackShot>();
            shots.AddRange(allRebounds.Where(r => attackGuids.Contains(r.AttackId)));
            return shots;
        }
        public List<DbAttackGoal> GetGoalsForPlayerByGame(Guid playerId, Guid gameId)
        {
            List<DbAttack> attacks = myDbConnection.Table<DbAttack>().Where(a => a.GameId == gameId).ToList();
            List<DbAttackGoal> goals = new List<DbAttackGoal>();
            attacks.ForEach(a =>
            {
                if (a.GoalId == null)
                    return;
                var goal = myDbConnection.Table<DbAttackGoal>().FirstOrDefault(ag => ag.Id == a.GoalId && ag.PlayerId == playerId);
                if (goal == null)
                    return;
                goals.Add(goal);

            });
            return goals;
        }

        public List<DbAttackGoal> GetAssitsForPlayerByGame(Guid playerId, Guid gameId)
        {
            List<DbAttack> attacks = myDbConnection.Table<DbAttack>().Where(a => a.GameId == gameId).ToList();
            List<DbAttackGoal> assists = new List<DbAttackGoal>();
            attacks.ForEach(a =>
            {
                if (a.GoalId == null)
                    return;
                var assist = myDbConnection.Table<DbAttackGoal>().FirstOrDefault(ag => ag.Id == a.GoalId && ag.AssistPlayerId == playerId);
                if (assist == null)
                    return;
                assists.Add(assist);

            });
            return assists;
        }

        public List<DbAttack> GetTurnoversForPlayerByGame(Guid playerId, Guid gameId)
        {
            List<DbAttack> attacks = myDbConnection.Table<DbAttack>().Where(a => a.GameId == gameId).ToList();
            return attacks.Where(a => a.TurnoverPlayerId == playerId).ToList();
        }
    }
}