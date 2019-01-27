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
    public class GameRemoteDbManager : BaseRemoteDbManager
    {
        public GameRemoteDbManager(Consumer consumer, MySqlConnection connection, SQLiteConnection localConnection) 
            : base(consumer, connection, localConnection)
        {
        }
        public void AddGame(DbGame game)
        {
            RemoteConsumer.AddAction(() => ExecuteAddGame(game));
        }

        public DbGame[] GetGamesForTeam(Guid teamId)
        {
            MySqlCommand command = new MySqlCommand($"SELECT * FROM Game WHERE Team_Id = '{teamId}'");
            return ExecuteMultiDataQuery<DbGame>(command);
        }

        private void ExecuteAddGame(DbGame game)
        {
                string query = CreateInsertStatement(game, "Game");
                ExecuteSingleQuery(query);
        }

        public DbAttack GetAttackById(Guid id)
        {
            var command = new MySqlCommand($"SELECT * FROM Attack WHERE Id = {id}");
            var data = ExecuteSingleDataQuery<DbAttack>(command);
            LocalConnection.InsertOrReplace(data);
            return null;
        }

        public DbAttack[] GetAttacksForGame(Guid id)
        {
            var command = new MySqlCommand($"SELECT * FROM Attack WHERE Game_Id = '{id}'");
            return ExecuteMultiDataQuery<DbAttack>(command);
        }
    }
}