using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using static KorfbalStatistics.CustomExtensions.Attributes;

namespace KorfbalStatistics.RemoteDb
{
    public class FormationRemoteDbManager : BaseRemoteDbManager, IFormationDbManager
    {
        public FormationRemoteDbManager(Consumer consumer, MySqlConnection connection, SQLiteConnection localCon) : base(consumer, connection, localCon)
        { }

        public void AddFormation(List<DbFormation> dbFormations)
        {
            RemoteConsumer.AddAction(() => ExecuteAddFormation(dbFormations));
        }

        private void ExecuteAddFormation(List<DbFormation> dbFormations)
        {
            string insertQuery = CreateInsertStatement(dbFormations, "Formation");
            ExecuteSingleQuery(insertQuery);
        }
        public DbFormation[] GetFormationByGameId(Guid gameId)
        {
            MySqlCommand command = new MySqlCommand();
            command.CommandText = "SELECT * FROM Formation WHERE Game_Id = @gameId";
            command.Parameters.AddWithValue("@gameId", gameId);
            command.Connection = RemoteConnection;
            MySqlDataReader reader = command.ExecuteReader();
            return ReadDataList<DbFormation>(reader);
            
        }

        public void UpdateFormation(List<Player> formation, Guid gameId)
        {
            throw new NotImplementedException();
        }
    }
}