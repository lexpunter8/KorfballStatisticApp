using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using KorfbalStatistics.CustomviewClasses;
using KorfbalStatistics.Model;

namespace KorfbalStatistics.Adapters
{
    public class AttackListAdapter : BaseExpandableListAdapter
    {
        public AttackListAdapter(List<Attack> data, Activity context)
        {
            mydata = data;
            myContext = context;
        }

        public override int GroupCount => mydata.Count;

        public override bool HasStableIds => true;

        private List<Attack> mydata { get; set; }
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
            return mydata[groupPosition].Shots.Count + mydata[groupPosition].Rebounds.Count +
                (mydata[groupPosition].Goal != null 
                || mydata[groupPosition].DbAttack.TurnoverPlayerId != null
                || mydata[groupPosition].DbAttack.IsSchotClockOverride ? 1 : 0 );
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            Attack item = mydata[groupPosition];
            View view = convertView;
            if (view == null)
                view = myContext.LayoutInflater.Inflate(Resource.Layout.attack_expanded_listview, null);
            TextView text1 = view.FindViewById<TextView>(Resource.Id.textView1);
            TextView text2 = view.FindViewById<TextView>(Resource.Id.textView2);
            TextView text3 = view.FindViewById<TextView>(Resource.Id.textView3);
            TextView text4 = view.FindViewById<TextView>(Resource.Id.textView4);

            PlayerDbManager dbPlayerManager = DbManager.Instance.PlayerDbManager;

            if (childPosition >= item.Shots.Count + item.Rebounds.Count)
            {
                if (item.Goal != null)
                {
                    text1.Text = "Goal";
                    text2.Text = dbPlayerManager.GetPlayerById(item.Goal.PlayerId).FirstName;
                    text3.Text = dbPlayerManager.GetPlayerById(item.Goal.AssistPlayerId).FirstName;
                    text4.Text = DbManager.Instance.GameDbManager.GetGoalTypeById(item.Goal.GoalTypeId).Name;
                }
                else if (item.DbAttack.IsSchotClockOverride)
                {
                    text1.Text = "SCO";
                    text2.Text = "";
                    text3.Text = "";
                    text4.Text = "";
                }
                else if (item.DbAttack.TurnoverPlayerId != null)
                {
                    text1.Text = "TO";
                    text2.Text = dbPlayerManager.GetPlayerById(item.DbAttack.TurnoverPlayerId.Value).FirstName;
                    text3.Text = "";
                    text4.Text = "";
                }
            }
            else
            {
                if (childPosition >= item.Shots.Count)
                {
                    text1.Text = "Rebound";
                    text2.Text = dbPlayerManager.GetPlayerById(item.Rebounds[childPosition - item.Shots.Count].PlayerId).FirstName;

                    text3.Text = "Count: " + item.Rebounds[childPosition - item.Shots.Count].Count;
                }
                else
                {
                    text1.Text = "Shot";
                    text2.Text = dbPlayerManager.GetPlayerById(item.Shots[childPosition].PlayerId).FirstName;

                    text3.Text = "Count: " + item.Shots[childPosition].Count;
                }
                text4.Text = "";
            }
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
                view = myContext.LayoutInflater.Inflate(Resource.Layout.list_attack_view, null);
            bool defence = item.DbAttack.IsOpponentAttack;
            RoundedTextView rt = view.FindViewById<RoundedTextView>(Resource.Id.functionText);
            rt.Text = defence ? "D" : "A";
            // RelativeLayout icons = view.FindViewById<RelativeLayout>(Resource.Id.icons);
            int shotCount = 0;
            item.Shots.ForEach(s => shotCount += s.Count);
            view.FindViewById<TextView>(Resource.Id.shotCountText).Text = shotCount.ToString();
            int reboundCount = 0;
            item.Rebounds.ForEach(r => reboundCount += r.Count);
            view.FindViewById<TextView>(Resource.Id.reboundCountText).Text = reboundCount.ToString();

            ImageView attackAndAction = view.FindViewById<ImageView>(Resource.Id.attackEndAction);
            if (item.Goal != null)
            {
                attackAndAction.SetImageResource(Resource.Drawable.ic_ball);
                int c = myContext.GetColor(defence ? Resource.Color.accent : Resource.Color.primary);
                Color color = new Color(c);
                attackAndAction.SetColorFilter(color);
            }
            else
                attackAndAction.SetImageDrawable(null);
            //view.FindViewById<TextView>(Resource.Id.statusText).Text = "W";
            //view.FindViewById<TextView>(Resource.Id.dateText).Text = item.Date.ToString("dd-MM-yyyy HH:mm");
            return view;
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }
    }
}