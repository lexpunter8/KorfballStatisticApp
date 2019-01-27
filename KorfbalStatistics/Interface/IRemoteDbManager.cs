using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace KorfbalStatistics.Interface
{
    public interface IRemoteDbManager
    {
        void UpdateLocalDatabase(Guid teamId);
        void GetUsers();
        void GetPlayers();
        void GetCoaches();
        void GetTeams();
        void GetGames(Guid teamId);
        void GetAttacks(Guid teamId);
        void GetAttackRebound(Guid teamId);
        void GetAttackShot(Guid teamId);
        void GetAttackGoal(Guid teamId);
        void GetFormation(Guid teamId);

    }
}