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
    public class FormationFragment : Fragment
    {
        private ListView attackPlayersListView, defencePlayersListView;
        private FormationViewModel myVieWModel;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            myVieWModel = new FormationViewModel(DbManager.Instance.FormationDbManager);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.formation_view, container, false);
            attackPlayersListView = view.FindViewById<ListView>(Resource.Id.attackPlayerList);
            attackPlayersListView.ItemClick += AddAttackingPlayer;
            attackPlayersListView.Adapter = new ZonePlayersListAdapter(Activity);
            defencePlayersListView = view.FindViewById<ListView>(Resource.Id.defencePlayerList);
            defencePlayersListView.Adapter = new ZonePlayersListAdapter(Activity);
            defencePlayersListView.ItemClick += AddDefensivePlayer;

            view.FindViewById<Button>(Resource.Id.confirmButton).Click += FormationButton_Click;
            return view;
           // return base.OnCreateView(inflater, container, savedInstanceState);
        }

        private void FormationButton_Click(object sender, EventArgs e)
        {
            bool saved = myVieWModel.SaveFormation();
            if (!saved)
                return;
            GameStatisticsFragment newFragment = new GameStatisticsFragment();
            var trans = Activity.FragmentManager.BeginTransaction();
            trans.Replace(Resource.Id.fragmentContainer, newFragment);
            trans.Commit();
        }

        private void AddAttackingPlayer(object sender, AdapterView.ItemClickEventArgs e)
        {
            addPlayerAction = delegate (DbPlayer player)
            {
                myVieWModel.AddAttackingPlayer(player);
            };
            AddPlayer(sender, e.Position);
        }
        private void AddDefensivePlayer(object sender, AdapterView.ItemClickEventArgs e)
        {
            addPlayerAction = delegate (DbPlayer player)
            {
                myVieWModel.AddDefensivePlayer(player);
            };
            AddPlayer(sender, e.Position);
        }

        private void AddPlayer(object sender, int position)
        {
            ListView listview = (ListView)sender;
            ZonePlayersListAdapter adapter = listview.Adapter as ZonePlayersListAdapter;
            zoneAdapter = adapter;
            DbPlayer player = adapter.GetItem(position);

            if (player == null)
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
                LayoutInflater inflater = LayoutInflater;
                View view = inflater.Inflate(Resource.Layout.select_player_layout, null);
                
                ListView playerList = view.FindViewById<ListView>(Resource.Id.playerList);
                playerList.Adapter = new DetailedPlayerListAdapter(myVieWModel.GetPlayers(), Activity);
                playerList.ItemClick += PlayerList_ItemClick;
                //string text = MainViewModel.Instance.GetDb().ToString();
                builder.SetView(view)
                    .SetNegativeButton("Cancel", (s, args) => {
                    });
                
                alert = builder.Create();
                alert.Show();
            }
        }
        private Action<DbPlayer> addPlayerAction;
        private AlertDialog alert;
        private ZonePlayersListAdapter zoneAdapter = null;
        private void PlayerList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            ListView listview = (ListView)sender;
            DetailedPlayerListAdapter adapter = listview.Adapter as DetailedPlayerListAdapter;
            DbPlayer player = adapter.GetItem(e.Position);
            zoneAdapter.Add(player);
            addPlayerAction.Invoke(player);
            zoneAdapter = null;
            alert.Cancel();
        }
    }
}