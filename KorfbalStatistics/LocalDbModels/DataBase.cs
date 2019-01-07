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
                    connection.Insert(new DbCoach { Id = Guid.NewGuid(), TeamId = teamGuid, Name = "Lex Punter", Sex = "M"});
                    Guid playerGuid1 = Guid.NewGuid();
                    Guid playerGuid2 = Guid.NewGuid();
                    Guid playerGuid3 = Guid.NewGuid();
                    Guid playerGuid4 = Guid.NewGuid();
                    Guid playerGuid5 = Guid.NewGuid();
                    Guid playerGuid6 = Guid.NewGuid();
                    Guid playerGuid7 = Guid.NewGuid();
                    Guid playerGuid8 = Guid.NewGuid();
                    Guid playerGuid9 = Guid.NewGuid();
                    Guid playerGuid10 = Guid.NewGuid();
                    connection.Insert(new DbPlayer { Id = playerGuid1, TeamId = teamGuid, Sex = "M", FirstName = "Jelmer", Name = "bij de Leij", Number = 1, Abbrevation = "LeJ" });
                    connection.Insert(new DbPlayer { Id = playerGuid2, TeamId = teamGuid, Sex = "M", FirstName = "Jurjen", Name = "Veenstra", Number = 2, Abbrevation = "VeJ" });
                    connection.Insert(new DbPlayer { Id = playerGuid3, TeamId = teamGuid, Sex = "M", FirstName = "Marten", Name = "van Houten", Number = 3, Abbrevation = "HuM" });
                    connection.Insert(new DbPlayer { Id = playerGuid4, TeamId = teamGuid, Sex = "M", FirstName = "Julian", Name = "Punter", Number = 4, Abbrevation = "PuJ" });
                    connection.Insert(new DbPlayer { Id = playerGuid5, TeamId = teamGuid, Sex = "M", FirstName = "Bas", Name = "Haaijer", Number = 5, Abbrevation = "HaB" });
                    connection.Insert(new DbPlayer { Id = playerGuid6, TeamId = teamGuid, Sex = "F", FirstName = "Lionne", Name = "Haaijer", Number = 6, Abbrevation = "HaL" });
                    connection.Insert(new DbPlayer { Id = playerGuid7, TeamId = teamGuid, Sex = "F", FirstName = "Maaike", Name = "van der Schuit", Number = 7, Abbrevation = "ScM" });
                    connection.Insert(new DbPlayer { Id = playerGuid8, TeamId = teamGuid, Sex = "F", FirstName = "Maike", Name = "de Jong", Number = 8, Abbrevation = "JoM" });
                    connection.Insert(new DbPlayer { Id = playerGuid9, TeamId = teamGuid, Sex = "F", FirstName = "Aniek", Name = "Hansma", Number = 9, Abbrevation = "HaA" });
                    connection.Insert(new DbPlayer { Id = playerGuid10, TeamId = teamGuid, Sex = "F", FirstName = "Fardau", Name = "van Houten", Number = 10, Abbrevation = "HuF" });
                    connection.Insert(new DbPlayer { Id = Guid.Parse("6467eac9-0164-4041-adc6-4b7b038c1a7d"), TeamId = Guid.Empty, Sex = "U", FirstName = "Onbekend", Number = -1, Abbrevation = "" });
                    connection.Insert(new DbPlayer { Id = Guid.Parse("4aeab093-8e88-4b41-aa4d-1aa41ba7a8fd"), TeamId = Guid.Empty, Sex = "U", FirstName = "Tegenstander", Number = -1, Abbrevation = "OPP" });

                    Guid gameId = Guid.NewGuid();
                    connection.Insert(new DbGame { Id = gameId, TeamId = teamGuid, Status = "H1",
                        Opponent = "TestOpponent", IsHome = true, Date = DateTime.Now });

                    connection.Insert(new DbFormation { Id = Guid.NewGuid(), GameId = gameId, PlayerId = playerGuid1, StartFunction = "A", CurrentFunction = "A" });
                    connection.Insert(new DbFormation { Id = Guid.NewGuid(), GameId = gameId, PlayerId = playerGuid2, StartFunction = "A", CurrentFunction = "A" });
                    connection.Insert(new DbFormation { Id = Guid.NewGuid(), GameId = gameId, PlayerId = playerGuid3, StartFunction = "A", CurrentFunction = "A" });
                    connection.Insert(new DbFormation { Id = Guid.NewGuid(), GameId = gameId, PlayerId = playerGuid4, StartFunction = "A", CurrentFunction = "A" });
                    connection.Insert(new DbFormation { Id = Guid.NewGuid(), GameId = gameId, PlayerId = playerGuid5, StartFunction = "D", CurrentFunction = "D" });
                    connection.Insert(new DbFormation { Id = Guid.NewGuid(), GameId = gameId, PlayerId = playerGuid6, StartFunction = "D", CurrentFunction = "D" });
                    connection.Insert(new DbFormation { Id = Guid.NewGuid(), GameId = gameId, PlayerId = playerGuid7, StartFunction = "D", CurrentFunction = "D" });
                    connection.Insert(new DbFormation { Id = Guid.NewGuid(), GameId = gameId, PlayerId = playerGuid8, StartFunction = "D", CurrentFunction = "D" });
                    connection.Insert(new DbFormation { Id = Guid.NewGuid(), GameId = gameId, PlayerId = playerGuid9, StartFunction = "S", CurrentFunction = "S" });
                    connection.Insert(new DbFormation { Id = Guid.NewGuid(), GameId = gameId, PlayerId = playerGuid10, StartFunction = "S",  CurrentFunction = "S" });

                    connection.Insert(new DbGoalType { Id = Guid.Parse("b8ba5726-b2bb-4ab8-a52f-fd041fcb0d85"), Name = "Kleine kans" });
                    connection.Insert(new DbGoalType { Id = Guid.Parse("d8a3a228-94df-4060-af95-8f6a1a1bf93c"), Name = "Kort schot" });
                    connection.Insert(new DbGoalType { Id = Guid.Parse("9ae1c88c-4469-4ad9-a65d-40a825696672"), Name = "Afstandschot" });
                    connection.Insert(new DbGoalType { Id = Guid.Parse("4d70f737-d787-43fb-b9c8-2ed40049f555"), Name = "Doorloopbal" });
                    connection.Insert(new DbGoalType { Id = Guid.Parse("4d2eade7-ebdb-441a-bd52-896d9dfb4a80"), Name = "Vrijebal" });
                    connection.Insert(new DbGoalType { Id = Guid.Parse("5dc42cda-3613-498c-b5e1-4f2936dd0be7"), Name = "Strafworp" });
                    connection.Insert(new DbGoalType { Id = Guid.Parse("51273c25-8193-4ea1-9145-de9648700b00"), Name = "Onbekend" });
                    

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
                createDatabase(true);
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