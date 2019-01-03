using System;
using KorfbalStatistics.Interface;
using SQLite;

namespace KorfbalStatistics.Model
{
    public class PlayerDbManager : IPlayerDbManager
    {
        private SQLiteConnection myDbConnection;

        public PlayerDbManager(SQLiteConnection connection)
        {
            myDbConnection = connection;
        }

        public DbPlayer GetPlayerById(Guid playerId)
        {
            if (playerId == Guid.Empty)
                return new DbPlayer { Id = Guid.Empty, Abbrevation = "-", Name = "-" };
            return myDbConnection.Table<DbPlayer>().FirstOrDefault(p => p.Id.Equals(playerId));
        }
        public DbPlayer[] GetPlayersForTeamId(Guid teamId)
        {
            var allPlayers = myDbConnection.Table<DbPlayer>().ToArray();
            return allPlayers;
        }

        public Guid GetTeamIdByUserId(Guid userId)
        {
            DbPlayer teamIdPlayer = myDbConnection.Table<DbPlayer>().FirstOrDefault(p => p.Id.Equals(userId));
            DbCoach teamIdCoach = myDbConnection.Table<DbCoach>().FirstOrDefault(c => c.Id.Equals(userId));

            if (teamIdCoach == null && teamIdPlayer == null)
                return Guid.Empty;

            return teamIdPlayer == null ? teamIdCoach.TeamId : teamIdPlayer.TeamId;
        }

        public void UpdatePlayer(DbPlayer player)
        {
            myDbConnection.Update(player);
        }
         
        public void AddPlayer(DbPlayer player)
        {
            myDbConnection.Insert(player);
        }
        public void RemovePlayer(DbPlayer player)
        {
            myDbConnection.Delete(player);
        }

        public DbTeam GetTeamByUSerId(Guid userId)
        {
            Guid teamId = GetTeamIdByUserId(userId);
            if (teamId == Guid.Empty)
                return null;
            var teams = myDbConnection.Table<DbTeam>();
            return teams.FirstOrDefault(t => t.Id == teamId);
           
        }
    }
}