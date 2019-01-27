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
using KorfbalStatistics.Interface;
using KorfbalStatistics.Model;
using KorfbalStatistics.ProducerConsumer;
using MySql.Data.MySqlClient;
using SQLite;

namespace KorfbalStatistics.RemoteDb
{
    public class PlayerRemoteDbManager : BaseRemoteDbManager, IPlayerDbManager
    {
        public PlayerRemoteDbManager(Consumer consumer, MySqlConnection connection, SQLiteConnection localConn) : base(consumer, connection, localConn)
        {
        }

        public void AddPlayer(DbPlayer player)
        {
            DbUser user = player as DbUser;
            string query = CreateInsertStatement(user, "User");
            ExecuteSingleQuery(query);
            query = $"INSERT INTO Player (Id, Team_Id, Number, Abbrevation) VALUES ('{user.Id}', '{player.TeamId}', '{player.Number}', '{player.Abbrevation}')";
            ExecuteSingleQuery(query);
        }

        public DbPlayer GetPlayerById(Guid playerId)
        {
            var command = new MySqlCommand($"SELECT * FROM Player Join User using (Id) WHERE Id = '{playerId}'");
            return ExecuteSingleDataQuery<DbPlayer>(command);
        }

        public DbPlayer[] GetPlayersForTeamId(Guid teamId)
        {
            var command = new MySqlCommand($"SELECT * FROM Player Join User using (Id) WHERE Team_Id = '{teamId}'");
            return ExecuteMultiDataQuery<DbPlayer>(command);
        }

        public void RemovePlayer(DbPlayer player)
        {
            throw new NotImplementedException();
        }

        public void UpdatePlayer(DbPlayer player)
        {
            throw new NotImplementedException();
        }
    }
}