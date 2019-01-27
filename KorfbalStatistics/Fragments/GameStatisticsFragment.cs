using System;
using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using KorfbalStatistics.Model;
using KorfbalStatistics.Viewmodel;
using KorfbalStatistics.CustomExtensions;
using KorfbalStatistics.CustomviewClasses;
using System.Reflection;
using static KorfbalStatistics.Model.Enums;
using KorfbalStatistics.Adapters;
using KorfbalStatistics.Command;
using KorfbalStatistics.Services;
using Android.Support.Design.Widget;

namespace KorfbalStatistics.Fragments
{
    public class GameStatisticsFragment : Fragment
    {
        private GameStatisticViewModel myViewModel;
        private CardView cardViewLeft;
        private CardView cardViewRight;
        private LinearLayout statButtonLayout;
        private LinearLayout actionButtonLayout;
        private ViewFlipper statInputSwitcher;
        private Button okButton, myEndHalfButton;
        private TextView myHomeScoreTextView, myAwayScoreTextView, myGameStatus;
        private SquaredTextViewLayout myCurrentPlayersLayout;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ContextManager.Instance.CurrentContext = Activity.ApplicationContext;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.statistic_game_view, container, false);

            myViewModel = new GameStatisticViewModel(DbManager.Instance);

            Button shotclockButton = view.FindViewById<Button>(Resource.Id.shotclockButton);
            Button turnoverButton = view.FindViewById<Button>(Resource.Id.turnoverButton);
            Button shotButton = view.FindViewById<Button>(Resource.Id.shotButton);
            Button goalButton = view.FindViewById<Button>(Resource.Id.goalButton);

            TextView homeTeam = view.FindViewById<TextView>(Resource.Id.homeTeamText);
            TextView awayTeam = view.FindViewById<TextView>(Resource.Id.awayTeamText);
            homeTeam.Text = myViewModel.HomeTeam;
            awayTeam.Text = myViewModel.AwayTeam;

            shotButton.Click += ShotButton_Click;
            turnoverButton.Click += TurnoverButton_Click;
            shotclockButton.Click += ShotclockButton_Click;
            goalButton.Click += GoalButton_Click;

            cardViewLeft = view.FindViewById<CardView>(Resource.Id.statCardLeft);
            cardViewRight = view.FindViewById<CardView>(Resource.Id.statCardRight);

            view.FindViewById<ImageButton>(Resource.Id.returnButton).Click += ReturnButton_Click;

            statInputSwitcher = view.FindViewById<LinearLayout>(Resource.Id.statsInput).FindViewById<ViewFlipper>(Resource.Id.viewSwitcher1);

            view.FindViewById<LinearLayout>(Resource.Id.statsInput).FindViewById<Button>(Resource.Id.backButton).Click += BackButton_Clicked; ;
            view.FindViewById<LinearLayout>(Resource.Id.statsInput).FindViewById<Button>(Resource.Id.cancelButton).Click += CancelButtonClicked;
            okButton = view.FindViewById<LinearLayout>(Resource.Id.statsInput).FindViewById<Button>(Resource.Id.okButton);
            okButton.Click += StatsInput_OkButton_Clicked;
            myViewModel.PropertyChanged += MyViewModel_PropertyChanged;
            statButtonLayout = statInputSwitcher.FindViewById<LinearLayout>(Resource.Id.buttonLayout);
            actionButtonLayout = view.FindViewById<LinearLayout>(Resource.Id.statsInput).FindViewById<LinearLayout>(Resource.Id.actionButtons);
            okButton.Visibility = ViewStates.Gone;
            statInputSwitcher.FindViewById(Resource.Id.four4Choice).FindViewById<MultiLineRadioGroup>(Resource.Id.radioGroup1).CheckedChanged += GameStatisticsActivity_CheckedChanged;
            statInputSwitcher.FindViewById(Resource.Id.twoChoice).FindViewById<ItemIdHolderRadioGroup>(Resource.Id.radioGroup1).CheckedChange += RadioButtonGroup_CheckedChange;
            statInputSwitcher.FindViewById(Resource.Id.goaltype).FindViewById<MultiLineRadioGroup>(Resource.Id.radioGroup1).CheckedChanged += GameStatisticsActivity_CheckedChanged;

            myGameStatus = view.FindViewById<TextView>(Resource.Id.gameStatus);
            myGameStatus.Text = myViewModel.GameStatus;

            myEndHalfButton = view.FindViewById<Button>(Resource.Id.endHalfButton);
            myEndHalfButton.Click += EndHalfButton_Clicked;

            myCurrentPlayersLayout = view.FindViewById<SquaredTextViewLayout>(Resource.Id.currentPlayersLayout);
            myHomeScoreTextView = view.FindViewById<TextView>(Resource.Id.homeTeamScore);
            myAwayScoreTextView = view.FindViewById<TextView>(Resource.Id.awayTeamScore);

            view.FindViewById(Resource.Id.bottomBar).FindViewById<ImageButton>(Resource.Id.rightActionButton).Click += Bottom_RightButtonClicked;
            view.FindViewById(Resource.Id.bottomBar).FindViewById<ImageButton>(Resource.Id.undoButton).Click += UndoButton_Clicked;
            view.FindViewById(Resource.Id.bottomBar).FindViewById<ImageButton>(Resource.Id.leftActionButton).Click += LeftActionButton_Clicked;
            view.FindViewById(Resource.Id.bottomBar).FindViewById<ImageButton>(Resource.Id.leftActionButton).SetImageDrawable(Context.GetDrawable(Resource.Drawable.ic_playerchange_48px));
           // view.FindViewById(Resource.Id.bottomBar).FindViewById<ImageButton>(Resource.Id.redoButton).Click += (e, args) => myViewModel.Redo();
            myViewModel.Init();
            
            return view;
            // return base.OnCreateView(inflater, container, savedInstanceState);
        }

        private void EndHalfButton_Clicked(object sender, EventArgs e)
        {
            myViewModel.SaveToRemoteDb();
            string currentStatus = myViewModel.Game.Status;
            string newStatus = "";
            if (currentStatus == "H1")
            {
                newStatus = "H2";
                myEndHalfButton.Text = "End H2";
            }
            else if (currentStatus == "H2")
            {
                newStatus = "END";
                myEndHalfButton.Visibility = ViewStates.Invisible;
                return;
            }
            myViewModel.SetGameStatus(newStatus);
        }

        private void UndoButton_Clicked(object sender, EventArgs e)
        {
            Snackbar snackbar = Snackbar.Make(statInputSwitcher, myViewModel.Undo(), Snackbar.LengthLong);
            snackbar.SetAction("Dismis", v => snackbar.Dismiss());
            snackbar.Show();
        }

        private void LeftActionButton_Clicked(object sender, EventArgs e)
        {
            SubstitutePlayer();
        }

        private void ReturnButton_Click(object sender, EventArgs e)
        {
            Activity.Finish();
        }

        private void CancelButtonClicked(object sender, EventArgs e)
        {
            statInputSwitcher.DisplayedChild = statInputSwitcher.IndexOfChild(statInputSwitcher.FindViewById<LinearLayout>(Resource.Id.buttonLayout));
            actionButtonLayout.Visibility = ViewStates.Gone;
            myViewModel.RemoveCommand();

        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            myCurrentStatToGet = myViewModel.CurrentStatistic.GetPreviousStatistic();
            if (myCurrentStatToGet == EStatisticType.None)
            {
                statInputSwitcher.DisplayedChild = statInputSwitcher.IndexOfChild(statInputSwitcher.FindViewById<LinearLayout>(Resource.Id.buttonLayout));
                actionButtonLayout.Visibility = ViewStates.Gone;
                myViewModel.RemoveCommand();
                return;
            }
            SetStatisticLayout();
        }

        private void Bottom_RightButtonClicked(object sender, EventArgs e)
        {
            Activity.StartActivity(typeof(StatisticOverViewActivity));
        }

        private void GameStatisticsActivity_CheckedChanged(object sender, EventArgs e)
        {
            ItemIdHolderRadioGroup radioGroup = sender as ItemIdHolderRadioGroup;
            ItemHolderRadioButton selectedButton = radioGroup.GetSelected();
            myViewModel.CurrentStatistic.SetStatistic(myCurrentStatToGet, selectedButton.ItemId);
            myCurrentStatToGet = myViewModel.CurrentStatistic.GetNextStatistic();
            SetStatisticLayout();
            radioGroup.DeselectAll();
        }

        private List<Player> myCurrentPlayers = new List<Player>();
        private EStatisticType myCurrentStatToGet { get; set; }

        private Action SaveStatistic { get; set; }

        private void MyViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            myGameStatus.Text = myViewModel.Game.Status;
            bool updateAll = e.PropertyName.Equals("UpdateAll");
            if (e.PropertyName.Equals(nameof(myViewModel.CurrentFunction)) || updateAll)
            {
                statButtonLayout.FindViewById<TextView>(Resource.Id.headerButtonText).Text = myViewModel.CurrentFunction.GetDescription();
                string turnoverText = myViewModel.CurrentFunction == EZoneFunction.Attack ? "Balverlies" : "Onderschepping";
                statButtonLayout.FindViewById<Button>(Resource.Id.turnoverButton).Text = turnoverText;
            }
            if (e.PropertyName.Equals(nameof(myViewModel.HomeScore)) || updateAll)
            {
                myHomeScoreTextView.Text = myViewModel.HomeScore.ToString();
            }
            if (e.PropertyName.Equals(nameof(myViewModel.AwayScore)) || updateAll)
            {
                myAwayScoreTextView.Text = myViewModel.AwayScore.ToString();
            }
            if (e.PropertyName.Equals("Shot") || updateAll)
            {
                cardViewLeft.FindViewById<TextView>(Resource.Id.headerText).Text = "Schoten";
                cardViewLeft.FindViewById<TextView>(Resource.Id.statText).Text = myViewModel.GetShotCount().ToString();

                cardViewRight.FindViewById<TextView>(Resource.Id.headerText).Text = "Rebounds";
                cardViewRight.FindViewById<TextView>(Resource.Id.statText).Text = myViewModel.GetReboundCount().ToString();

            }
            if (e.PropertyName.Equals(nameof(myViewModel.CurrentPlayers)) || updateAll)
            {
                myCurrentPlayersLayout.AddViews(myViewModel.GetPlayers());
            }

        }

        private void StatsInput_OkButton_Clicked(object sender, EventArgs e)
        {
            ItemIdHolderRadioGroup statsGroup = statInputSwitcher.CurrentView.FindViewById<ItemIdHolderRadioGroup>(Resource.Id.radioGroup1);
            ItemHolderRadioButton selectedButton = statsGroup.GetSelected();
            myViewModel.CurrentStatistic.SetStatistic(myCurrentStatToGet, selectedButton.ItemId);
            myCurrentStatToGet = myViewModel.CurrentStatistic.GetNextStatistic();
            statsGroup.DeselectAll();
            SetStatisticLayout();
            okButton.Enabled = false;
        }

        private void RadioButtonGroup_CheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            ItemIdHolderRadioGroup radioGroup = sender as ItemIdHolderRadioGroup;
            ItemHolderRadioButton button = radioGroup.FindViewById<ItemHolderRadioButton>(e.CheckedId);
            //ItemHolderRadioButton selectedButton = radioGroup.GetSelected();
            myViewModel.CurrentStatistic.SetStatistic(myCurrentStatToGet, button.ItemId);
            myCurrentStatToGet = myViewModel.CurrentStatistic.GetNextStatistic();
            SetStatisticLayout();
            radioGroup.DeselectAll();
        }

        private void SetStatisticLayout()
        {
            switch (myCurrentStatToGet)
            {
                case EStatisticType.DefensiveRebound:
                    statInputSwitcher.DisplayedChild = statInputSwitcher.IndexOfChild(statInputSwitcher.FindViewById<LinearLayout>(Resource.Id.twoChoice));
                    actionButtonLayout.Visibility = ViewStates.Visible;
                    statInputSwitcher.CurrentView.FindViewById<MultiLineRadioGroup>(Resource.Id.radioGroup1).SetToDefault();
                    break;
                case EStatisticType.Goal:
                case EStatisticType.ConcededGoal:
                case EStatisticType.ConcededShot:
                case EStatisticType.Interception:
                case EStatisticType.Turnover:
                case EStatisticType.Shot:
                case EStatisticType.Assist:
                case EStatisticType.Rebound:
                    statInputSwitcher.DisplayedChild = statInputSwitcher.IndexOfChild(statInputSwitcher.FindViewById<LinearLayout>(Resource.Id.four4Choice));
                    SetPlayers();
                    break;
                case EStatisticType.GoalType:
                    statInputSwitcher.DisplayedChild = statInputSwitcher.IndexOfChild(statInputSwitcher.FindViewById<LinearLayout>(Resource.Id.goaltype));
                    statInputSwitcher.CurrentView.FindViewById<MultiLineRadioGroup>(Resource.Id.radioGroup1).SetToDefault();
                    break;
                default:
                    statInputSwitcher.DisplayedChild = statInputSwitcher.IndexOfChild(statInputSwitcher.FindViewById<LinearLayout>(Resource.Id.buttonLayout));
                    actionButtonLayout.Visibility = ViewStates.Gone;

                    Snackbar snackbar = Snackbar.Make(statInputSwitcher, myViewModel.ExecuteCommand(), Snackbar.LengthLong);
                    snackbar.SetAction("Dismis", v => snackbar.Dismiss());
                    snackbar.Show();
                    break;
            }
            TextView header = statInputSwitcher.CurrentView.FindViewById<TextView>(Resource.Id.headerText);
            if (header != null)
                header.Text = myCurrentStatToGet.GetDescription();
        }

        private void SetCardStatistics(CardView view, string headerText, string statText, string statDetailText)
        {
            view.FindViewById<TextView>(Resource.Id.headerText).Text = headerText;
            view.FindViewById<TextView>(Resource.Id.statText).Text = statText;
            view.FindViewById<TextView>(Resource.Id.statDetailText).Text = statDetailText;
        }

        private void SetPlayers()
        {
            myCurrentPlayers = myViewModel.GetPlayers();
            myCurrentPlayers.Add(ServiceLocator.GetService<PlayersService>().GetUnkownPlayer());
            if (myCurrentStatToGet == EStatisticType.Rebound)
            {
                Player additionalPlayer = new Player {
                    Id = Guid.Empty,
                    FirstName = myViewModel.CurrentFunction == EZoneFunction.Attack ? "Tegenstander" : "LDODK",
                    Number = -1
                };
                myCurrentPlayers.Add(additionalPlayer);
            }
            if (myCurrentStatToGet == EStatisticType.Turnover || myCurrentStatToGet == EStatisticType.Interception)
            {
                Player additionalPlayer = new Player
                {
                    Id = Guid.Empty,
                    FirstName = "Andere reden",
                    Number = -1
                };
                myCurrentPlayers.Add(additionalPlayer);
            }
            var current = statInputSwitcher.CurrentView;
            var rg = current.FindViewById<ItemIdHolderRadioGroup>(Resource.Id.radioGroup1);
            rg.SetPlayers(myCurrentPlayers);
            actionButtonLayout.Visibility = ViewStates.Visible;
        }

        private void ShotclockButton_Click(object sender, EventArgs e)
        {
            myViewModel.CurrentStatistic = new ShotclockoverrideCommand(myViewModel);
            myCurrentStatToGet = myViewModel.CurrentStatistic.GetNextStatistic();
            SetStatisticLayout();
        }

        private void TurnoverButton_Click(object sender, EventArgs e)
        {
            if (myViewModel.CurrentFunction == EZoneFunction.Attack)
                myViewModel.CurrentStatistic = new TurnoverCommand(myViewModel);
            else
                myViewModel.CurrentStatistic = new InterceptionCommand(myViewModel);
            myCurrentStatToGet = myViewModel.CurrentStatistic.GetNextStatistic();
            SetStatisticLayout();
        }
        private void GoalButton_Click(object sender, EventArgs e)
        {
            if (myViewModel.CurrentFunction == EZoneFunction.Attack)
                myViewModel.CurrentStatistic = new GoalCommand(myViewModel);
            else
                myViewModel.CurrentStatistic = new GoalConcededCommand(myViewModel);
            myCurrentStatToGet = myViewModel.CurrentStatistic.GetNextStatistic();
            SetStatisticLayout();
        }

        private void ShotButton_Click(object sender, EventArgs e)
        {
            if (myViewModel.CurrentFunction == EZoneFunction.Attack)
                myViewModel.CurrentStatistic = new ShotCommand(myViewModel);
            else
                myViewModel.CurrentStatistic = new ConcededShotCommand(myViewModel);
            myCurrentStatToGet = myViewModel.CurrentStatistic.GetNextStatistic();
            SetStatisticLayout();
        }

        private void SubstitutePlayer()
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
            LayoutInflater inflater = LayoutInflater;
            View view = inflater.Inflate(Resource.Layout.substitute_layout, null);

            MultiLineRadioGroup inGameGroup = view.FindViewById<MultiLineRadioGroup>(Resource.Id.inGamePlayersGroup);
            MultiLineRadioGroup subsGroup = view.FindViewById<MultiLineRadioGroup>(Resource.Id.subsGroup);

            inGameGroup.SetPlayers(myViewModel.InGamePlayers);
            subsGroup.SetPlayers(myViewModel.SubstituePlayers);

            //string text = MainViewModel.Instance.GetDb().ToString();
            builder.SetView(view)
                .SetPositiveButton("OK", (s, args) =>
                {
                    Guid inGamePlayer = inGameGroup.GetSelected().ItemId;
                    Guid sub = subsGroup.GetSelected().ItemId;
                    if (inGamePlayer == null || sub == null)
                        return;
                    myViewModel.Substitute(inGamePlayer, sub);
                })
                .SetNegativeButton("Cancel", (s, args) => {
                });

            builder.Show();
        }

    }
}