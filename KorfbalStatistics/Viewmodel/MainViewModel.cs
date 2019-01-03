using System;
using System.IO;
using System.Linq;
using KorfbalStatistics.LocalDbModels;
using KorfbalStatistics.Model;
using KorfbalStatistics.Services;
using SQLite;

namespace KorfbalStatistics.Viewmodel
{
    public class MainViewModel
    {
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        private MainViewModel()
        {
            ServiceLocator.Register(new PlayersService(DbManager.Instance.PlayerDbManager));
            ServiceLocator.Register(new FormationService(DbManager.Instance.FormationDbManager));
            ServiceLocator.Register(new GameService(DbManager.Instance.GameDbManager));
            
            myDatabase = new Database();
            Data();
        }
        private Database myDatabase { get; set; }
        private static MainViewModel myInstance;
        public static MainViewModel Instance
        {
            get
            {
                if (myInstance == null)
                    myInstance = new MainViewModel();
                return myInstance;
            }
        }
        public void Data()
        {
            
            myDatabase.createDatabase(true);
            myDatabase.TestDb();
            LoggedInUser = myDatabase.selectTable().First();
        }
        public DbGame CurrentGame { get; set; }

        public DbUser LoggedInUser { get; set; }
    }
}