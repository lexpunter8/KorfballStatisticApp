﻿using System;
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
    public class FormationDbManager
    {
        private SQLiteConnection myDbConnection;

        public FormationDbManager(SQLiteConnection connection)
        {
            myDbConnection = connection;
        }
        public void AddFormation(List<DbFormation> dbFormations)
        {
            dbFormations.ForEach(f =>
            {
                myDbConnection.Insert(f);
            });
        }

        public DbFormation[] GetFormationByGameId(Guid gameId)
        {
            return myDbConnection.Table<DbFormation>().Where(f => f.GameId.Equals(gameId)).ToArray();
        }
    }
}