using KorfbalStatistics.Interface;
using KorfbalStatistics.Model;
using KorfbalStatistics.ProducerConsumer;
using MySql.Data.MySqlClient;
using SQLite;
using System;
using System.Collections.Generic;
using System.Data;

namespace KorfbalStatistics.RemoteDb
{
    public class RemoteDbManager : BaseRemoteDbManager, IRemoteDbManager
    {
        private UserRemoteDbManager myUserDbManager;
        private GameRemoteDbManager myGameRemoteDbManager;
        private PlayerRemoteDbManager myPlayerRemoteDbManager;
        private FormationRemoteDbManager myFormationDbManager;

        public RemoteDbManager(MySqlConnection connection, Consumer consumer, SQLiteConnection localConn) : base(consumer, connection, localConn)
        {
            
        }
        public FormationRemoteDbManager FormationDbManager
        {
            get
            {
                if (myFormationDbManager == null)
                    myFormationDbManager = new FormationRemoteDbManager(RemoteConsumer, RemoteConnection, LocalConnection);
                return myFormationDbManager;
            }
        }

        public UserRemoteDbManager UserRemoteDbManager
        {
            get
            {
                if (myUserDbManager == null)
                    myUserDbManager = new UserRemoteDbManager(RemoteConsumer, RemoteConnection, LocalConnection);
                return myUserDbManager;
            }
        }

        public GameRemoteDbManager GameRemoteDbManager
        {
            get
            {
                if (myGameRemoteDbManager == null)
                    myGameRemoteDbManager = new GameRemoteDbManager(RemoteConsumer, RemoteConnection, LocalConnection);
                return myGameRemoteDbManager;
            }
        }

        public PlayerRemoteDbManager PlayerRemoteDbManager
        {
            get
            {
                if (myPlayerRemoteDbManager == null)
                    myPlayerRemoteDbManager = new PlayerRemoteDbManager(RemoteConsumer, RemoteConnection, LocalConnection);
                return myPlayerRemoteDbManager;
            }
        }

        public void Test()
        {
            Guid gameId = Guid.Parse("59c2bb37-eaad-4f5c-8760-ed00aa349a85");
            var localAttacks = LocalConnection;
            string query = $"SELECT * FROM Attack WHERE GameId = ?";
            SQLiteCommand command = LocalConnection.CreateCommand(query, gameId);
            var test = command.CommandText;
            List<DbAttack> result = command.ExecuteQuery<DbAttack>();
        }

        public void InsertGameData(DbGame game)
        {
            RemoteConsumer.AddAction(() => ExecuteInsertGameData(game));
        }

        private void ExecuteInsertGameData(DbGame game)
        {
            game.IsSynchronised = true;
            LocalConnection.Update(game);

            Guid gameId = game.Id;

            List<DbAttackGoal> goals = new List<DbAttackGoal>();
            List<DbAttack> attacks = new List<DbAttack>();
            List<DbAttackRebound> rebounds = new List<DbAttackRebound>();
            List<DbAttackShot> shots = new List<DbAttackShot>();

            MySqlCommand remoteCommand = new MySqlCommand();
            MySqlTransaction transaction = RemoteConnection.BeginTransaction();
            remoteCommand.Transaction = transaction;
            try {
                var localAttacks = LocalConnection;
                string query = "SELECT * FROM Attack WHERE GameId = ?";
                SQLiteCommand command = LocalConnection.CreateCommand(query, gameId);
                attacks = command.ExecuteQuery<DbAttack>();

                string getGoalsQuery = "SELECT ag.* FROM Attack a" +
                                        " JOIN AttackGoal ag" +
                                        " on a.GoalId = ag.Id" +
                                        " WHERE a.GameId = ? AND a.IsSynchronised = 0";
                command = LocalConnection.CreateCommand(getGoalsQuery, gameId);
                goals = command.ExecuteQuery<DbAttackGoal>();

                string insertGoalsQuery = CreateInsertStatement(goals, "Attack_Goal");

                string insertAttacksQuery = CreateInsertStatement(attacks, "Attack");


                string localShotsQuery = "SELECT ash.* FROM Attack a" +
                                        " join AttackShot ash" +
                                        " on a.Id = ash.AttackId" +
                                        " WHERE a.GameId = ? AND a.IsSynchronised = 0";

                command = LocalConnection.CreateCommand(localShotsQuery, gameId);
                shots = command.ExecuteQuery<DbAttackShot>();
                string insertShotsQuery = CreateInsertStatement(shots, "Attack_Shot");

                string localReboundsQuery = "SELECT ash.* FROM Attack a" +
                                        " join AttackShot ash " +
                                        " on a.Id = ash.AttackId" +
                                        " WHERE a.GameId = ? AND a.IsSynchronised = 0";

                command = LocalConnection.CreateCommand(localReboundsQuery, gameId);
                rebounds = command.ExecuteQuery<DbAttackRebound>();
                string insertReboundsQuery = CreateInsertStatement(rebounds, "Attack_Rebound");

                remoteCommand.Connection = RemoteConnection;

                remoteCommand.CommandText = insertGoalsQuery;
                remoteCommand.ExecuteNonQuery();

                remoteCommand.CommandText = insertAttacksQuery;
                remoteCommand.ExecuteNonQuery();

                remoteCommand.CommandText = insertReboundsQuery;
                remoteCommand.ExecuteNonQuery();

                remoteCommand.CommandText = insertShotsQuery;
                remoteCommand.ExecuteNonQuery();

                transaction.Commit();

                goals.ForEach(g => g.IsSynchronised = true);
                attacks.ForEach(a => a.IsSynchronised = true);
                rebounds.ForEach(r => r.IsSynchronised = true);
                shots.ForEach(s => s.IsSynchronised = true);

                LocalConnection.UpdateAll(goals);
                LocalConnection.UpdateAll(attacks);
                LocalConnection.UpdateAll(rebounds);
                LocalConnection.UpdateAll(shots);


            } catch (Exception e)
            {
                transaction.Rollback();
                throw new Exception(e.Message);
            }

            

        }

        public void InsertAttack(IAttack attack)
        {
            bool isGoal = attack.Goal != null;
            string attackGoalInsert = string.Empty;
            if (isGoal) 
                attackGoalInsert = CreateInsertStatement(attack.Goal, "Attack_Goal");
            bool insertShot = attack.Shots.Count > 0;

            string attackInsert = CreateInsertStatement(attack.DbAttack, "Attack");
            string attackShotInsert = string.Empty;
            if (insertShot)
                attackShotInsert = CreateInsertStatement(attack.Shots, "Attack_Shot");

            string attackReboundInsert = string.Empty;
            bool insertRebounds = attack.Rebounds.Count > 0;
            if (insertRebounds)
                CreateInsertStatement(attack.Rebounds, "Attack_Rebound");

            MySqlCommand command = new MySqlCommand();
            command.Connection = RemoteConnection;

            MySqlTransaction transaction = RemoteConnection.BeginTransaction();
            command.Transaction = transaction;
            try
            {
                if (isGoal)
                {
                    command.CommandText = attackGoalInsert;
                    command.ExecuteNonQuery();
                }
                command.CommandText = attackInsert;
                command.ExecuteNonQuery();
                if (insertShot)
                {
                    command.CommandText = attackShotInsert;
                    command.ExecuteNonQuery();
                }
                if (insertRebounds)
                {
                    command.CommandText = attackReboundInsert;
                    command.ExecuteNonQuery();
                }

                transaction.Commit();
            } catch (Exception e)
            {
                transaction.Rollback();
                Console.WriteLine(e.Message);
            }
            

        }

        public void GetUsers()
        {
            throw new NotImplementedException();
        }

        public void GetPlayers()
        {
            throw new NotImplementedException();
        }

        public void GetCoaches()
        {
            throw new NotImplementedException();
        }

        public void GetTeams()
        {
            throw new NotImplementedException();
        }

        public void GetGames(Guid teamId)
        {
            throw new NotImplementedException();
        }

        public void GetAttacks(Guid teamId)
        {
            ExecuteStoredProcedureByTeamId<DbAttack>("GetAllAttackByTeamId", teamId);
        }

        public void GetAttackRebound(Guid teamId)
        {
            ExecuteStoredProcedureByTeamId<DbAttackRebound>("GetAllAttackReboundByTeamId", teamId);
        }

        public void GetAttackShot(Guid teamId)
        {
            ExecuteStoredProcedureByTeamId<DbAttackShot>("GetAllAttackShotByTeamId", teamId);
        }

        public void GetAttackGoal(Guid teamId)
        {
            ExecuteStoredProcedureByTeamId<DbAttackGoal>("GetAllAttackGoalByTeamId", teamId);
        }

        public void GetFormation(Guid teamId)
        {
            ExecuteStoredProcedureByTeamId<DbFormation>("GetAllFormationByTeamId", teamId);
        }

        public void GetPlayer(Guid teamId)
        {
            string query = $"SELECT * FROM Player JOIN User using (Id) WHERE Team_Id = '{teamId}'";
            var command = new MySqlCommand(query);
            var data = ExecuteMultiDataQuery<DbPlayer>(command);
            SaveToLocalDb(data);
        }

        private void SaveToLocalDb<T>(T[] data)
        {
                if (data.Length == 0)
                    return;

                foreach (T item in data)
                {
                    LocalConnection.InsertOrReplace(item);
                }
        }

        private void ExecuteStoredProcedureByTeamId<T>(string procedure, Guid teamId)
        {
            try
            {
                string storedProcedure = procedure;
                MySqlCommand command = new MySqlCommand(storedProcedure, RemoteConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("TeamId", teamId);

                var reader = command.ExecuteReader();
                var data = ReadDataList<T>(reader);
                SaveToLocalDb(data);
                
            } catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void UpdateLocalDatabase(Guid teamId)
        {
            RemoteConsumer.AddAction(() => GetFormation(teamId));
            RemoteConsumer.AddAction(() => GetPlayer(teamId));
            RemoteConsumer.AddAction(() => GetAttacks(teamId));
            RemoteConsumer.AddAction(() => GetAttackGoal(teamId));
            RemoteConsumer.AddAction(() => GetAttackRebound(teamId));
            RemoteConsumer.AddAction(() => GetAttackShot(teamId));
        }
    }
}