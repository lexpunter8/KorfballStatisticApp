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
using KorfbalStatistics.Adapters;
using KorfbalStatistics.CustomviewClasses;
using KorfbalStatistics.Model;
using KorfbalStatistics.Viewmodel;
using OxyPlot.Xamarin.Android;

namespace KorfbalStatistics.Fragments
{
    public class AllStatisticFragment : Android.Support.V4.App.Fragment
    {
        private ViewFlipper viewFlipper;
        private StatisticOverViewViewModel myViewModel;
        private ExpandableListView myAttackList;
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
            View view = inflater.Inflate(Resource.Layout.all_statistics_view, container, false);

            viewFlipper = view.FindViewById<ViewFlipper>(Resource.Id.viewFlipper1);
            
            var viewSwitchGroup = view.FindViewById<MultiLineRadioGroup>(Resource.Id.viewSwitchGroup);
            viewSwitchGroup.CheckedChanged += ViewSwitchGroup_CheckedChanged;
            myViewModel.CurrentStatisticViewModel.CurrentZoneToShowFilter = Enums.EZoneFunction.None;


            view.FindViewById(Resource.Id.header).FindViewById<TextView>(Resource.Id.headerText).Text = "Team statistieken overzicht";
            view.FindViewById(Resource.Id.header).FindViewById<TextView>(Resource.Id.teamName).Visibility = ViewStates.Invisible;

            LoadStats();
            return view;
        }

        private void ViewSwitchGroup_CheckedChanged(object sender, EventArgs e)
        {
            var group = sender as ItemIdHolderRadioGroup;
            ItemHolderRadioButton selectedButton = group.GetSelected();
            if (selectedButton.Text.Equals("Aanvallen"))
            {
                viewFlipper.DisplayedChild = viewFlipper.IndexOfChild(viewFlipper.FindViewById(Resource.Id.attacksList));
                SetAttacksView();
            }
            else
            {
                viewFlipper.DisplayedChild = viewFlipper.IndexOfChild(viewFlipper.FindViewById(Resource.Id.overView));
                LoadStats();
            }
        }

        private void SetAttacksView()
        {
            var attacksView = viewFlipper.CurrentView;
            myAttackList = attacksView.FindViewById<ExpandableListView>(Resource.Id.attacksList);
            myAttackList.SetAdapter(new AttackListAdapter(myViewModel.Attacks, Activity));
            myAttackList.ItemClick += MyAttackList_ItemClick;
        }

        private void MyAttackList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
        }

        private void LoadStats()
        {
            var viewModel = myViewModel.CurrentStatisticViewModel;
            viewModel.SetAttacks();

            var flipperView = viewFlipper.GetChildAt(0);
            plotView = flipperView.FindViewById<PlotView>(Resource.Id.plotView1);
            statShot = flipperView.FindViewById<CardView>(Resource.Id.statShot);
            statRebound = flipperView.FindViewById<CardView>(Resource.Id.statRebound);
            statInterception = flipperView.FindViewById<CardView>(Resource.Id.statInterceptions);
            statShotclock = flipperView.FindViewById<CardView>(Resource.Id.statShotClock);
            statConcededShot = flipperView.FindViewById<CardView>(Resource.Id.statConcededShot);
            statAttackCount = flipperView.FindViewById<CardView>(Resource.Id.statAttackCount);

            statShot.FindViewById<TextView>(Resource.Id.headerText).Text = "Doelpunten / schoten";
            statShot.FindViewById<TextView>(Resource.Id.statText).Text = viewModel.GoalCount + " / " + viewModel.ShotCount;
            double percentageGoal = (Convert.ToDouble(viewModel.GoalCount) / viewModel.ShotCount) * 100;

            statShot.FindViewById<TextView>(Resource.Id.statDetailText).Text = string.Format("{0}%", percentageGoal);

            statRebound.FindViewById<TextView>(Resource.Id.headerText).Text = "Aanvallende / verdedigende rebounds";
            statRebound.FindViewById<TextView>(Resource.Id.statText).Text = viewModel.ReboundCount + " / " + viewModel.DevensiveReboundCount;

            statInterception.FindViewById<TextView>(Resource.Id.headerText).Text = "Onderscheppingen / Balverlies";
            statInterception.FindViewById<TextView>(Resource.Id.statText).Text = viewModel.InterceptionCount + " / " + viewModel.TurnoverCount;

            statShotclock.FindViewById<TextView>(Resource.Id.headerText).Text = "Schotklok overschrijding";
            statShotclock.FindViewById<TextView>(Resource.Id.statText).Text = viewModel.ShotClokcOverrideCount.ToString();

            statConcededShot.FindViewById<TextView>(Resource.Id.headerText).Text = "CDoelpunten / schoten tegen";
            statConcededShot.FindViewById<TextView>(Resource.Id.statText).Text = viewModel.ConcededGoalCount + " / " + viewModel.ConcededShotCount;

            statAttackCount.FindViewById<TextView>(Resource.Id.headerText).Text = "Aantal aanvallen";
            statAttackCount.FindViewById<TextView>(Resource.Id.statText).Text = viewModel.AttackCount.ToString();


            plotView.Model = myViewModel.CurrentStatisticViewModel.PlotModel;

        }

    }
}