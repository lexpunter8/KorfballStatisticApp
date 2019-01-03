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
    public class AllStatisticFragment : Android.Support.V4.App.Fragment
    {
        private StatisticOverViewViewModel myViewModel;
        private ExpandableListView myAttackList;
        public override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.all_statistics_view, container, false);
            StatisticOverViewActivity activity = (StatisticOverViewActivity)Activity;
            myViewModel = activity.myViewModel;

            myAttackList = view.FindViewById<ExpandableListView>(Resource.Id.attacksList);
            myAttackList.SetAdapter(new AttackListAdapter(myViewModel.Attacks, Activity));
            // myAttackList.Adapter = ;
            myAttackList.ItemClick += MyAttackList_ItemClick;
            return view;
        }

        private void MyAttackList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //throw new NotImplementedException();
        }

        
    }
}