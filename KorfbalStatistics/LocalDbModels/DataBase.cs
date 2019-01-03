using System;
using System.Collections.Generic;
using System.IO;
using KorfbalStatistics.Model;
using SQLite;

namespace KorfbalStatistics.LocalDbModels
{
    public class Database
    {
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        

        public bool createDatabase(bool recreate = false)
        {
            if (File.Exists(Path.Combine(folder, "KorfbalStatistics.db")) && !recreate)
            {
                return true;
            }
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(System.IO.Path.Combine(folder, "KorfbalStatistics.db")))
                {
                    if (recreate)
                    {
                        connection.DropTable <DbUser>();
                        connection.DropTable <DbPlayer>();
                        connection.DropTable <DbCoach>();
                        connection.DropTable <DbTeam>();
                        connection.DropTable <DbFormation>();
                        connection.DropTable <DbGame>();
                        connection.DropTable <DbAttack>();
                        connection.DropTable <DbAttackGoal>();
                        connection.DropTable <DbGoalType>();
                        connection.DropTable <DbAttackRebound>();
                        connection.DropTable <DbAttackShot>();

                    }
                    connection.CreateTable<DbUser>();
                    connection.CreateTable<DbPlayer>();
                    connection.CreateTable<DbCoach>();
                    connection.CreateTable<DbTeam>();
                    connection.CreateTable<DbFormation>();
                    connection.CreateTable<DbGame>();
                    connection.CreateTable<DbAttackShot>();
                    connection.CreateTable<DbAttack>();
                    connection.CreateTable<DbAttackGoal>();
                    connection.CreateTable<DbGoalType>();
                    connection.CreateTable<DbAttackRebound>();

                    Guid teamGuid = Guid.NewGuid();
                    connection.Insert(new DbTeam { Id = teamGuid, Name = "B1" });
                    connection.Insert(new DbCoach { Id = Guid.NewGuid(), TeamId = teamGuid, Name = "TestUser", Sex = "M"});
                    Guid playerGuid1 = Guid.NewGuid();
                    Guid playerGuid2 = Guid.NewGuid();
                    Guid playerGuid3 = Guid.NewGuid();
                    Guid playerGuid4 = Guid.NewGuid();
                    Guid playerGuid5 = Guid.NewGuid();
                    Guid playerGuid6 = Guid.NewGuid();
                    Guid playerGuid7 = Guid.NewGuid();
                    Guid playerGuid8 = Guid.NewGuid();
                    Guid playerGuid9 = Guid.NewGuid();
                    connection.Insert(new DbPlayer { Id = playerGuid1, TeamId = teamGuid, Name = "Player1", Number = 1, Abbrevation = "Pl1" });
                    connection.Insert(new DbPlayer { Id = playerGuid2, TeamId = teamGuid, Name = "Player2", Number = 2, Abbrevation = "Pl2" });
                    connection.Insert(new DbPlayer { Id = playerGuid3, TeamId = teamGuid, Name = "Player3", Number = 3, Abbrevation = "Pl3" });
                    connection.Insert(new DbPlayer { Id = playerGuid4, TeamId = teamGuid, Name = "Player4", Number = 4, Abbrevation = "Pl4" });
                    connection.Insert(new DbPlayer { Id = playerGuid5, TeamId = teamGuid, Name = "Player5", Number = 5, Abbrevation = "Pl5" });
                    connection.Insert(new DbPlayer { Id = playerGuid6, TeamId = teamGuid, Name = "Player6", Number = 6, Abbrevation = "Pl6" });
                    connection.Insert(new DbPlayer { Id = playerGuid7, TeamId = teamGuid, Name = "Player7", Number = 7, Abbrevation = "Pl7" });
                    connection.Insert(new DbPlayer { Id = playerGuid8, TeamId = teamGuid, Name = "Player8", Number = 8, Abbrevation = "Pl8" });
                    connection.Insert(new DbPlayer { Id = playerGuid9, TeamId = teamGuid, Name = "Player9", Number = 9, Abbrevation = "Pl9" });

                    Guid gameId = Guid.NewGuid();
                    connection.Insert(new DbGame { Id = gameId, TeamId = teamGuid,
                        Opponent = "TestOpponent", IsHome = true, Date = DateTime.Now });

                    connection.Insert(new DbFormation { Id = Guid.NewGuid(), GameId = gameId, PlayerId = playerGuid1, Function = "A" });
                    connection.Insert(new DbFormation { Id = Guid.NewGuid(), GameId = gameId, PlayerId = playerGuid2, Function = "A" });
                    connection.Insert(new DbFormation { Id = Guid.NewGuid(), GameId = gameId, PlayerId = playerGuid3, Function = "A" });
                    connection.Insert(new DbFormation { Id = Guid.NewGuid(), GameId = gameId, PlayerId = playerGuid4, Function = "A" });
                    connection.Insert(new DbFormation { Id = Guid.NewGuid(), GameId = gameId, PlayerId = playerGuid5, Function = "D" });
                    connection.Insert(new DbFormation { Id = Guid.NewGuid(), GameId = gameId, PlayerId = playerGuid6, Function = "D" });
                    connection.Insert(new DbFormation { Id = Guid.NewGuid(), GameId = gameId, PlayerId = playerGuid7, Function = "D" });
                    connection.Insert(new DbFormation { Id = Guid.NewGuid(), GameId = gameId, PlayerId = playerGuid8, Function = "D" });
                    connection.Insert(new DbFormation { Id = Guid.NewGuid(), GameId = gameId, PlayerId = playerGuid9, Function = "S" });

                    connection.Insert(new DbGoalType { Id = Guid.Parse("b8ba5726-b2bb-4ab8-a52f-fd041fcb0d85"), Name = "Short shot" });
                    connection.Insert(new DbGoalType { Id = Guid.Parse("d8a3a228-94df-4060-af95-8f6a1a1bf93c"), Name = "Medium shot" });
                    connection.Insert(new DbGoalType { Id = Guid.Parse("9ae1c88c-4469-4ad9-a65d-40a825696672"), Name = "Long shot" });
                    connection.Insert(new DbGoalType { Id = Guid.Parse("4d70f737-d787-43fb-b9c8-2ed40049f555"), Name = "Running in shot" });
                    connection.Insert(new DbGoalType { Id = Guid.Parse("4d2eade7-ebdb-441a-bd52-896d9dfb4a80"), Name = "Free-throw" });
                    connection.Insert(new DbGoalType { Id = Guid.Parse("5dc42cda-3613-498c-b5e1-4f2936dd0be7"), Name = "Penalty" });

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool TestDb()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(System.IO.Path.Combine(folder, "KorfbalStatistics.db")))
                {
                    List<SQLiteConnection.ColumnInfo> user = connection.GetTableInfo("User");
                    List<SQLiteConnection.ColumnInfo> player = connection.GetTableInfo("Player");
                    List<SQLiteConnection.ColumnInfo> coach = connection.GetTableInfo("Coach");
                    List<SQLiteConnection.ColumnInfo> team = connection.GetTableInfo("Team");
                    List<SQLiteConnection.ColumnInfo> attack = connection.GetTableInfo("Attack");
                    List<SQLiteConnection.ColumnInfo> formation = connection.GetTableInfo("Formation");
                    List<SQLiteConnection.ColumnInfo> game = connection.GetTableInfo("Game");
                    List<SQLiteConnection.ColumnInfo> attackShot = connection.GetTableInfo("AttackShot");
                    List<SQLiteConnection.ColumnInfo> attackGoal = connection.GetTableInfo("AttackGoal");
                    List<SQLiteConnection.ColumnInfo> goalType = connection.GetTableInfo("GoalType");
                    List<SQLiteConnection.ColumnInfo> attackRebound = connection.GetTableInfo("AttackRebound");
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //Add or Insert Operation  

        public bool insertIntoTable(DbUser person)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "KorfbalStatistics.db")))
                {
                    connection.Insert(person);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public List<DbCoach> selectTable()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "KorfbalStatistics.db")))
                {
                    var list = connection.Table<DbCoach>().ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //Edit Operation  

        //public bool updateTable(DbUser person)
        //{
        //    try
        //    {
        //        using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Persons.db")))
        //        {
        //            connection.Query<DbUser>("UPDATE Person set Name=?, Department=?, Email=? Where Id=?", person.Name, person.Department, person.Email, person.Id);
        //            return true;
        //        }
        //    }
        //    catch (SQLiteException ex)
        //    {
        //        return false;
        //    }
        //}

        //Delete Data Operation  

        public bool removeTable(DbUser person)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "KorfbalStatistics.db")))
                {
                    connection.Delete(person);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //Select Operation  

        public bool selectTable(int Id)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "KorfbalStatistics.db")))
                {
                    connection.Query<DbUser>("SELECT * FROM Person Where Id=?", Id);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        
    }
}