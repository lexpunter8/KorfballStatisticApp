using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using KorfbalStatistics.Adapters;
using KorfbalStatistics.Model;
using KorfbalStatistics.Viewmodel;

namespace KorfbalStatistics.Fragments
{
    public class PlayerStatisticFragment : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_player_statistic, container, false);
            List<DbFormation> formation = DbManager.Instance.FormationDbManager.GetFormationByGameId(MainViewModel.Instance.CurrentGame.Id).ToList();
            var playerStats = new List<PlayerStatisticViewModel>();
            formation.ForEach(p => playerStats.Add(new PlayerStatisticViewModel(p.PlayerId)));

            var playerList = view.FindViewById<ExpandableListView>(Resource.Id.playerList);
            playerList.SetAdapter(new PlayerStatisticsAdapter(playerStats, Activity));
            playerList.ItemClick += PlayerList_ItemClick;


            view.FindViewById(Resource.Id.header).FindViewById<TextView>(Resource.Id.headerText).Text = "Speler statistieken";
            view.FindViewById(Resource.Id.header).FindViewById<TextView>(Resource.Id.teamName).Visibility = ViewStates.Invisible;


            return view;
        }

        private void PlayerList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
        }
    }
}