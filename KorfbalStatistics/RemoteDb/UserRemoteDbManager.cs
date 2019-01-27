using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KorfbalStatistics.Model;
using KorfbalStatistics.ProducerConsumer;
using MySql.Data.MySqlClient;
using SQLite;
using static KorfbalStatistics.CustomExtensions.Attributes;

namespace KorfbalStatistics.RemoteDb
{
    public class UserRemoteDbManager : BaseRemoteDbManager
    {
        public UserRemoteDbManager(Consumer consumer, MySqlConnection connection, SQLiteConnection localConn) : base(consumer, connection, localConn)
        {
        }

        public DbUser GetUserById(Guid userId)
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = RemoteConnection;
            command.CommandText = $"SELECT * FROM User WHERE Id = '{userId}'";
            command.Parameters.AddWithValue("@userId", userId.ToString());

            MySqlDataReader reader = command.ExecuteReader();

            return ReadData<DbUser>(reader);
        }

        public DbUser GetUserByUsername(string username)
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = RemoteConnection;
            command.CommandText = "SELECT * FROM User WHERE Username = @username";
            command.Parameters.AddWithValue("@username", username);

            MySqlDataReader reader = command.ExecuteReader();

            return ReadData<DbUser>(reader);
        }

        public DbCoach GetCoachById(Guid coachId)
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = RemoteConnection;
            command.CommandText = "SELECT * FROM Coach Join User on Coach.User_Id = User.Id WHERE User_Id = @coachId";
            command.Parameters.AddWithValue("@coachId", coachId.ToString());

            MySqlDataReader reader = command.ExecuteReader();

            return ReadData<DbCoach>(reader);
        }

        public DbTeam GetTeamById(Guid teamId)
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = RemoteConnection;
            command.CommandText = "SELECT * FROM Team WHERE Team.Id = @teamId";
            command.Parameters.AddWithValue("@teamId", teamId.ToString());

            MySqlDataReader reader = command.ExecuteReader();

            return ReadData<DbTeam>(reader);
        }

        public DbPlayer GetPlayerById(Guid playerId)
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = RemoteConnection;
            command.CommandText = $"SELECT * FROM Player Join User using (Id) WHERE Id = '{playerId.ToString()}'";
            //command.Parameters.AddWithValue("@playerId", playerId.ToString());

            MySqlDataReader reader = command.ExecuteReader();

            return ReadData<DbPlayer>(reader);
        }

        
    }
}