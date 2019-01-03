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
using KorfbalStatistics.CustomviewClasses;
using Android.Support.V7.Widget;
using KorfbalStatistics.Viewmodel;
using KorfbalStatistics.Adapters;
using Android.Support.Design.Widget;
using System.Collections.ObjectModel;
using KorfbalStatistics.Model;
using KorfbalStatistics.Interface;

namespace KorfbalStatistics
{
    [Activity(Label = "HomeActivity")]
    public class HomeActivity : Activity
    {
        private HomeViewModel myViewModel;
        private ListView gameListView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_home);
            //CardView card = (CardView)FindViewById(Resource.Id.cardView);
            //TextView text = (TextView)card.FindViewById(Resource.Id.headerText);
            //text.SetText("Text from Home".ToCharArray(), 0, 14);
            
            
            myViewModel = new HomeViewModel(DbManager.Instance);
            myViewModel.PropertyChanged += MyViewModel_PropertyChanged;

            gameListView = FindViewById<ListView>(Resource.Id.gamesListView);
            gameListView.Adapter = new GameListAdapter(myViewModel.Games, this);
            gameListView.ItemClick += GameListView_ItemClick;
            // Create your application here
            FloatingActionButton addGameButton = FindViewById<FloatingActionButton>(Resource.Id.addGameButton);

            addGameButton.Click += AddGameButton_Click;

            FindViewById<ImageButton>(Resource.Id.leftActionButton).Click += RightActionButton_Clicked;
            FindViewById<ImageButton>(Resource.Id.rightActionButton).Visibility = ViewStates.Invisible;

            myViewModel.GetGames();
        }

        private void RightActionButton_Clicked(object sender, EventArgs e)
        {
            StartActivity(typeof(PlayersActivity));
        }

        private void MyViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            GameListAdapter ad = gameListView.Adapter as GameListAdapter;
            ad.NotifyDataSetChanged();
        }

        private void AddGameButton_Click(object sender, EventArgs e)
        {

            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            LayoutInflater inflater = LayoutInflater;
            View view = inflater.Inflate(Resource.Layout.popup_new_game_layout, null);

            EditText opponentName = view.FindViewById<EditText>(Resource.Id.opponentInputText);
            CheckBox isHomeCheckBox = view.FindViewById<CheckBox>(Resource.Id.isHomeCheckBox);

            //string text = MainViewModel.Instance.GetDb().ToString();
            builder.SetView(view)
                .SetPositiveButton("OK", (s, args) =>
                {
                    //MainViewModel.Instance.GetData();
                    myViewModel.CreateGame(opponentName.Text, DateTime.Now, isHomeCheckBox.Checked);
                })
                .SetNegativeButton("Cancel", (s, args) => {
                    //MainViewModel.Instance.DataBase();
                });

            builder.Show();
            
        }

        private void GameListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            GameListAdapter ad = gameListView.Adapter as GameListAdapter;
            DbGame game = ad.GetItem(e.Position);
            MainViewModel.Instance.CurrentGame = game;
            Intent intent = new Intent(this, typeof(GameStatisticsActivity));
            StartActivity(intent);
        }
    }
}