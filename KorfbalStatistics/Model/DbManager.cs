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
using KorfbalStatistics.LocalDbModels;
using KorfbalStatistics.RemoteDb;
using MySql.Data.MySqlClient;
using SQLite;

namespace KorfbalStatistics.Model
{
    public class DbManager
    {
        private static DbManager myInstance;
        private GameDbManager myGameDbManager;
        private SQLiteConnection myConnection;
        private MySqlConnection myRemoteConnection;
        private string myRemoteConnectionString = "Server=lexpunter.hopto.org; database=StatisticKorfballApp; Uid=root; Pwd=2964Lpsql; Convert Zero Datetime=True";

        private string myConnectionString;
        private PlayerDbManager myPlayerDbManager;
        private FormationDbManager myFormationDbManager;
        private RemoteDbManager myRemoteDbManager;
        private UserDbManager myUserDbManager;

        private DbManager()
        {
            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            myConnectionString = Path.Combine(folder, "KorfbalStatistics.db");
            myConnection = new SQLiteConnection(myConnectionString);

            myRemoteConnection = new MySqlConnection(myRemoteConnectionString);
        }

        public static DbManager Instance { get
            {
                if (myInstance == null)
                    myInstance = new DbManager();
                return myInstance;
            }
        }

        public RemoteDbManager RemoteDbManager
        {
            get
            {
                if (myRemoteDbManager == null)
                    myRemoteDbManager = new RemoteDbManager(myRemoteConnection, new ProducerConsumer.Consumer(new Queue<Action>()), myConnection);
                return myRemoteDbManager;
            }
        }

        public GameDbManager GameDbManager { get
            {
                if (myGameDbManager == null)
                    myGameDbManager = new GameDbManager(myConnection, RemoteDbManager.GameRemoteDbManager);
                return myGameDbManager;
            }
        }

        public PlayerDbManager PlayerDbManager { get
            {
                if (myPlayerDbManager == null)
                    myPlayerDbManager = new PlayerDbManager(myConnection, RemoteDbManager.PlayerRemoteDbManager);
                return myPlayerDbManager;
            }
        }

        public FormationDbManager FormationDbManager
        {
            get
            {
                if (myFormationDbManager == null)
                    myFormationDbManager = new FormationDbManager(myConnection, RemoteDbManager.FormationDbManager);
                return myFormationDbManager;
            }
        }

        public UserDbManager UserDbManager
        {
            get
            {
                if (myUserDbManager == null)
                    myUserDbManager = new UserDbManager(myConnection, RemoteDbManager.UserRemoteDbManager);
                return myUserDbManager;
            }
        }
    }
}