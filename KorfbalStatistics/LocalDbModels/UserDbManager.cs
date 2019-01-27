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
using KorfbalStatistics.Model;
using KorfbalStatistics.RemoteDb;
using SQLite;

namespace KorfbalStatistics.LocalDbModels
{
    public class UserDbManager
    {
        public UserDbManager(SQLiteConnection connection, UserRemoteDbManager remoteDb)
        {
            myDbConnection = connection;
            myRemoteDb = remoteDb;
        }
        private SQLiteConnection myDbConnection;

        private UserRemoteDbManager myRemoteDb { get; }


        private void SaveToLocal<T>(T item)
        {
            myDbConnection.Insert(item);
        }
        public DbUser GetUserByUsername(string username)
        {
            var localUser = myDbConnection.Table<DbUser>().FirstOrDefault(user => user.Username.Equals(username));

            if (localUser != null)
                return localUser;

            DbUser remoteUser = myRemoteDb.GetUserByUsername(username);
            return remoteUser;
        }

        public DbUser GetUserById(Guid Id)
        {
            var localUser = myDbConnection.Table<DbUser>().FirstOrDefault(user => user.Id.Equals(Id));

            if (localUser != null)
                return localUser;

            DbUser remoteUser = myRemoteDb.GetUserById(Id);
            if (remoteUser != null)
                SaveToLocal(remoteUser);
            return remoteUser;
        }

        public DbTeam[] GetTeamByUser(Guid userId)
        {
            List<DbTeam> teams = new List<DbTeam>();
            var coach = GetCoachById(userId);
            if (coach != null)
            {
                var team = GetTeamById(coach.TeamId);
                if (team == null)
                    team = myRemoteDb.GetTeamById(coach.TeamId);
                teams.Add(team);
            }

            var player = GetPlayerById(userId);
            if (player != null)
            {
                var team = GetTeamById(player.TeamId);
                if (team == null)
                    team = myRemoteDb.GetTeamById(player.TeamId);
                teams.Add(team);
            }

            return teams.ToArray();
            
        }
        public DbTeam GetTeamById(Guid teamId)
        {
            return myDbConnection.Table<DbTeam>().FirstOrDefault(t => t.Id.Equals(teamId));
        }
        public DbPlayer GetPlayerById(Guid playerId)
        {
            var localPlayer = myDbConnection.Table<DbPlayer>().FirstOrDefault(player => player.Id.Equals(playerId));
            if (localPlayer != null)
                return localPlayer;

            DbPlayer remotePlayer = myRemoteDb.GetPlayerById(playerId);
            if (remotePlayer != null)
                SaveToLocal(remotePlayer);
            return remotePlayer;
        }
        public DbCoach GetCoachById(Guid coachId)
        {
            var localCoach = myDbConnection.Table<DbCoach>().FirstOrDefault(player => player.Id.Equals(coachId));
            if (localCoach != null)
                return localCoach;

            DbCoach remoteCoach = myRemoteDb.GetCoachById(coachId);
            return remoteCoach;
        }
    }
}