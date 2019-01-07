using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using KorfbalStatistics.Adapters;
using KorfbalStatistics.CustomviewClasses;
using KorfbalStatistics.Model;
using KorfbalStatistics.Viewmodel;

namespace KorfbalStatistics
{
    [Activity(Label = "PlayersActivity")]
    public class PlayersActivity : Activity
    {
        private PlayersViewModel myViewModel;
        private ListView myPlayersList;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_players);

            myViewModel = new PlayersViewModel();

            myPlayersList = FindViewById<ListView>(Resource.Id.playersList);
            myPlayersList.Adapter = new DetailedPlayerListAdapter(myViewModel.Players, this);

            myPlayersList.ItemClick += PlayersList_ItemClick;
            FindViewById<FloatingActionButton>(Resource.Id.addPlayerButton).Click += AddPlayerButton_Click;
            FindViewById<RoundedTextView>(Resource.Id.teamName).Text = MainViewModel.Instance.Team.Name;
            FindViewById<TextView>(Resource.Id.headerText).Text = "Spelers";

        }

        private void AddPlayerButton_Click(object sender, EventArgs e)
        {
            ShowPlayerPopup();
        }

        private void PlayersList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

            ListView listview = (ListView)sender;
            DetailedPlayerListAdapter adapter = listview.Adapter as DetailedPlayerListAdapter;
            DbPlayer player = adapter.GetItem(e.Position);
            ShowPlayerPopup(player);
        }

        private void ShowPlayerPopup(DbPlayer player = null)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            LayoutInflater inflater = LayoutInflater;
            View view = inflater.Inflate(Resource.Layout.update_add_player_view, null);

            EditText playerName = view.FindViewById<EditText>(Resource.Id.playerName);
            EditText abbrevationName = view.FindViewById<EditText>(Resource.Id.playerAbbrevation);
            EditText numberName = view.FindViewById<EditText>(Resource.Id.playerNumber);

            bool newPlayer = false;
            if (player != null)
            {
                playerName.Text = player.Name;
                abbrevationName.Text = player.Abbrevation;
                numberName.Text = player.Number.ToString();
            } else
            {
                player = new DbPlayer();
                newPlayer = true;
            }
           

            //string text = MainViewModel.Instance.GetDb().ToString();
            builder.SetView(view)
                .SetPositiveButton("OK", (s, args) =>
                {
                    player.Name = playerName.Text;
                    player.Abbrevation = abbrevationName.Text;
                    player.Number = Convert.ToInt16(numberName.Text);
                    if (newPlayer)
                        myViewModel.AddPlayer(player);
                    else
                        myViewModel.UpdatePlayer(player);
                })
                .SetNegativeButton("Cancel", (s, args) => {
                });

            builder.Show();
        }
    }
}