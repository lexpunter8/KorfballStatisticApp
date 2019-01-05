using KorfbalStatistics.Interface;
using KorfbalStatistics.Model;
using KorfbalStatistics.Services;
using KorfbalStatistics.Viewmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using static KorfbalStatistics.Model.Enums;

namespace KorfbalStatistics.Command
{
    public abstract class AttackEndingCommand : BaseStatisticCommand
    {
        public AttackEndingCommand(GameStatisticViewModel gameStatisticViewModel)
        {
            myGameStat = gameStatisticViewModel;
            myGameService = ServiceLocator.GetService<GameService>();
            myCurrentAttack = gameStatisticViewModel.CurrentAttack;
            myOldAttack = myCurrentAttack.Clone() as Attack;

            myOldCurrentFunction = gameStatisticViewModel.CurrentFunction;
            myOldStartingFunction = gameStatisticViewModel.StartingFunction;
        }

        protected GameStatisticViewModel myGameStat;
        protected IAttack myCurrentAttack { get; set; }
        protected IGameService myGameService { get; set; }
        protected EZoneFunction myOldCurrentFunction;
        protected EZoneFunction myOldStartingFunction;
        protected IAttack myOldAttack;

        public override void Execute()
        {
            myGameService.AddAttackToDb(myCurrentAttack);
            myGameStat.ChangeFunction(myCurrentAttack.Goal != null);
           // EZoneFunction startfunction = myGameStat.StartingFunction == EZoneFunction.Defence ? EZoneFunction.Attack : EZoneFunction.Defence;
            bool isFirstHalf = myGameStat.GameStatus == "H1" ? true : false;
            if (myGameStat.CurrentFunction == EZoneFunction.Attack)
            {
                myGameStat.CurrentAttack = new Attack(EZoneFunction.Attack, myGameStat.StartingFunction, myGameStat.Game.Id, isFirstHalf);
            }

            else
                myGameStat.CurrentAttack = new Attack(EZoneFunction.Defence, myGameStat.StartingFunction, myGameStat.Game.Id, isFirstHalf);
            IsCompleted = true;
        }

        public override void Redo()
        {
        }

        public override void Undo()
        {
            myGameService.RemoveAttackFromDb(myCurrentAttack);
            myGameStat.StartingFunction = myOldStartingFunction;
            myGameStat.CurrentFunction = myOldCurrentFunction;
            myGameStat.CurrentAttack = myOldAttack;
        }
    }
}