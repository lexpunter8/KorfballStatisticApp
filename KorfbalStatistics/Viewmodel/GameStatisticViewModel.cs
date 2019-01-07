using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using KorfbalStatistics.Command;
using KorfbalStatistics.Interface;
using KorfbalStatistics.Model;
using KorfbalStatistics.Services;
using static KorfbalStatistics.Model.Enums;

namespace KorfbalStatistics.Viewmodel
{
    public class GameStatisticViewModel : BaseViewModel
    {
        public GameStatisticViewModel(DbManager dbManager)
        {
            myDbManager = dbManager;
            myGameDbManager = myDbManager.GameDbManager;
            Game = MainViewModel.Instance.CurrentGame;
            StartingFunction = Game.IsHome ? EZoneFunction.Attack : EZoneFunction.Defence;
            CurrentFunction = Game.IsHome ? EZoneFunction.Attack : EZoneFunction.Defence;
            CurrentAttack = new Attack(CurrentFunction, StartingFunction, Game.Id, true);

            myCommandManager = new CommandManager();
        }

        private DbManager myDbManager { get; set; }
        private CommandManager myCommandManager { get; set; }
        private IGameDbManager myGameDbManager;
        private FormationService myFormationService = ServiceLocator.GetService<FormationService>();

        // the current function, attacking or defending 
        private EZoneFunction myCurrentFunction { get; set; }
        // the starting function of the current zone, attack or defence 
        public EZoneFunction StartingFunction { get; set; }
        public EZoneFunction CurrentFunction
        {
            get => myCurrentFunction;
            set
            {
                myCurrentFunction = value;
                OnPropertyChanged(nameof(CurrentFunction));
            }
        }

        private IStatisticCommand myCurrentStatistic;
        public IStatisticCommand CurrentStatistic {
            get => myCurrentStatistic;
            set
            {
                myCommandManager.AddCommand(value);
                myCurrentStatistic = value;
            }
        }
        public DbGame Game { get; set; }
        public IAttack CurrentAttack { get; set; }

        public void ChangeFunction(bool isGoal)
        {
            //   s    function
            // attack attack ==> attack defence
            // defenc attack ==> defenc defence 
            // attack defenc ==> attack attack

            int totalGoal = GetScore(false) + GetScore(true);
            if (totalGoal % 2 == 1 || !isGoal)
            {
                StartingFunction = StartingFunction == EZoneFunction.Attack
                                                            ? EZoneFunction.Defence : EZoneFunction.Attack;
                CurrentFunction = CurrentFunction == EZoneFunction.Attack ? EZoneFunction.Defence : EZoneFunction.Attack;

                return;
            }
            int iets = totalGoal % 4;
            if (iets == 2 || iets == 3)
            {
                CurrentFunction = StartingFunction == EZoneFunction.Attack ? EZoneFunction.Defence : EZoneFunction.Attack;
            }
            else
            {
                CurrentFunction = StartingFunction == EZoneFunction.Attack ? EZoneFunction.Attack : EZoneFunction.Defence;
            }
            // 1 2 3 4 5 6 7 8 9 10
            // a a d d a a d d a a
            // 2 2 2 2 2 2 2 2 2 2 
            // 1 0 1 0 1 0 1 0 1 0
            // 4 4 4 4 4 4 4 4 4 4
            // 1 2 3 0 1 2 3 0 1 2
        }
        private List<Player> myCurrentPlayers;

        public List<Player> CurrentPlayers
        {
            get
            {
                if (myCurrentPlayers == null)
                    myCurrentPlayers = myFormationService.GetFormationForGame(gameId: Game.Id).ToList();
                return myCurrentPlayers;
            }
        }

        public List<Player> GetPlayers()
        {
            if (StartingFunction == EZoneFunction.Attack)
                return CurrentPlayers.Where(p => p.ZoneFunction == EPlayerFunction.Attack).ToList();
            if (StartingFunction == EZoneFunction.Defence)
                return CurrentPlayers.Where(p => p.ZoneFunction == EPlayerFunction.Defence).ToList();
            else return null;
        }

        internal void Init()
        {
            OnPropertyChanged("UpdateAll");
        }

        public int AwayScore => GetScore(Game.IsHome ? true : false);
        public int HomeScore => GetScore(Game.IsHome ? false : true);

        public string AwayTeam => Game.IsHome ? Game.Opponent : "LDODK";
        public string HomeTeam => Game.IsHome ? "LDODK" : Game.Opponent;

        public string GameStatus => Game.Status;

        public void SetGameStatus(string status)
        {
            Game.Status = status;
        }
        public int GetScore(bool opponentScore)
        {
            List<DbAttack> attacks = myGameDbManager.GetAttacksForGame(Game.Id).ToList();
            int score = 0;
            attacks.ForEach(a =>
            {

                if (a.IsOpponentAttack == opponentScore)
                {
                    if (a.GoalId != null)
                    {
                        score++;
                    }
                }
            });
            return score;
        }

        public int GetShotCount()
        {
            int count = 0;
            CurrentAttack.Shots.ForEach(s => count += s.Count);
            return count;
        }

        public int GetReboundCount()
        {
            int count = 0;
            CurrentAttack.Rebounds.ForEach(s => count += s.Count);
            return count;
        }

        public void Redo()
        {
            myCommandManager.RedoLastUndoneAction();
            OnPropertyChanged("UpdateAll");
        }

        public void Undo()
        {
            myCommandManager.UndoLastAction();
            OnPropertyChanged("UpdateAll");
        }

        public void ExecuteCommand()
        {
            myCommandManager.ProccespendingCommands();
            OnPropertyChanged("UpdateAll");
        }
        public void RemoveCommand()
        {
            myCommandManager.RemoveLastAction();
        }
    }
}