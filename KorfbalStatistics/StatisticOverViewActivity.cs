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
        public StatisticOverViewViewModel myViewModel;
        private ExpandableListView myAttackList;
        private FragmentTabHost myTabHost;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_statisticoverview);
            // Create your application here
            myViewModel = new StatisticOverViewViewModel(DbManager.Instance);

            myTabHost = FindViewById<FragmentTabHost>(Resource.Id.tabHost);
            myTabHost.Setup(this, SupportFragmentManager, Resource.Id.tabContainer);

            //Tab 1
            myTabHost.AddTab(myTabHost.NewTabSpec("all").SetIndicator("All"),
                Java.Lang.Class.FromType(typeof(AllStatisticFragment)), null);
            //Tab 2
            myTabHost.AddTab(myTabHost.NewTabSpec("player").SetIndicator("Player"),
                Java.Lang.Class.FromType(typeof(PlayerStatisticFragment)), null);

            //Tab 3
            myTabHost.AddTab(myTabHost.NewTabSpec("zone").SetIndicator("Zone"),
                Java.Lang.Class.FromType(typeof(ZoneStatisticFragment)), null);
            
        }
    }
}