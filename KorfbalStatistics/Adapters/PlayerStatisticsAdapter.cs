using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using KorfbalStatistics.Model;
using KorfbalStatistics.Viewmodel;
using OxyPlot.Xamarin.Android;

namespace KorfbalStatistics.Adapters
{
    public class PlayerStatisticsAdapter : BaseExpandableListAdapter
    {
        public PlayerStatisticsAdapter(List<PlayerStatisticViewModel> data, Activity context)
        {
            mydata = data;
            myContext = context;
        }

        public override int GroupCount => mydata.Count;

        public override bool HasStableIds => true;

        private List<PlayerStatisticViewModel> mydata { get; set; }
        private Activity myContext { get; set; }
        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            throw new NotImplementedException();
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public override int GetChildrenCount(int groupPosition)
        {
            return 1;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            PlayerStatisticViewModel item = mydata[groupPosition];
            item.SetAttacks();
            View view = convertView;
            if (view == null)
                view = myContext.LayoutInflater.Inflate(Resource.Layout.base_statistic_layout, null);
            PlotView plotView = view.FindViewById<PlotView>(Resource.Id.plotView1);
            CardView statShot = view.FindViewById<CardView>(Resource.Id.statShot);
            CardView statRebound = view.FindViewById<CardView>(Resource.Id.statRebound);
            CardView statInterception = view.FindViewById<CardView>(Resource.Id.statInterceptions);
            CardView statShotclock = view.FindViewById<CardView>(Resource.Id.statShotClock);
            CardView statConcededShot = view.FindViewById<CardView>(Resource.Id.statConcededShot);
            CardView statAttackCount = view.FindViewById<CardView>(Resource.Id.statAttackCount);
            statShotclock.Visibility = ViewStates.Gone;

            statShot.FindViewById<TextView>(Resource.Id.headerText).Text = "Doelpunten / Schoten";
            statShot.FindViewById<TextView>(Resource.Id.statText).Text = item.GoalCount + " / " + item.ShotCount;
            double percentageGoal = (Convert.ToDouble(item.GoalCount) / item.ShotCount) * 100;

            statShot.FindViewById<TextView>(Resource.Id.statDetailText).Text = string.Format("{0}%", percentageGoal);

            statRebound.FindViewById<TextView>(Resource.Id.headerText).Text = "Aanvallende rebounds";
            statRebound.FindViewById<TextView>(Resource.Id.statText).Text = item.ReboundCount.ToString();

            statInterception.FindViewById<TextView>(Resource.Id.headerText).Text = "Onderscheppingen / Balverlies";
            statInterception.FindViewById<TextView>(Resource.Id.statText).Text = item.InterceptionCount + " / " + item.TurnoverCount;
            
            statConcededShot.FindViewById<TextView>(Resource.Id.headerText).Text = "Doelpunten / schoten tegen";
            statConcededShot.FindViewById<TextView>(Resource.Id.statText).Text = item.ConcededGoalCount + " / " + item.ConcededShotCount;


            statAttackCount.FindViewById<TextView>(Resource.Id.headerText).Text = "Assists";
            statAttackCount.FindViewById<TextView>(Resource.Id.statText).Text = item.AssistCount.ToString();


            plotView.Model = item.PlotModel;
            return view;
        }

        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            return null;
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            var item = mydata[groupPosition];
            View view = convertView;
            if (view == null)
                view = myContext.LayoutInflater.Inflate(Resource.Layout.detailed_player_view, null);
            view.FindViewById<TextView>(Resource.Id.playerAbbrevation).Text = item.Player.Abbrevation;
            view.FindViewById<TextView>(Resource.Id.playerName).Text = item.Player.FirstName + " " + item.Player.Name;
            view.FindViewById<TextView>(Resource.Id.playerNumber).Text = item.Player.Number.ToString();
            return view;
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }
    }
}