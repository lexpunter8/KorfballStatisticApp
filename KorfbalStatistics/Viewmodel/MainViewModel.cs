using System;
using System.IO;
using System.Linq;
using KorfbalStatistics.LocalDbModels;
using KorfbalStatistics.Model;
using KorfbalStatistics.ProducerConsumer;
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
            ServiceLocator.Register(new GameService(DbManager.Instance.GameDbManager, DbManager.Instance.RemoteDbManager));

            myDatabase = new Database();
            Data();
            var dbm = DbManager.Instance.RemoteDbManager;
            dbm.Test();
            dbm.UpdateLocalDatabase(Guid.Parse("1c30b5af-68d5-40b8-a461-fa8d4c363744"));
            

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
          //  myDatabase.createDatabase(LoginHelper.IsFirstLogin);
            myDatabase.createDatabase(false);
            myDatabase.TestDb();
            LoggedInUser = DbManager.Instance.UserDbManager.GetUserByUsername("TestUser 1");
            Team = DbManager.Instance.UserDbManager.GetTeamByUser(LoggedInUser.Id)[0];
        }
        public DbGame CurrentGame { get; set; }
        public DbTeam Team { get; set; } = new DbTeam { Name = "NOT conn" };

        public DbUser LoggedInUser { get; set; } = new DbUser();
    }
}