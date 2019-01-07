using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using KorfbalStatistics.CustomviewClasses;
using KorfbalStatistics.Viewmodel;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Xamarin.Android;
using static KorfbalStatistics.Model.Enums;

namespace KorfbalStatistics.Fragments
{
    public class ZoneStatisticFragment : Android.Support.V4.App.Fragment
    {
        private StatisticOverViewViewModel myViewModel;
        private CardView statShot, statRebound, statInterception, statShotclock, statConcededShot, statAttackCount;
        PlotView plotView;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            StatisticOverViewActivity activity = (StatisticOverViewActivity)Activity;
            myViewModel = activity.ViewModel;
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_zone_statistic, container, false);
            plotView = view.FindViewById<PlotView>(Resource.Id.plotView1);
            statShot = view.FindViewById<CardView>(Resource.Id.statShot);
            statRebound = view.FindViewById<CardView>(Resource.Id.statRebound);
            statInterception = view.FindViewById<CardView>(Resource.Id.statInterceptions);
            statShotclock = view.FindViewById<CardView>(Resource.Id.statShotClock);
            statConcededShot = view.FindViewById<CardView>(Resource.Id.statConcededShot);
            statAttackCount = view.FindViewById<CardView>(Resource.Id.statAttackCount);
            var attackDefenceGroup = view.FindViewById<MultiLineRadioGroup>(Resource.Id.attackDefenceGroup);
            attackDefenceGroup.CheckedChanged += AttackDefenceGroup_CheckedChanged;

            view.FindViewById(Resource.Id.header).FindViewById<TextView>(Resource.Id.headerText).Text = "Vak statistieken overzicht";
            view.FindViewById(Resource.Id.header).FindViewById<TextView>(Resource.Id.teamName).Visibility = ViewStates.Invisible;

            LoadStats(); 

            return view;
        }

        private void AttackDefenceGroup_CheckedChanged(object sender, EventArgs e)
        {
            var group = sender as ItemIdHolderRadioGroup;
            ItemHolderRadioButton selectedButton = group.GetSelected();
            if (selectedButton.Text.Equals("Verdediging"))
            {
                myViewModel.CurrentStatisticViewModel.CurrentZoneToShowFilter = EZoneFunction.Defence;
            }
            else
                myViewModel.CurrentStatisticViewModel.CurrentZoneToShowFilter = EZoneFunction.Attack;
            LoadStats();
        }

        private void AttackDefenceGroup_CheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            
        }

        private void LoadStats()
        {
            var viewModel = myViewModel.CurrentStatisticViewModel;
            viewModel.SetAttacks();

            statShot.FindViewById<TextView>(Resource.Id.headerText).Text = "Doelpunten / schoten";
            statShot.FindViewById<TextView>(Resource.Id.statText).Text = viewModel.GoalCount + " / " + viewModel.ShotCount;
            double percentageGoal = (Convert.ToDouble(viewModel.GoalCount) / viewModel.ShotCount) * 100;

            statShot.FindViewById<TextView>(Resource.Id.statDetailText).Text = string.Format("{0}%", percentageGoal);

            statRebound.FindViewById<TextView>(Resource.Id.headerText).Text = "aanvallende / verdedigende rebounds";
            statRebound.FindViewById<TextView>(Resource.Id.statText).Text = viewModel.ReboundCount + " / " + viewModel.DevensiveReboundCount ;

            statInterception.FindViewById<TextView>(Resource.Id.headerText).Text = "Onderscheppingen / balverlies";
            statInterception.FindViewById<TextView>(Resource.Id.statText).Text = viewModel.InterceptionCount + " / " + viewModel.TurnoverCount;

            statShotclock.FindViewById<TextView>(Resource.Id.headerText).Text = "Schotklok overschrijding";
            statShotclock.FindViewById<TextView>(Resource.Id.statText).Text = viewModel.ShotClokcOverrideCount.ToString();

            statConcededShot.FindViewById<TextView>(Resource.Id.headerText).Text = "Doelpunten / schoten tegen";
            statConcededShot.FindViewById<TextView>(Resource.Id.statText).Text = viewModel.ConcededGoalCount + " / " + viewModel.ConcededShotCount;

            statAttackCount.FindViewById<TextView>(Resource.Id.headerText).Text = "Aantal aanvallen";
            statAttackCount.FindViewById<TextView>(Resource.Id.statText).Text = viewModel.AttackCount.ToString();


            plotView.Model = myViewModel.CurrentStatisticViewModel.PlotModel;

        }
    }
}