using System;
using System.Collections.Generic;
using System.IO;
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
    public class DbManager
    {
        private static DbManager myInstance;
        private GameDbManager myGameDbManager;
        private SQLiteConnection myConnection;

        private string myConnectionString;
        private PlayerDbManager myPlayerDbManager;
        private FormationDbManager myFormationDbManager;

        private DbManager()
        {
            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            myConnectionString = Path.Combine(folder, "KorfbalStatistics.db");
            myConnection = new SQLiteConnection(myConnectionString);
        }

        public static DbManager Instance { get
            {
                if (myInstance == null)
                    myInstance = new DbManager();
                return myInstance;
            }
        }
        public GameDbManager GameDbManager { get
            {
                if (myGameDbManager == null)
                    myGameDbManager = new GameDbManager(myConnection);
                return myGameDbManager;
            }
        }

        public PlayerDbManager PlayerDbManager { get
            {
                if (myPlayerDbManager == null)
                    myPlayerDbManager = new PlayerDbManager(myConnection);
                return myPlayerDbManager;
            }
        }

        public FormationDbManager FormationDbManager
        {
            get
            {
                if (myFormationDbManager == null)
                    myFormationDbManager = new FormationDbManager(myConnection);
                return myFormationDbManager;
            }
        }
    }
}