using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using KorfbalStatistics.Adapters;
using KorfbalStatistics.Fragments;
using KorfbalStatistics.Model;
using KorfbalStatistics.Viewmodel;

namespace KorfbalStatistics
{
    [Activity(Label = "StatisticOverViewActivity")]
    public class StatisticOverViewActivity : FragmentActivity
    {
        public StatisticOverViewViewModel ViewModel;
        private ExpandableListView myAttackList;
        private FragmentTabHost myTabHost;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_statisticoverview);
            // Create your application here
            ViewModel = new StatisticOverViewViewModel(DbManager.Instance);

            myTabHost = FindViewById<FragmentTabHost>(Resource.Id.tabHost);
            myTabHost.TabChanged += MyTabHost_TabChanged;
            myTabHost.Setup(this, SupportFragmentManager, Resource.Id.tabContainer);

            foreach (var tab in ViewModel.StatisticViewModels)
            {
                var type = tab.GetType();
                if (type == typeof(ZoneStatisticViewModel))
                    myTabHost.AddTab(myTabHost.NewTabSpec("zone").SetIndicator("Vak"),
                        Java.Lang.Class.FromType(typeof(ZoneStatisticFragment)), null);
                if (type == typeof(TeamStatisticViewModel))
                    myTabHost.AddTab(myTabHost.NewTabSpec("team").SetIndicator("Team"),
                        Java.Lang.Class.FromType(typeof(AllStatisticFragment)), null);
                if (type == typeof(PlayerStatisticViewModel))
                    myTabHost.AddTab(myTabHost.NewTabSpec("player").SetIndicator("Speler"),
                        Java.Lang.Class.FromType(typeof(PlayerStatisticFragment)), null);

            }

            //Tab 1
            //Tab 2

        }

        private void MyTabHost_TabChanged(object sender, TabHost.TabChangeEventArgs e)
        {
            var tab = sender as TabHost;
            ViewModel.SetTab(tab.CurrentTab);
            ViewModel.CurrentStatisticViewModel.SetAttacks();
        }
    }
}