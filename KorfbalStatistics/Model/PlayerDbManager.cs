using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
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
            DbPlayer teamIdPlayer = myDbConnection.Table<DbPlayer>().FirstOrDefault(p => p.TeamId.Equals(userId));
            DbCoach teamIdCoach = myDbConnection.Table<DbCoach>().FirstOrDefault(c => c.TeamId.Equals(userId));

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
    }
}