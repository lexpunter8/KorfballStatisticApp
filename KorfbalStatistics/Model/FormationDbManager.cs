using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using KorfbalStatistics.CustomExtensions;
using KorfbalStatistics.Interface;
using KorfbalStatistics.RemoteDb;

namespace KorfbalStatistics.Model
{
    public class FormationDbManager : IFormationDbManager
    {

        public FormationDbManager(SQLiteConnection connection, FormationRemoteDbManager remoteDb)
        {
            myDbConnection = connection;
            myRemoteDb = remoteDb;
        }
        private SQLiteConnection myDbConnection;

        private FormationRemoteDbManager myRemoteDb { get; }

        public void AddFormation(List<DbFormation> dbFormations)
        {
            dbFormations.ForEach(f =>
            {
                myDbConnection.Insert(f);
            });

            myRemoteDb.AddFormation(dbFormations);
        }

        public DbFormation[] GetFormationByGameId(Guid gameId)
        {
            return myDbConnection.Table<DbFormation>().Where(f => f.GameId.Equals(gameId)).ToArray();
        }

        public void UpdateFormation(List<Player> formation, Guid gameId)
        {
            List<DbFormation> dbFormation = GetFormationByGameId(gameId).ToList();

            dbFormation.ForEach(dbf =>
            {
                Player player = formation.FirstOrDefault(p => p.Id == dbf.PlayerId);
                if (player == null)
                    return;
                dbf.CurrentFunction = player.CurrentZoneFunction.GetDescription();
                myDbConnection.Update(dbf);
            });
        }
    }
}