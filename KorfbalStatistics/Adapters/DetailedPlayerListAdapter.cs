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
using KorfbalStatistics.Model;

namespace KorfbalStatistics.Adapters
{
    public class DetailedPlayerListAdapter : ArrayAdapter<DbPlayer>
    {
        public DetailedPlayerListAdapter(List<DbPlayer> data, Activity context) : base(context, Resource.Layout.detailed_player_view)
        {
            mydata = data;
            myContext = context;
        }

        private List<DbPlayer> mydata;
        private Activity myContext;

        public override long GetItemId(int position)
        {
            return base.GetItemId(position);
        }

        new public DbPlayer GetItem(int position)
        {
            return mydata[position];
        }

        public override int Count
        {
            get { return mydata.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = mydata[position];
            View view = convertView;
            if (view == null)
                view = myContext.LayoutInflater.Inflate(Resource.Layout.detailed_player_view, null);
            view.FindViewById<TextView>(Resource.Id.playerAbbrevation).Text = item.Abbrevation;
            view.FindViewById<TextView>(Resource.Id.playerName).Text = item.Name;
            view.FindViewById<TextView>(Resource.Id.playerNumber).Text = item.Number.ToString();
            return view;
        }
    }
}